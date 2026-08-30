using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Posts;

public sealed record CreatePostCommand(
    string Title,
    string Content,
    Guid AuthorId,
    Guid CategoryId)
    : ICommand<PostDto>;

internal sealed class CreatePostCommandHandler(
    IPostRepository postRepository,
    IAuthorRepository authorRepository,
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreatePostCommand, PostDto>
{
    public async Task<Result<PostDto>> Handle(CreatePostCommand command, CancellationToken cancellationToken)
    {
        bool authorExists = await authorRepository.ExistsByIdAsync(command.AuthorId, cancellationToken);

        if (!authorExists)
        {
            return Result.Failure<PostDto>(AuthorErrors.NotFound);
        }

        bool categoryExists = await categoryRepository.ExistsByIdAsync(command.CategoryId, cancellationToken);

        if (!categoryExists)
        {
            return Result.Failure<PostDto>(CategoryErrors.NotFound);
        }

        Result<Post> result = Post.Create(
            command.Title,
            command.Content,
            command.AuthorId,
            command.CategoryId);

        if (result.IsFailure)
        {
            return Result.Failure<PostDto>(result.Error);
        }

        await postRepository.CreateAsync(result.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        Post post = (await postRepository.GetByIdAsync(result.Value.Id, cancellationToken))!;

        var postDto = post.ToPostDto();

        return Result.Success(postDto);
    }
}