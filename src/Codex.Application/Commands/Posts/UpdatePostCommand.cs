using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Posts;

public sealed record UpdatePostCommand(
    Guid Id,
    string Title,
    string Content,
    Guid CategoryId) : ICommand;

internal sealed class UpdatePostCommandHandler(
    IPostRepository postRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdatePostCommand>
{
    public async Task<Result> Handle(UpdatePostCommand command, CancellationToken cancellationToken)
    {
        Post? post = await postRepository.GetByIdAsync(command.Id, cancellationToken);

        if (post is null)
        {
            return Result.Failure(PostErrors.NotFound);
        }

        bool categoryExists = await categoryRepository.ExistsByIdAsync(command.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Result.Failure(CategoryErrors.NotFound);
        }

        Result postUpdateResult = post.Update(command.Title, command.Content, command.CategoryId);

        if (postUpdateResult.IsFailure)
        {
            return Result.Failure(postUpdateResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}