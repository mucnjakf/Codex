using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Authors;

public sealed record DeleteAuthorCommand(Guid Id) : ICommand;

internal sealed class DeleteAuthorCommandHandler(
    IAuthorRepository authorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteAuthorCommand>
{
    public async Task<Result> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        Author? author = await authorRepository.GetWithPostsAsync(command.Id, cancellationToken);

        if (author is null)
        {
            return Result.Failure(AuthorErrors.NotFound);
        }

        // TODO: maybe another call to database to check if contains posts
        if (author.Posts.Any())
        {
            return Result.Failure(AuthorErrors.CannotDeleteContainsPosts);
        }

        await authorRepository.DeleteAsync(author, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}