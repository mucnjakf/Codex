using Codex.Application.Dtos.Pagination;
using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface ICategoryRepository
{
    Task<PaginationDto<Category>> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}