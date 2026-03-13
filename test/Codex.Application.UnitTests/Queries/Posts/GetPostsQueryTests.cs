using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Pagination;
using Codex.Application.Queries.Posts;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Posts;

public sealed class GetPostsQueryTests
{
    private readonly GetPostsQueryHandler _queryHandler;

    private readonly IPostRepository _postRepositoryMock;

    public GetPostsQueryTests()
    {
        _postRepositoryMock = Substitute.For<IPostRepository>();

        _queryHandler = new GetPostsQueryHandler(_postRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnPosts_WhenPostsExist()
    {
        IReadOnlyList<Post> posts = [PostData.PostWithAuthorAndCategory()];
        const int pageNumber = 1;
        const int pageSize = 1;

        var paginatedPosts = new PaginationDto<Post>(
            posts,
            pageNumber,
            pageSize,
            posts.Count);

        _postRepositoryMock
            .GetPaginatedAsync(
                pageNumber,
                pageSize,
                Arg.Any<CancellationToken>())
            .Returns((posts, posts.Count));

        GetPostsQuery query = new(pageNumber, pageSize);

        Result<PaginationDto<PostDto>> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Items.Count.ShouldBe(paginatedPosts.Items.Count);
        result.Value.Items[0].Id.ShouldBe(paginatedPosts.Items[0].Id);
        result.Value.PageNumber.ShouldBe(paginatedPosts.PageNumber);
        result.Value.PageSize.ShouldBe(paginatedPosts.PageSize);
        result.Value.TotalCount.ShouldBe(paginatedPosts.TotalCount);
        result.Value.TotalPages.ShouldBe(paginatedPosts.TotalPages);
        result.Value.HasPreviousPage.ShouldBe(paginatedPosts.HasPreviousPage);
        result.Value.HasNextPage.ShouldBe(paginatedPosts.HasNextPage);

        await _postRepositoryMock
            .Received(1)
            .GetPaginatedAsync(pageNumber, pageSize);
    }
}