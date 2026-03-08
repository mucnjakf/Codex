using Codex.Domain.Outcomes;
using MediatR;

namespace Codex.Application.Mediator;

internal interface ICommand : IRequest<Result>, IBaseCommand;

internal interface ICommand<TResponse> : IRequest<Result<TResponse>>, IBaseCommand;

internal interface IBaseCommand;