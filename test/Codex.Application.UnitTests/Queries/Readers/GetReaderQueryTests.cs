using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Queries.Readers;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Readers;

public sealed class GetReaderQueryTests
{
    private readonly GetReaderQueryHandler _queryHandler;

    private readonly IReaderRepository _readerRepositoryMock;

    public GetReaderQueryTests()
    {
        _readerRepositoryMock = Substitute.For<IReaderRepository>();

        _queryHandler = new GetReaderQueryHandler(_readerRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnReader_WhenReaderExists()
    {
        Reader reader = ReaderData.Reader;

        _readerRepositoryMock
            .GetByIdAsync(reader.Id, Arg.Any<CancellationToken>())
            .Returns(reader);

        GetReaderQuery query = new(reader.Id);

        Result<ReaderDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(reader.Id);
        result.Value.CreatedAtUtc.ShouldBe(reader.CreatedAtUtc);
        result.Value.UpdatedAtUtc.ShouldBe(reader.UpdatedAtUtc);
        result.Value.FirstName.ShouldBe(reader.FirstName);
        result.Value.LastName.ShouldBe(reader.LastName);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenReaderDoesNotExist()
    {
        Guid readerId = ReaderData.Id;

        _readerRepositoryMock
            .GetByIdAsync(readerId, Arg.Any<CancellationToken>())
            .Returns((Reader)null!);

        GetReaderQuery query = new(readerId);

        Result<ReaderDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.NotFound);

        await _readerRepositoryMock
            .Received(1)
            .GetByIdAsync(readerId);
    }
}