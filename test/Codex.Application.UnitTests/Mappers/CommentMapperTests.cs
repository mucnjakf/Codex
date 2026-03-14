using Codex.Application.Dtos.Mappers;
using Codex.Domain.Entities;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Application.UnitTests.Mappers;

public sealed class CommentMapperTests
{
    [Fact]
    public void ToCommentDto_ShouldReturnCommentDto()
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        var commentDto = comment.ToCommentDto();

        commentDto.ShouldNotBeNull();
        commentDto.Id.ShouldBe(comment.Id);
        commentDto.CreatedAtUtc.ShouldBe(comment.CreatedAtUtc);
        commentDto.UpdatedAtUtc.ShouldBe(comment.UpdatedAtUtc);
        commentDto.Content.ShouldBe(comment.Content);
        commentDto.Post.Id.ShouldBe(comment.Post.Id);
        commentDto.Reader.Id.ShouldBe(comment.Reader.Id);
    }
}