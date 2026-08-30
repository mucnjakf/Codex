using Codex.Application.Commands.Readers;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Readers;

public sealed class DeleteReaderCommandTests
{
    private readonly DeleteReaderCommandHandler _commandHandler;

    private readonly IReaderRepository _readerRepositoryMock;
    private readonly ICommentRepository _commentRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public DeleteReaderCommandTests()
    {
        _readerRepositoryMock = Substitute.For<IReaderRepository>();
        _commentRepositoryMock = Substitute.For<ICommentRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler =
            new DeleteReaderCommandHandler(_readerRepositoryMock, _commentRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldDeleteAuthor_WhenParametersAreValid()
    {
        Reader reader = ReaderData.Reader;

        _readerRepositoryMock
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(reader);

        _commentRepositoryMock
            .ExistsByReaderIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        DeleteReaderCommand command = new(reader.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>());

        await _commentRepositoryMock
            .Received(1)
            .ExistsByReaderIdAsync(reader.Id, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenReaderDoesNotExist()
    {
        Guid readerId = ReaderData.Id;

        _readerRepositoryMock
            .GetByIdAsync(readerId, Arg.Any<CancellationToken>())
            .Returns((Reader)null!);

        DeleteReaderCommand command = new(readerId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.NotFound);

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(readerId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnCannotDeleteContainsCommentsError_WhenReaderContainsComments()
    {
        Reader reader = ReaderData.Reader;

        _readerRepositoryMock
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(reader);

        _commentRepositoryMock
            .ExistsByReaderIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        DeleteReaderCommand command = new(reader.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.CannotDeleteContainsComments);

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>());

        await _commentRepositoryMock
            .Received(1)
            .ExistsByReaderIdAsync(reader.Id, Arg.Any<CancellationToken>());
    }
}