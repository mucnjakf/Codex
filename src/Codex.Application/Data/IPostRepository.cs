using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IPostRepository
{
    Task<Post?> GetAsync(Guid id, CancellationToken cancellationToken = default);
}