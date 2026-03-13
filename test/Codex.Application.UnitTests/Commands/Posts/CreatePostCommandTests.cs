using Codex.Application.Commands.Posts;
using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Posts;

public sealed class CreatePostCommandTests
{
    private readonly CreatePostCommandHandler _commandHandler;

    private readonly IPostRepository _postRepositoryMock;
    private readonly IAuthorRepository _authorRepositoryMock;
    private readonly ICategoryRepository _categoryRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public CreatePostCommandTests()
    {
        _postRepositoryMock = Substitute.For<IPostRepository>();
        _authorRepositoryMock = Substitute.For<IAuthorRepository>();
        _categoryRepositoryMock = Substitute.For<ICategoryRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new CreatePostCommandHandler(
            _postRepositoryMock,
            _authorRepositoryMock,
            _categoryRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldCreatePost_WhenParametersAreValid()
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _authorRepositoryMock
            .ExistsByIdAsync(post.Author.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _categoryRepositoryMock
            .ExistsByIdAsync(post.Category.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _postRepositoryMock
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(post);

        CreatePostCommand command = new(
            post.Title,
            post.Content,
            post.Author.Id,
            post.Category.Id);

        Result<PostDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Title.ShouldBe(post.Title);
        result.Value.Content.ShouldBe(post.Content);
        result.Value.Status.ShouldBe(post.Status);
        result.Value.PublishedAtUtc.ShouldBe(post.PublishedAtUtc);
        result.Value.Author.Id.ShouldBe(post.Author.Id);
        result.Value.Category.Id.ShouldBe(post.Category.Id);

        await _authorRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.AuthorId, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.CategoryId, Arg.Any<CancellationToken>());

        await _postRepositoryMock
            .Received(1)
            .CreateAsync(Arg.Any<Post>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenAuthorDoesNotExist()
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _authorRepositoryMock
            .ExistsByIdAsync(post.Author.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        CreatePostCommand command = new(
            post.Title,
            post.Content,
            post.Author.Id,
            post.Category.Id);

        Result<PostDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.NotFound);

        await _authorRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.AuthorId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCategoryDoesNotExist()
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _authorRepositoryMock
            .ExistsByIdAsync(post.Author.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _categoryRepositoryMock
            .ExistsByIdAsync(post.Category.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        CreatePostCommand command = new(
            post.Title,
            post.Content,
            post.Author.Id,
            post.Category.Id);

        Result<PostDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CategoryErrors.NotFound);

        await _authorRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.AuthorId, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.CategoryId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnTitleIsRequired_WhenTitleIsEmptyOrWhitespace(string title)
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _authorRepositoryMock
            .ExistsByIdAsync(post.Author.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _categoryRepositoryMock
            .ExistsByIdAsync(post.Category.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        CreatePostCommand command = new(
            title,
            post.Content,
            post.Author.Id,
            post.Category.Id);

        Result<PostDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.TitleIsRequired);

        await _authorRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.AuthorId, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.CategoryId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnContentIsRequired_WhenContentIsEmptyOrWhitespace(string content)
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _authorRepositoryMock
            .ExistsByIdAsync(post.Author.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _categoryRepositoryMock
            .ExistsByIdAsync(post.Category.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        CreatePostCommand command = new(
            post.Title,
            content,
            post.Author.Id,
            post.Category.Id);

        Result<PostDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.ContentIsRequired);

        await _authorRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.AuthorId, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.CategoryId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthorIdIsRequired_WhenAuthorIdIsEmpty()
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _authorRepositoryMock
            .ExistsByIdAsync(Guid.Empty, Arg.Any<CancellationToken>())
            .Returns(true);

        _categoryRepositoryMock
            .ExistsByIdAsync(post.Category.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        CreatePostCommand command = new(
            post.Title,
            post.Content,
            Guid.Empty,
            post.Category.Id);

        Result<PostDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.AuthorIdIsRequired);

        await _authorRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.AuthorId, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.CategoryId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnCategoryIdIsRequired_WhenCategoryIdIsEmpty()
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _authorRepositoryMock
            .ExistsByIdAsync(post.Author.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _categoryRepositoryMock
            .ExistsByIdAsync(Guid.Empty, Arg.Any<CancellationToken>())
            .Returns(true);

        CreatePostCommand command = new(
            post.Title,
            post.Content,
            post.Author.Id,
            Guid.Empty);

        Result<PostDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.CategoryIdIsRequired);

        await _authorRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.AuthorId, Arg.Any<CancellationToken>());

        await _categoryRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.CategoryId, Arg.Any<CancellationToken>());
    }
}