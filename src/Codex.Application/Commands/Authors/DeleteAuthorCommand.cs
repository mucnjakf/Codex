using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Authors;

public sealed record DeleteAuthorCommand(Guid Id) : ICommand;

internal sealed class DeleteAuthorCommandHandler(
    IAuthorRepository authorRepository,
    IPostRepository postRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteAuthorCommand>
{
    public async Task<Result> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
    {
        Author? author = await authorRepository.GetByIdAsync(command.Id, cancellationToken);

        if (author is null)
        {
            return Result.Failure(AuthorErrors.NotFound);
        }

        bool postsExist = await postRepository.ExistsByAuthorIdAsync(author.Id, cancellationToken);

        if (postsExist)
        {
            return Result.Failure(AuthorErrors.CannotDeleteContainsPosts);
        }

        authorRepository.Delete(author);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}