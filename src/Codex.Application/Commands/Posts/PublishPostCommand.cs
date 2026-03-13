using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Posts;

public sealed record PublishPostCommand(Guid Id) : ICommand;

internal sealed class PublishPostCommandHandler(
    IPostRepository postRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PublishPostCommand>
{
    public async Task<Result> Handle(PublishPostCommand command, CancellationToken cancellationToken)
    {
        Post? post = await postRepository.GetByIdAsync(command.Id, cancellationToken);

        if (post is null)
        {
            return Result.Failure(PostErrors.NotFound);
        }

        Result publishPostResult = post.Publish();

        if (publishPostResult.IsFailure)
        {
            return Result.Failure(publishPostResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}