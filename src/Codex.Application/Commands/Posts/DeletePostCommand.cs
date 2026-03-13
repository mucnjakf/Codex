using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Posts;

public sealed record DeletePostCommand(Guid Id) : ICommand;

internal sealed class DeletePostCommandHandler(
    IPostRepository postRepository,
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeletePostCommand>
{
    public async Task<Result> Handle(DeletePostCommand command, CancellationToken cancellationToken)
    {
        Post? post = await postRepository.GetByIdAsync(command.Id, cancellationToken);

        if (post is null)
        {
            return Result.Failure(PostErrors.NotFound);
        }

        bool commentsExist = await commentRepository.ExistsByPostIdAsync(post.Id, cancellationToken);

        if (commentsExist)
        {
            return Result.Failure(PostErrors.CannotDeleteContainsComments);
        }

        await postRepository.DeleteAsync(post, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}