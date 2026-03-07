using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Domain.UnitTests.Bootstrapper;
using Codex.Domain.UnitTests.Bootstrapper.Data;
using Shouldly;

namespace Codex.Domain.UnitTests.Entities;

public sealed class AuthorTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnAuthor_WhenParametersAreValid()
    {
        Result<Author> result = Author.Create(AuthorData.FirstName, AuthorData.LastName, AuthorData.Biography);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.FirstName.ShouldBe(AuthorData.FirstName);
        result.Value.LastName.ShouldBe(AuthorData.LastName);
        result.Value.Biography.ShouldBe(AuthorData.Biography);

        result.Error.ShouldBe(Error.None);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        Result<Author> result = Author.Create(firstName, AuthorData.LastName, AuthorData.Biography);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(AuthorErrors.FirstNameIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        Result<Author> result = Author.Create(AuthorData.FirstName, lastName, AuthorData.Biography);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(AuthorErrors.LastNameIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnBiographyIsRequiredError_WhenBiographyIsEmptyOrWhitespace(string biography)
    {
        Result<Author> result = Author.Create(AuthorData.FirstName, AuthorData.LastName, biography);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(AuthorErrors.BiographyIsRequired);
    }

    [Fact]
    public void Update_ShouldReturnSuccess_WhenParametersAreValid()
    {
        Author author = Author.Create(AuthorData.FirstName, AuthorData.LastName, AuthorData.Biography).Value;

        Result result = author.Update("New first name", "new last name", "New biography");

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        Author author = Author.Create(AuthorData.FirstName, AuthorData.LastName, AuthorData.Biography).Value;

        Result result = author.Update(firstName, "New last name", "New biography");

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(AuthorErrors.FirstNameIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        Author author = Author.Create(AuthorData.FirstName, AuthorData.LastName, AuthorData.Biography).Value;

        Result result = author.Update("New first name", lastName, "New biography");

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(AuthorErrors.LastNameIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnBiographyIsRequiredError_WhenBiographyIsEmptyOrWhitespace(string biography)
    {
        Author author = Author.Create(AuthorData.FirstName, AuthorData.LastName, AuthorData.Biography).Value;

        Result result = author.Update("New first name", "New last name", biography);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(AuthorErrors.BiographyIsRequired);
    }
}