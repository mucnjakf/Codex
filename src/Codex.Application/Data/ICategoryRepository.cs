using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface ICategoryRepository
{
    Task<Category?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}