using Codex.Application.Commands.Authors;
using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Authors;

public sealed class CreateAuthorCommandTests
{
    private readonly CreateAuthorCommandHandler _commandHandler;

    private readonly IAuthorRepository _authorRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public CreateAuthorCommandTests()
    {
        _authorRepositoryMock = Substitute.For<IAuthorRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new CreateAuthorCommandHandler(_authorRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldCreateAuthor_WhenParametersAreValid()
    {
        _authorRepositoryMock
            .CreateAsync(Arg.Any<Author>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        CreateAuthorCommand command = new(AuthorData.FirstName, AuthorData.LastName, AuthorData.Biography);

        Result<AuthorDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.FirstName.ShouldBe(AuthorData.FirstName);
        result.Value.LastName.ShouldBe(AuthorData.LastName);
        result.Value.Biography.ShouldBe(AuthorData.Biography);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        CreateAuthorCommand command = new(firstName, AuthorData.LastName, AuthorData.Biography);

        Result<AuthorDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.FirstNameIsRequired);

        await _authorRepositoryMock
            .Received(0)
            .CreateAsync(Arg.Any<Author>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        CreateAuthorCommand command = new(AuthorData.FirstName, lastName, AuthorData.Biography);

        Result<AuthorDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.LastNameIsRequired);

        await _authorRepositoryMock
            .Received(0)
            .CreateAsync(Arg.Any<Author>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnBiographyIsRequiredError_WhenBiographyIsEmptyOrWhitespace(string biography)
    {
        CreateAuthorCommand command = new(AuthorData.FirstName, AuthorData.LastName, biography);

        Result<AuthorDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.BiographyIsRequired);

        await _authorRepositoryMock
            .Received(0)
            .CreateAsync(Arg.Any<Author>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}