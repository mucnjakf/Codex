using Codex.Application.Commands.Posts;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Posts;

public sealed class UpdatePostCommandTests
{
    private readonly UpdatePostCommandHandler _commandHandler;

    private readonly IPostRepository _postRepositoryMock;
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public UpdatePostCommandTests()
    {
        _postRepositoryMock = Substitute.For<IPostRepository>();
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new UpdatePostCommandHandler(
            _postRepositoryMock,
            _categoryRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldUpdatePost_WhenParametersAreValid()
    {
        Post post = PostData.Post;
        Guid categoryId = CategoryData.Id;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _categoryRepositoryMock
            .ExistsByIdAsync(categoryId)
            .Returns(true);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        UpdatePostCommand command = new(
            post.Id,
            "New post title",
            "New post content",
            categoryId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(categoryId, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPostDoesNotExist()
    {
        Post post = PostData.Post;
        Guid categoryId = CategoryData.Id;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns((Post)null!);

        UpdatePostCommand command = new(
            post.Id,
            "New post title",
            "New post content",
            categoryId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.NotFound);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCategoryDoesNotExist()
    {
        Post post = PostData.Post;
        Guid categoryId = CategoryData.Id;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _categoryRepositoryMock
            .ExistsByIdAsync(categoryId)
            .Returns(false);

        UpdatePostCommand command = new(
            post.Id,
            "New post title",
            "New post content",
            categoryId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.NotFound);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(categoryId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnTitleIsRequiredError_WhenTitleIsEmptyOrWhitespace(string title)
    {
        Post post = PostData.Post;
        Guid categoryId = CategoryData.Id;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _categoryRepositoryMock
            .ExistsByIdAsync(categoryId)
            .Returns(true);

        UpdatePostCommand command = new(
            post.Id,
            title,
            "New post content",
            categoryId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.TitleIsRequired);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(categoryId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnContentIsRequiredError_WhenContentIsEmptyOrWhitespace(string content)
    {
        Post post = PostData.Post;
        Guid categoryId = CategoryData.Id;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _categoryRepositoryMock
            .ExistsByIdAsync(categoryId)
            .Returns(true);

        UpdatePostCommand command = new(
            post.Id,
            "New post title",
            content,
            categoryId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.ContentIsRequired);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(categoryId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryIdIsRequiredError_WhenCategoryIdIsEmpty()
    {
        Post post = PostData.Post;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _categoryRepositoryMock
            .ExistsByIdAsync(Guid.Empty)
            .Returns(true);

        UpdatePostCommand command = new(
            post.Id,
            "New post title",
            "New post content",
            Guid.Empty);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.CategoryIdIsRequired);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.CategoryId, Arg.Any<CancellationToken>());
    }
}