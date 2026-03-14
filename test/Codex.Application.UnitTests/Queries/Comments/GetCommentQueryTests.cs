using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Queries.Comments;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Comments;

public sealed class GetCommentQueryTests
{
     private readonly GetCommentQueryHandler _queryHandler;

    private readonly ICommentRepository _commentRepositoryMock;

    public GetCommentQueryTests()
    {
        _commentRepositoryMock = Substitute.For<ICommentRepository>();

        _queryHandler = new GetCommentQueryHandler(_commentRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnComment_WhenCommentExists()
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        _commentRepositoryMock
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>())
            .Returns(comment);

        GetCommentQuery query = new(comment.Id);

        Result<CommentDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(comment.Id);
        result.Value.CreatedAtUtc.ShouldBe(comment.CreatedAtUtc);
        result.Value.UpdatedAtUtc.ShouldBe(comment.UpdatedAtUtc);
        result.Value.Content.ShouldBe(comment.Content);
        result.Value.Post.Id.ShouldBe(comment.Post.Id);
        result.Value.Reader.Id.ShouldBe(comment.Reader.Id);

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(comment.Id);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenCommentDoesNotExist()
    {
        Guid commentId = CommentData.Id;

        _commentRepositoryMock
            .GetByIdAsync(commentId, Arg.Any<CancellationToken>())
            .Returns((Comment)null!);

        GetCommentQuery query = new(commentId);

        Result<CommentDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CommentErrors.NotFound);

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(commentId);
    }
}