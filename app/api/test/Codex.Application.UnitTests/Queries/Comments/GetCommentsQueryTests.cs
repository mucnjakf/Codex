using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Comments;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Comments;

public sealed class GetCommentsQueryTests
{
    private readonly GetCommentsQueryHandler _queryHandler;

    private readonly ICommentRepository _commentRepositoryMock;

    public GetCommentsQueryTests()
    {
        _commentRepositoryMock = Substitute.For<ICommentRepository>();

        _queryHandler = new GetCommentsQueryHandler(_commentRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnComments_WhenCommentsExist()
    {
        IReadOnlyList<Comment> comments = [CommentData.CommentWithPostAndReader()];
        const int pageNumber = 1;
        const int pageSize = 1;

        var paginatedComments = new PaginationDto<Comment>(
            comments,
            pageNumber,
            pageSize,
            comments.Count);

        _commentRepositoryMock
            .GetPaginatedAsync(
                pageNumber,
                pageSize,
                Arg.Any<CancellationToken>())
            .Returns((comments, comments.Count));

        GetCommentsQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<CommentDto>> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Items.Count.ShouldBe(paginatedComments.Items.Count);
        result.Value.Items[0].Id.ShouldBe(paginatedComments.Items[0].Id);
        result.Value.PageNumber.ShouldBe(paginatedComments.PageNumber);
        result.Value.PageSize.ShouldBe(paginatedComments.PageSize);
        result.Value.TotalCount.ShouldBe(paginatedComments.TotalCount);
        result.Value.TotalPages.ShouldBe(paginatedComments.TotalPages);
        result.Value.HasPreviousPage.ShouldBe(paginatedComments.HasPreviousPage);
        result.Value.HasNextPage.ShouldBe(paginatedComments.HasNextPage);

        await _commentRepositoryMock
            .Received(1)
            .GetPaginatedAsync(pageNumber, pageSize);
    }
}