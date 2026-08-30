using Codex.Application.Commands.Readers;
using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Readers;

public sealed class CreateReaderCommandTests
{
    private readonly CreateReaderCommandHandler _commandHandler;

    private readonly IReaderRepository _readerRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public CreateReaderCommandTests()
    {
        _readerRepositoryMock = Substitute.For<IReaderRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new CreateReaderCommandHandler(_readerRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldCreateReader_WhenParametersAreValid()
    {
        _readerRepositoryMock
            .CreateAsync(Arg.Any<Reader>(), Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        CreateReaderCommand command = new(ReaderData.FirstName, ReaderData.LastName);

        Result<ReaderDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.FirstName.ShouldBe(ReaderData.FirstName);
        result.Value.LastName.ShouldBe(ReaderData.LastName);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnFirstNameIsRequiredError_WhenFirstNameIsEmptyOrWhitespace(string firstName)
    {
        CreateReaderCommand command = new(firstName, ReaderData.LastName);

        Result<ReaderDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.FirstNameIsRequired);

        await _readerRepositoryMock
            .Received(0)
            .CreateAsync(Arg.Any<Reader>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnLastNameIsRequiredError_WhenLastNameIsEmptyOrWhitespace(string lastName)
    {
        CreateReaderCommand command = new(ReaderData.FirstName, lastName);

        Result<ReaderDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.LastNameIsRequired);

        await _readerRepositoryMock
            .Received(0)
            .CreateAsync(Arg.Any<Reader>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(0)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}