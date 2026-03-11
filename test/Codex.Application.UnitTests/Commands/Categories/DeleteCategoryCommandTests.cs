using Codex.Application.Commands.Categories;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Categories;

public sealed class DeleteCategoryCommandTests
{
    private readonly DeleteCategoryCommandHandler _commandHandler;

    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public DeleteCategoryCommandTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new DeleteCategoryCommandHandler(_categoryRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldDeleteCategory_WhenParametersAreValid()
    {
        Category category = CategoryData.Category;

        _categoryRepositoryMock
            .GetWithPostsAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        _categoryRepositoryMock
            .DeleteAsync(category, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        DeleteCategoryCommand command = new(category.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _categoryRepositoryMock
            .Received(1)
            .GetWithPostsAsync(category.Id, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .DeleteAsync(category, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCategoryDoesNotExist()
    {
        Guid categoryId = CategoryData.Id;

        _categoryRepositoryMock
            .GetWithPostsAsync(categoryId, Arg.Any<CancellationToken>())
            .Returns((Category)null!);

        DeleteCategoryCommand command = new(categoryId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.NotFound);

        await _categoryRepositoryMock
            .Received(1)
            .GetWithPostsAsync(categoryId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnCannotDeleteContainsPostsError_WhenCategoryContainsErrors()
    {
        Category category = CategoryData.CategoryWithPosts();

        _categoryRepositoryMock
            .GetWithPostsAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        DeleteCategoryCommand command = new(category.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.CannotDeleteContainsPosts);

        await _categoryRepositoryMock
            .Received(1)
            .GetWithPostsAsync(category.Id, Arg.Any<CancellationToken>());
    }
}