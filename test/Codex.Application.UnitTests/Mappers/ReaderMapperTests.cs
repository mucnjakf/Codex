using Codex.Application.Dtos.Mappers;
using Codex.Domain.Entities;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Application.UnitTests.Mappers;

public sealed class ReaderMapperTests
{
    [Fact]
    public void ToReaderDto_ShouldReturnReaderDto()
    {
        Reader reader = ReaderData.Reader;

        var readerDto = reader.ToReaderDto();

        readerDto.ShouldNotBeNull();
        readerDto.Id.ShouldBe(reader.Id);
        readerDto.CreatedAtUtc.ShouldBe(reader.CreatedAtUtc);
        readerDto.UpdatedAtUtc.ShouldBe(reader.UpdatedAtUtc);
        readerDto.FirstName.ShouldBe(reader.FirstName);
        readerDto.LastName.ShouldBe(reader.LastName);
    }
}