using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IAuthorRepository
{
    Task<Author?> GetAsNoTrackingAsync(Guid id, CancellationToken cancellationToken = default);
}