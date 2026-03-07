using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Events;
using Codex.Domain.Outcomes;
using Codex.Domain.UnitTests.Bootstrapper;
using Codex.Domain.UnitTests.Bootstrapper.Data;
using Shouldly;

namespace Codex.Domain.UnitTests.Entities;

public sealed class CategoryTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnCategory_WhenParametersAreValid()
    {
        Result<Category> result = Category.Create(
            CategoryData.Name,
            CategoryData.CreatedAtUtc);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(CategoryData.Name);
        result.Value.Posts.ShouldBeEmpty();
        result.Value.CreatedAtUtc.ShouldBe(CategoryData.CreatedAtUtc);
        result.Value.UpdatedAtUtc.ShouldBeNull();

        result.IsFailure.ShouldBeFalse();
        result.Error.ShouldBe(Error.None);
    }

    [Fact]
    public void Create_ShouldRaiseCategoryCreatedDomainEvent_WhenCreatedSuccessfully()
    {
        Result<Category> result = Category.Create(
            CategoryData.Name,
            CategoryData.CreatedAtUtc);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        var domainEvent = AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(result.Value);
        domainEvent.CategoryId.ShouldBe(result.Value.Id);
    }

    [Fact]
    public void Create_ShouldReturnFailureAndNameIsRequiredError_WhenNameIsWhitespace()
    {
        Result<Category> result = Category.Create(
            string.Empty,
            CategoryData.CreatedAtUtc);

        result.IsSuccess.ShouldBeFalse();

        result.IsFailure.ShouldBeTrue();
        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CategoryErrors.NameIsRequired);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIds_WhenCreatedMultipleTimes()
    {
        Result<Category> result1 = Category.Create(CategoryData.Name, CategoryData.CreatedAtUtc);
        Result<Category> result2 = Category.Create(CategoryData.Name, CategoryData.CreatedAtUtc);

        result1.IsSuccess.ShouldBeTrue();
        result1.IsFailure.ShouldBeFalse();
        result2.IsSuccess.ShouldBeTrue();
        result2.IsFailure.ShouldBeFalse();
        result1.Value.ShouldNotBeNull();
        result2.Value.ShouldNotBeNull();
        result1.Value.Id.ShouldNotBe(result2.Value.Id);
    }
}