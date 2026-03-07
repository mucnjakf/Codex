using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Domain.UnitTests.Bootstrapper;
using Codex.Domain.UnitTests.Bootstrapper.Data;
using Shouldly;

namespace Codex.Domain.UnitTests.Entities;

public sealed class CommentTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnComment_WhenParametersAreValid()
    {
        Guid postId = CommentData.PostId;
        Guid readerId = CommentData.ReaderId;

        Result<Comment> result = Comment.Create(
            CommentData.Content,
            postId,
            readerId);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Content.ShouldBe(CommentData.Content);
        result.Value.PostId.ShouldBe(postId);
        result.Value.ReaderId.ShouldBe(readerId);

        result.Error.ShouldBe(Error.None);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnFailureAndContentIsRequiredError_WhenContentIsEmptyOrWhitespace(string content)
    {
        Guid postId = CommentData.PostId;
        Guid readerId = CommentData.ReaderId;

        Result<Comment> result = Comment.Create(content, postId, readerId);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CommentErrors.ContentIsRequired);
    }

    [Fact]
    public void Create_ShouldReturnFailureAndPostIdIsRequiredError_WhenPostIdIsEmpty()
    {
        Result<Comment> result = Comment.Create(CommentData.Content, Guid.Empty, CommentData.ReaderId);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CommentErrors.PostIdIsRequired);
    }

    [Fact]
    public void Create_ShouldReturnFailureAndReaderIdIsRequiredError_WhenReaderIdIsEmpty()
    {
        Result<Comment> result = Comment.Create(CommentData.Content, CommentData.PostId, Guid.Empty);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CommentErrors.ReaderIdIsRequired);
    }
}