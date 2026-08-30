using Codex.Application.Commands.Readers;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Readers;

public sealed class UpdateReaderCommandTests
{
    private readonly UpdateReaderCommandHandler _commandHandler;

    private readonly IReaderRepository _readerRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public UpdateReaderCommandTests()
    {
        _readerRepositoryMock = Substitute.For<IReaderRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new UpdateReaderCommandHandler(_readerRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldUpdateReader_WhenParametersAreValid()
    {
        Reader reader = ReaderData.Reader;

        _readerRepositoryMock
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(reader);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        UpdateReaderCommand command = new(
            reader.Id,
            "New reader first name",
            "New reader last name");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenReaderNotFound()
    {
        Guid readerId = ReaderData.Id;

        _readerRepositoryMock
            .GetByIdAsync(readerId, Arg.Any<CancellationToken>())
            .Returns((Reader)null!);

        UpdateReaderCommand command = new(
            readerId,
            "New reader first name",
            "New reader last name");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(readerId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        Reader reader = ReaderData.Reader;

        _readerRepositoryMock
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(reader);

        UpdateReaderCommand command = new(reader.Id, firstName, reader.LastName);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.FirstNameIsRequired);

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        Reader reader = ReaderData.Reader;

        _readerRepositoryMock
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(reader);

        UpdateReaderCommand command = new(reader.Id, reader.FirstName, lastName);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.LastNameIsRequired);

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>());
    }
}