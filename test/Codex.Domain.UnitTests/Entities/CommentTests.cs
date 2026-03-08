using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Domain.UnitTests.Bootstrapper;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Domain.UnitTests.Entities;

public sealed class CommentTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnComment_WhenParametersAreValid()
    {
        Guid postId = PostData.Id;
        Guid readerId = ReaderData.Id;

        Result<Comment> result = Comment.Create(CommentData.Content, postId, readerId);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Content.ShouldBe(CommentData.Content);
        result.Value.PostId.ShouldBe(postId);
        result.Value.ReaderId.ShouldBe(readerId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnContentIsRequiredError_WhenContentIsEmptyOrWhitespace(string content)
    {
        Guid postId = PostData.Id;
        Guid readerId = ReaderData.Id;

        Result<Comment> result = Comment.Create(content, postId, readerId);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CommentErrors.ContentIsRequired);
    }

    [Fact]
    public void Create_ShouldReturnPostIdIsRequiredError_WhenPostIdIsEmpty()
    {
        Result<Comment> result = Comment.Create(CommentData.Content, Guid.Empty, ReaderData.Id);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CommentErrors.PostIdIsRequired);
    }

    [Fact]
    public void Create_ShouldReturnReaderIdIsRequiredError_WhenReaderIdIsEmpty()
    {
        Result<Comment> result = Comment.Create(CommentData.Content, PostData.Id, Guid.Empty);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CommentErrors.ReaderIdIsRequired);
    }

    [Fact]
    public void Update_ShouldReturnSuccess_WhenParametersAreValid()
    {
        Comment comment = CommentData.Comment;

        const string content = "New content";

        Result result = comment.Update(content);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        comment.UpdatedAtUtc.ShouldNotBeNull();

        comment.Content.ShouldBe(content);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnContentIsRequiredError_WhenContentIsEmptyOrWhitespace(string content)
    {
        Comment comment = CommentData.Comment;

        Result result = comment.Update(content);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CommentErrors.ContentIsRequired);
    }
}