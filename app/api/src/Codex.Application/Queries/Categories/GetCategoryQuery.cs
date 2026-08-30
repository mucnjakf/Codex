using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Dtos.Mappers;
using Codex.Application.Mediator;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;

namespace Codex.Application.Queries.Categories;

public sealed record GetCategoryQuery(Guid Id) : IQuery<CategoryDto>;

internal sealed class GetCategoryQueryHandler(
    ICategoryRepository categoryRepository)
    : IQueryHandler<GetCategoryQuery, CategoryDto>
{
    public async Task<Result<CategoryDto>> Handle(GetCategoryQuery query, CancellationToken cancellationToken)
    {
        Category? category = await categoryRepository.GetByIdAsync(query.Id, cancellationToken);

        if (category is null)
        {
            return Result.Failure<CategoryDto>(CategoryErrors.NotFound);
        }

        var categoryDto = category.ToCategoryDto();

        return Result.Success(categoryDto);
    }
}