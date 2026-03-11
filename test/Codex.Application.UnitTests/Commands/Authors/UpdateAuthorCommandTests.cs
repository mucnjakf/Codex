using Codex.Application.Commands.Authors;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Authors;

public sealed class UpdateAuthorCommandTests
{
    private readonly UpdateAuthorCommandHandler _commandHandler;

    private readonly IAuthorRepository _authorRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public UpdateAuthorCommandTests()
    {
        _authorRepositoryMock = Substitute.For<IAuthorRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new UpdateAuthorCommandHandler(_authorRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldUpdateAuthor_WhenParametersAreValid()
    {
        Author author = AuthorData.Author;

        _authorRepositoryMock
            .GetAsync(author.Id, Arg.Any<CancellationToken>())
            .Returns(author);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        UpdateAuthorCommand command = new(
            author.Id,
            "New author first name",
            "New author last name",
            "New author biography");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _authorRepositoryMock
            .Received(1)
            .GetAsync(author.Id, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenAuthorNotFound()
    {
        Guid authorId = AuthorData.Id;

        _authorRepositoryMock
            .GetAsync(authorId, Arg.Any<CancellationToken>())
            .Returns((Author)null!);

        UpdateAuthorCommand command = new(
            authorId,
            "New author first name",
            "New author last name",
            "New author biography");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        await _authorRepositoryMock
            .Received(1)
            .GetAsync(authorId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        Author author = AuthorData.Author;

        _authorRepositoryMock
            .GetAsync(author.Id, Arg.Any<CancellationToken>())
            .Returns(author);

        UpdateAuthorCommand command = new(author.Id, firstName, author.LastName, author.Biography);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.FirstNameIsRequired);

        await _authorRepositoryMock
            .Received(1)
            .GetAsync(author.Id, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        Author author = AuthorData.Author;

        _authorRepositoryMock
            .GetAsync(author.Id, Arg.Any<CancellationToken>())
            .Returns(author);

        UpdateAuthorCommand command = new(author.Id, author.FirstName, lastName, author.Biography);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.LastNameIsRequired);

        await _authorRepositoryMock
            .Received(1)
            .GetAsync(author.Id, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnBiographyIsRequiredError_WhenBiographyIsEmptyOrWhitespace(string biography)
    {
        Author author = AuthorData.Author;

        _authorRepositoryMock
            .GetAsync(author.Id, Arg.Any<CancellationToken>())
            .Returns(author);

        UpdateAuthorCommand command = new(author.Id, author.FirstName, author.LastName, biography);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.BiographyIsRequired);

        await _authorRepositoryMock
            .Received(1)
            .GetAsync(author.Id, Arg.Any<CancellationToken>());
    }
}