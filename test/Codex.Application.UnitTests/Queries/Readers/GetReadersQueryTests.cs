using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Readers;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Readers;

public sealed class GetReadersQueryTests
{
    private readonly GetReadersQueryHandler _queryHandler;

    private readonly IReaderRepository _readerRepositoryMock;

    public GetReadersQueryTests()
    {
        _readerRepositoryMock = Substitute.For<IReaderRepository>();

        _queryHandler = new GetReadersQueryHandler(_readerRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnReaders_WhenReadersExist()
    {
        IReadOnlyList<Reader> readers = [ReaderData.Reader];
        const int pageNumber = 1;
        const int pageSize = 1;

        var paginatedReaders = new PaginationDto<Reader>(
            readers,
            pageNumber,
            pageSize,
            readers.Count);

        _readerRepositoryMock
            .GetPaginatedAsync(
                pageNumber,
                pageSize,
                Arg.Any<CancellationToken>())
            .Returns((readers, readers.Count));

        GetReadersQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<ReaderDto>> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.Items.Count.ShouldBe(paginatedReaders.Items.Count);
        result.Value.Items[0].Id.ShouldBe(paginatedReaders.Items[0].Id);
        result.Value.PageNumber.ShouldBe(paginatedReaders.PageNumber);
        result.Value.PageSize.ShouldBe(paginatedReaders.PageSize);
        result.Value.TotalCount.ShouldBe(paginatedReaders.TotalCount);
        result.Value.TotalPages.ShouldBe(paginatedReaders.TotalPages);
        result.Value.HasPreviousPage.ShouldBe(paginatedReaders.HasPreviousPage);
        result.Value.HasNextPage.ShouldBe(paginatedReaders.HasNextPage);

        await _readerRepositoryMock
            .Received(1)
            .GetPaginatedAsync(pageNumber, pageSize);
    }
}