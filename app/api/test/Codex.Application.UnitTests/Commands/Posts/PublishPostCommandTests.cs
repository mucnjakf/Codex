using Codex.Application.Commands.Posts;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Posts;

public sealed class PublishPostCommandTests
{
    private readonly PublishPostCommandHandler _commandHandler;

    private readonly IPostRepository _postRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public PublishPostCommandTests()
    {
        _postRepositoryMock = Substitute.For<IPostRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new PublishPostCommandHandler(
            _postRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldPublishPost_WhenParameterAreValid()
    {
        Post post = PostData.Post;

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        PublishPostCommand command = new(post.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());

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

        PublishPostCommand command = new(postId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.NotFound);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(postId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnPublishOnlyDraft_WhenPostIsNotInStatusDraft()
    {
        Post post = PostData.PostWithStatusPublished();

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        PublishPostCommand command = new(post.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.PublishOnlyDraft);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>());
    }
}