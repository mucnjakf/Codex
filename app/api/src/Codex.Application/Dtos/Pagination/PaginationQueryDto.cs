namespace Codex.Application.Dtos.Pagination;

public abstract record PaginationQueryDto(int PageNumber = 1, int PageSize = 10);