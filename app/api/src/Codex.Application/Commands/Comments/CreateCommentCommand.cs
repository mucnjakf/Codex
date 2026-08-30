using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Comments;

public sealed record CreateCommentCommand(
    string Content,
    Guid PostId,
    Guid ReaderId) : ICommand<CommentDto>;

internal sealed class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IReaderRepository readerRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCommentCommand, CommentDto>
{
    public async Task<Result<CommentDto>> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
    {
        bool postExists = await postRepository.ExistsByIdAsync(command.PostId, cancellationToken);

        if (!postExists)
        {
            return Result.Failure<CommentDto>(PostErrors.NotFound);
        }

        bool readerExists = await readerRepository.ExistsByIdAsync(command.ReaderId, cancellationToken);

        if (!readerExists)
        {
            return Result.Failure<CommentDto>(ReaderErrors.NotFound);
        }

        Result<Comment> result = Comment.Create(
            command.Content,
            command.PostId,
            command.ReaderId);

        if (result.IsFailure)
        {
            return Result.Failure<CommentDto>(result.Error);
        }

        await commentRepository.CreateAsync(result.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        Comment comment = (await commentRepository.GetByIdAsync(result.Value.Id, cancellationToken))!;

        var commentDto = comment.ToCommentDto();

        return Result.Success(commentDto);
    }
}