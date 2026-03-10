using Codex.Domain.Outcomes;
using MediatR;

namespace Codex.Application.Mediator;

internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;