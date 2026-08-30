using Codex.Application.Dtos.Mappers;
using Codex.Domain.Entities;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Application.UnitTests.Mappers;

public sealed class AuthorMapperTests
{
    [Fact]
    public void ToAuthorDto_ShouldReturnAuthorDto()
    {
        Author author = AuthorData.Author;

        var authorDto = author.ToAuthorDto();

        authorDto.ShouldNotBeNull();
        authorDto.Id.ShouldBe(author.Id);
        authorDto.CreatedAtUtc.ShouldBe(author.CreatedAtUtc);
        authorDto.UpdatedAtUtc.ShouldBe(author.UpdatedAtUtc);
        authorDto.FirstName.ShouldBe(author.FirstName);
        authorDto.LastName.ShouldBe(author.LastName);
        authorDto.Biography.ShouldBe(author.Biography);
    }
}