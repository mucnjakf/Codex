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
    private readonly IPostRepository _postRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public DeleteCategoryCommandTests()
    {
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _postRepositoryMock = Substitute.For<IPostRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new DeleteCategoryCommandHandler(
            _categoryRepositoryMock,
            _postRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldDeleteCategory_WhenParametersAreValid()
    {
        Category category = CategoryData.Category;

        _categoryRepositoryMock
            .GetByIdAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        _postRepositoryMock
            .ExistsByCategoryIdAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(false);

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
            .GetByIdAsync(category.Id, Arg.Any<CancellationToken>());

        await _postRepositoryMock
            .Received(1)
            .ExistsByCategoryIdAsync(category.Id, Arg.Any<CancellationToken>());

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
            .GetByIdAsync(categoryId, Arg.Any<CancellationToken>())
            .Returns((Category)null!);

        DeleteCategoryCommand command = new(categoryId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.NotFound);

        await _categoryRepositoryMock
            .Received(1)
            .GetByIdAsync(categoryId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnCannotDeleteContainsPostsError_WhenCategoryContainsPosts()
    {
        Category category = CategoryData.CategoryWithPosts();

        _categoryRepositoryMock
            .GetByIdAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(category);

        _postRepositoryMock
            .ExistsByCategoryIdAsync(category.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        DeleteCategoryCommand command = new(category.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.CannotDeleteContainsPosts);

        await _categoryRepositoryMock
            .Received(1)
            .GetByIdAsync(category.Id, Arg.Any<CancellationToken>());

        await _postRepositoryMock
            .Received(1)
            .ExistsByCategoryIdAsync(category.Id, Arg.Any<CancellationToken>());
    }
}