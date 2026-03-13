using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Queries.Posts;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Posts;

public sealed class GetPostQueryTests
{
    private readonly GetPostQueryHandler _queryHandler;

    private readonly IPostRepository _postRepositoryMock;

    public GetPostQueryTests()
    {
        _postRepositoryMock = Substitute.For<IPostRepository>();

        _queryHandler = new GetPostQueryHandler(_postRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnPost_WhenPostExists()
    {
        Post post = PostData.PostWithAuthorAndCategory();

        _postRepositoryMock
            .GetByIdAsync(post.Id, Arg.Any<CancellationToken>())
            .Returns(post);

        GetPostQuery query = new(post.Id);

        Result<PostDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(post.Id);
        result.Value.CreatedAtUtc.ShouldBe(post.CreatedAtUtc);
        result.Value.UpdatedAtUtc.ShouldBe(post.UpdatedAtUtc);
        result.Value.Title.ShouldBe(post.Title);
        result.Value.Content.ShouldBe(post.Content);
        result.Value.Status.ShouldBe(post.Status);
        result.Value.PublishedAtUtc.ShouldBe(post.PublishedAtUtc);
        result.Value.Author.Id.ShouldBe(post.Author.Id);
        result.Value.Category.Id.ShouldBe(post.Category.Id);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(post.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenPostDoesNotExist()
    {
        Guid postId = PostData.Id;

        _postRepositoryMock
            .GetByIdAsync(postId, Arg.Any<CancellationToken>())
            .Returns((Post)null!);

        GetPostQuery query = new(postId);

        Result<PostDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.NotFound);

        await _postRepositoryMock
            .Received(1)
            .GetByIdAsync(postId);
    }
}