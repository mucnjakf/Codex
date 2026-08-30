using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Authors;

public sealed record CreateAuthorCommand(string FirstName, string LastName, string Biography) : ICommand<AuthorDto>;

internal sealed class CreateAuthorCommandHandler(
    IAuthorRepository authorRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateAuthorCommand, AuthorDto>
{
    public async Task<Result<AuthorDto>> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
    {
        Result<Author> authorCreateResult = Author.Create(
            command.FirstName,
            command.LastName,
            command.Biography);

        if (authorCreateResult.IsFailure)
        {
            return Result.Failure<AuthorDto>(authorCreateResult.Error);
        }

        await authorRepository.CreateAsync(authorCreateResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var authorDto = authorCreateResult.Value.ToAuthorDto();

        return Result.Success(authorDto);
    }
}