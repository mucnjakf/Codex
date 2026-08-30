using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Domain.UnitTests.Bootstrapper;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Domain.UnitTests.Entities;

public sealed class ReaderTests : BaseTest
{
    [Fact]
    public void Create_ShouldReturnReader_WhenParametersAreValid()
    {
        Result<Reader> result = Reader.Create(ReaderData.FirstName, ReaderData.LastName);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.FirstName.ShouldBe(ReaderData.FirstName);
        result.Value.LastName.ShouldBe(ReaderData.LastName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        Result<Reader> result = Reader.Create(firstName, ReaderData.LastName);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.FirstNameIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        Result<Reader> result = Reader.Create(ReaderData.FirstName, lastName);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.LastNameIsRequired);
    }

    [Fact]
    public void Update_ShouldReturnSuccess_WhenParametersAreValid()
    {
        Reader reader = ReaderData.Reader;

        const string firstName = "New first name";
        const string lastName = "New last name";

        Result result = reader.Update(firstName, lastName);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        reader.UpdatedAtUtc.ShouldNotBeNull();

        reader.FirstName.ShouldBe(firstName);
        reader.LastName.ShouldBe(lastName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        Reader reader = ReaderData.Reader;

        Result result = reader.Update(firstName, "New last name");

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.FirstNameIsRequired);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Update_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        Reader reader = ReaderData.Reader;

        Result result = reader.Update("New first name", lastName);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.LastNameIsRequired);
    }
}