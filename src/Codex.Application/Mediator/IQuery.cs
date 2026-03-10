using Codex.Domain.Outcomes;
using MediatR;

namespace Codex.Application.Mediator;

internal interface IQuery<TResponse> : IRequest<Result<TResponse>>;