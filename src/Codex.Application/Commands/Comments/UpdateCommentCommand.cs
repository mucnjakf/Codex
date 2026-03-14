using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Comments;

public sealed record UpdateCommentCommand(Guid Id, string Content) : ICommand;

internal sealed class UpdateCommentCommandHandler(
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCommentCommand>
{
    public async Task<Result> Handle(UpdateCommentCommand command, CancellationToken cancellationToken)
    {
        Comment? comment = await commentRepository.GetByIdAsync(command.Id, cancellationToken);

        if (comment is null)
        {
            return Result.Failure(CommentErrors.NotFound);
        }

        Result commentUpdateResult = comment.Update(command.Content);

        if (commentUpdateResult.IsFailure)
        {
            return Result.Failure(commentUpdateResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}