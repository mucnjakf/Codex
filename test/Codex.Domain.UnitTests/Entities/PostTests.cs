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

        result.Error.ShouldBe(Error.None);
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
}