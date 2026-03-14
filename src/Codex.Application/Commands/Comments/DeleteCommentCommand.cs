using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Comments;

public sealed record DeleteCommentCommand(Guid Id) : ICommand;

internal sealed class DeleteCommentCommandHandler(
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteCommentCommand>
{
    public async Task<Result> Handle(DeleteCommentCommand command, CancellationToken cancellationToken)
    {
        Comment? comment = await commentRepository.GetByIdAsync(command.Id, cancellationToken);

        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound);
        }

        commentRepository.Delete(comment);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}