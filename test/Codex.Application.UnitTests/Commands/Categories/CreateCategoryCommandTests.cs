using Codex.Application.Commands.Categories;
using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Categories;

public sealed class CreateCategoryCommandTests
{
    private readonly CreateCategoryCommandHandler _commandHandler;

    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoryCommandTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _commandHandler = new CreateCategoryCommandHandler(_categoryRepositoryMock, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ShouldCreateCategory_WhenParametersAreValid()
    {
        _categoryRepositoryMock
            .CreateAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWork
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        CreateCategoryCommand command = new(CategoryData.Name);

        Result<CategoryDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.Name.ShouldBe(CategoryData.Name);

        await _categoryRepositoryMock
            .Received(1)
            .CreateAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnNameIsRequiredError_WhenNameIsEmptyOrWhitespace(string name)
    {
        CreateCategoryCommand command = new(name);

        Result<CategoryDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CategoryErrors.NameIsRequired);

        await _categoryRepositoryMock
            .Received(0)
            .CreateAsync(Arg.Any<Category>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}