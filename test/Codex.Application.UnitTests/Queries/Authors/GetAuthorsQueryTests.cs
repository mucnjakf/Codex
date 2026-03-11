using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Authors;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Authors;

public sealed class GetAuthorsQueryTests
{
    private readonly GetAuthorsQueryHandler _queryHandler;

    private readonly IAuthorRepository _authorRepositoryMock;

    public GetAuthorsQueryTests()
    {
        _authorRepositoryMock = Substitute.For<IAuthorRepository>();

        _queryHandler = new GetAuthorsQueryHandler(_authorRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthors_WhenAuthorsExist()
    {
        IReadOnlyList<Author> authors = [AuthorData.Author];
        const int pageNumber = 1;
        const int pageSize = 1;

        var paginatedAuthors = new PaginationDto<Author>(
            authors,
            pageNumber,
            pageSize,
            authors.Count);

        _authorRepositoryMock
            .GetPaginatedAsNoTrackingAsync(
                pageNumber,
                pageSize,
                Arg.Any<CancellationToken>())
            .Returns((authors, authors.Count));

        GetAuthorsQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<AuthorDto>> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.Items.Count.ShouldBe(paginatedAuthors.Items.Count);
        result.Value.Items[0].Id.ShouldBe(paginatedAuthors.Items[0].Id);
        result.Value.PageNumber.ShouldBe(paginatedAuthors.PageNumber);
        result.Value.PageSize.ShouldBe(paginatedAuthors.PageSize);
        result.Value.TotalCount.ShouldBe(paginatedAuthors.TotalCount);
        result.Value.TotalPages.ShouldBe(paginatedAuthors.TotalPages);
        result.Value.HasPreviousPage.ShouldBe(paginatedAuthors.HasPreviousPage);
        result.Value.HasNextPage.ShouldBe(paginatedAuthors.HasNextPage);

        await _authorRepositoryMock
            .Received(1)
            .GetPaginatedAsNoTrackingAsync(pageNumber, pageSize);
    }
}