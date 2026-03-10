using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Data;
using Codex.Domain.Entities;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Categories;

public sealed record CreateCategoryCommand(string Name) : ICommand<CategoryDto>;

internal sealed class CreateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<Result<CategoryDto>> Handle(
        CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        Result<Category> result = Category.Create(command.Name);

        if (result.IsFailure)
        {
            return Result.Failure<CategoryDto>(result.Error);
        }

        await categoryRepository.CreateAsync(result.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var categoryDto = result.Value.ToCategoryDto();

        return Result.Success(categoryDto);
    }
}