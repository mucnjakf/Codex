using Codex.Application.Commands.Categories;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Categories;

public sealed class UpdateCategoryCommandTests
{
    private readonly UpdateCategoryCommandHandler _commandHandler;

    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public UpdateCategoryCommandTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new UpdateCategoryCommandHandler(_categoryRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldUpdateCategory_WhenParametersAreValid()
    {
        Category category = CategoryData.Category;

        _categoryRepositoryMock
            .GetAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        UpdateCategoryCommand command = new(category.Id, "New category name");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _categoryRepositoryMock
            .Received(1)
            .GetAsync(category.Id, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCategoryNotFound()
    {
        Guid categoryId = CategoryData.Id;

        _categoryRepositoryMock
            .GetAsync(categoryId, Arg.Any<CancellationToken>())
            .Returns((Category)null!);

        UpdateCategoryCommand command = new(categoryId, "New category name");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.NotFound);

        await _categoryRepositoryMock
            .Received(1)
            .GetAsync(categoryId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnNameIsRequiredError_WhenNameIsEmptyOrWhitespace(string name)
    {
        Category category = CategoryData.Category;

        _categoryRepositoryMock
            .GetAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        UpdateCategoryCommand command = new(category.Id, name);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.NameIsRequired);

        await _categoryRepositoryMock
            .Received(1)
            .GetAsync(category.Id, Arg.Any<CancellationToken>());
    }
}