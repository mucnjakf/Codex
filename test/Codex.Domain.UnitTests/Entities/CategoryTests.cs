using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Events;
using Codex.Domain.Outcomes;
using Codex.Domain.UnitTests.Bootstrapper;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Domain.UnitTests.Entities;

public sealed class CategoryTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnCategory_WhenParametersAreValid()
    {
        Result<Category> result = Category.Create(CategoryData.Name);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Name.ShouldBe(CategoryData.Name);
    }

    [Fact]
    public void Create_ShouldRaiseCategoryCreatedDomainEvent_WhenCreatedSuccessfully()
    {
        Result<Category> result = Category.Create(CategoryData.Name);

        result.IsSuccess.ShouldBeTrue();
        result.Value.ShouldNotBeNull();

        var domainEvent = AssertDomainEventWasPublished<CategoryCreatedDomainEvent>(result.Value);
        domainEvent.CategoryId.ShouldBe(result.Value.Id);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnNameIsRequiredError_WhenNameIsEmptyOrWhitespace(string name)
    {
        Result<Category> result = Category.Create(name);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CategoryErrors.NameIsRequired);
    }

    [Fact]
    public void Create_ShouldGenerateUniqueIds_WhenCreatedMultipleTimes()
    {
        Result<Category> result1 = Category.Create(CategoryData.Name);
        Result<Category> result2 = Category.Create(CategoryData.Name);

        result1.IsSuccess.ShouldBeTrue();
        result2.IsSuccess.ShouldBeTrue();

        result1.IsFailure.ShouldBeFalse();
        result2.IsFailure.ShouldBeFalse();

        result1.Value.ShouldNotBeNull();
        result2.Value.ShouldNotBeNull();
        result1.Value.Id.ShouldNotBe(result2.Value.Id);
    }

    [Fact]
    public void Update_ShouldReturnSuccess_WhenParametersAreValid()
    {
        Category category = CategoryData.Category;

        const string name = "New name";

        Result result = category.Update(name);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        category.UpdatedAtUtc.ShouldNotBeNull();

        category.Name.ShouldBe(name);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnNameIsRequiredError_WhenNameIsEmptyOrWhitespace(string name)
    {
        Category category = CategoryData.Category;

        Result result = category.Update(name);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(CategoryErrors.NameIsRequired);
    }
}