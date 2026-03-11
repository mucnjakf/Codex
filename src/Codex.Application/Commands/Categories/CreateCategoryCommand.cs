using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
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
        Result<Category> categoryCreateResult = Category.Create(command.Name);

        if (categoryCreateResult.IsFailure)
        {
            return Result.Failure<CategoryDto>(categoryCreateResult.Error);
        }

        await categoryRepository.CreateAsync(categoryCreateResult.Value, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        var categoryDto = categoryCreateResult.Value.ToCategoryDto();

        return Result.Success(categoryDto);
    }
}