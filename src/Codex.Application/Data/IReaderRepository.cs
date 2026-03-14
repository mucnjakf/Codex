using Codex.Domain.Entities;

namespace Codex.Application.Data;

public interface IReaderRepository
{
    Task<Reader?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}