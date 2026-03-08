using Codex.Domain.Entities;
using Codex.Domain.Enumerations;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Domain.UnitTests.Bootstrapper;
using Codex.Domain.UnitTests.Bootstrapper.Data;
using Shouldly;

namespace Codex.Domain.UnitTests.Entities;

public sealed class PostTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnPost_WhenParametersAreValid()
    {
        Guid authorId = AuthorData.Id;
        Guid categoryId = CategoryData.Id;

        Result<Post> result = Post.Create(PostData.Title, PostData.Content, authorId, categoryId);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Title.ShouldBe(PostData.Title);
        result.Value.Content.ShouldBe(PostData.Content);
        result.Value.Status.ShouldBe(PostStatus.Draft);
        result.Value.PublishedAtUtc.ShouldBeNull();
        result.Value.AuthorId.ShouldBe(authorId);
        result.Value.CategoryId.ShouldBe(categoryId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnTitleIsRequiredError_WhenTitleIsEmptyOrWhitespace(string title)
    {
        Result<Post> result = Post.Create(title, PostData.Content, AuthorData.Id, CategoryData.Id);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.TitleIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnContentIsRequiredError_WhenContentIsEmptyOrWhitespace(string content)
    {
        Result<Post> result = Post.Create(PostData.Title, content, AuthorData.Id, CategoryData.Id);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.ContentIsRequired);
    }

    [Fact]
    public void Create_ShouldReturnAuthorIsRequiredError_WhenAuthorIdIsEmpty()
    {
        Result<Post> result = Post.Create(PostData.Title, PostData.Content, Guid.Empty, CategoryData.Id);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.AuthorIdIsRequired);
    }

    [Fact]
    public void Create_ShouldReturnCategoryIsRequiredError_WhenCategoryIdIsEmpty()
    {
        Result<Post> result = Post.Create(PostData.Title, PostData.Content, AuthorData.Id, Guid.Empty);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.CategoryIdIsRequired);
    }

    [Fact]
    public void Update_ShouldReturnSuccess_WhenParametersAreValid()
    {
        Post post = PostData.Post;

        const string title = "New title";
        const string content = "New content";
        var categoryId = Guid.CreateVersion7();

        Result result = post.Update(title, content, categoryId);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        post.UpdatedAtUtc.ShouldNotBeNull();

        post.Title.ShouldBe(title);
        post.Content.ShouldBe(content);
        post.CategoryId.ShouldBe(categoryId);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnTitleIsRequiredError_WhenTitleIsEmptyOrWhitespace(string title)
    {
        Post post = PostData.Post;

        Result result = post.Update(title, "New content", Guid.CreateVersion7());

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.TitleIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnContentIsRequiredError_WhenContentIsEmptyOrWhitespace(string content)
    {
        Post post = PostData.Post;

        Result result = post.Update("New title", content, Guid.CreateVersion7());

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.ContentIsRequired);
    }

    [Fact]
    public void Update_ShouldReturnCategoryIdIsRequiredError_WhenCategoryIdIsEmpty()
    {
        Post post = PostData.Post;

        Result result = post.Update("New title", "New content", Guid.Empty);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.CategoryIdIsRequired);
    }

    [Fact]
    public void Publish_ShouldUpdateStatusAndPublishedAtUtc()
    {
        Post post = PostData.Post;

        Result result = post.Publish();

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        post.Status.ShouldBe(PostStatus.Published);
        post.PublishedAtUtc.ShouldNotBeNull();
        post.UpdatedAtUtc.ShouldNotBeNull();
    }

    [Fact]
    public void Publish_ShouldReturnPublishOnlyDraftError_WhenStatusIsNotDraft()
    {
        Post post = PostData.Post;
        post.Publish();

        Result result = post.Publish();

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(PostErrors.PublishOnlyDraft);
    }
}