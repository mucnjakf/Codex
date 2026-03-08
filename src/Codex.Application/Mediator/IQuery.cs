using Codex.Domain.Outcomes;
using MediatR;

namespace Codex.Application.Mediator;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>;