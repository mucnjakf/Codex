using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Authors;

public sealed record UpdateAuthorCommand(Guid Id, string FirstName, string LastName, string Biography) : ICommand;

internal sealed class UpdateAuthorCommandHandler(
    IAuthorRepository authorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateAuthorCommand>
{
    public async Task<Result> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
    {
        Author? author = await authorRepository.GetAsync(command.Id, cancellationToken);

        if (author is null)
        {
            return Result.Failure(AuthorErrors.NotFound);
        }

        Result authorUpdateResult = author.Update(command.FirstName, command.LastName, command.Biography);

        if (authorUpdateResult.IsFailure)
        {
            return Result.Failure(authorUpdateResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}