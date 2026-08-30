using Codex.Application.Commands.Posts;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Posts;

public sealed class DeletePostCommandTests
{
    private readonly DeletePostCommandHandler _commandHandler;

    private readonly IPostRepository _postRepositoryMock;
    private readonly ICommentRepository _commentRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public DeletePostCommandTests()
    {
        _postRepositoryMock = Substitute.For<IPostRepository>();
        _commentRepositoryMock = Substitute.For<ICommentRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new DeletePostCommandHandler(
            _postRepositoryMock,
            _commentRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldDeletePost_WhenParametersAreValid()
    {
        Post post = PostData.Post;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _commentRepositoryMock
            .ExistsByPostIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        DeletePostCommand command = new(post.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _commentRepositoryMock
            .Received(1)
            .ExistsByPostIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenPostDoesNotExist()
    {
        Guid postId = PostData.Id;

        _postRepositoryMock
            .GetByIdAsync(postId, Arg.Any<CancellationToken>())
            .Returns((Post)null!);

        DeletePostCommand command = new(postId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.NotFound);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(postId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnCannotDeleteContainsCommentsError_WhenPostContainsComments()
    {
        Post post = PostData.Post;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _commentRepositoryMock
            .ExistsByPostIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        DeletePostCommand command = new(post.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.CannotDeleteContainsComments);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

        await _commentRepositoryMock
            .Received(1)
            .ExistsByPostIdAsync(post.Id, Arg.Any<CancellationToken>());
    }
}