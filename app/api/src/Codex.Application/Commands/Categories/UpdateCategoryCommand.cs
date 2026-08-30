using Codex.Application.Data;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Commands.Categories;

public sealed record UpdateCategoryCommand(Guid Id, string Name) : ICommand;

internal sealed class UpdateCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCategoryCommand>
{
    public async Task<Result> Handle(UpdateCategoryCommand command, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetByIdAsync(command.Id, cancellationToken);

        if (category is null)
        {
            return Result.Failure(CategoryErrors.NotFound);
        }

        Result categoryUpdateResult = category.Update(command.Name);

        if (categoryUpdateResult.IsFailure)
        {
            return Result.Failure(categoryUpdateResult.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}