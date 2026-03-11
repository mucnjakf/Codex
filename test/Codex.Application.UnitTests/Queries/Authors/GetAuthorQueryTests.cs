using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Application.Queries.Authors;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Queries.Authors;

public sealed class GetAuthorQueryTests
{
    private readonly GetAuthorQueryHandler _queryHandler;

    private readonly IAuthorRepository _authorRepositoryMock;

    public GetAuthorQueryTests()
    {
        _authorRepositoryMock = Substitute.For<IAuthorRepository>();

        _queryHandler = new GetAuthorQueryHandler(_authorRepositoryMock);
    }

    [Fact]
    public async Task Handle_ShouldReturnAuthor_WhenAuthorExists()
    {
        Author author = AuthorData.Author;

        _authorRepositoryMock
            .GetAsNoTrackingAsync(author.Id, Arg.Any<CancellationToken>())
            .Returns(author);

        GetAuthorQuery query = new(author.Id);

        Result<AuthorDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldBe(author.Id);
        result.Value.CreatedAtUtc.ShouldBe(author.CreatedAtUtc);
        result.Value.UpdatedAtUtc.ShouldBe(author.UpdatedAtUtc);
        result.Value.FirstName.ShouldBe(author.FirstName);
        result.Value.LastName.ShouldBe(author.LastName);
        result.Value.Biography.ShouldBe(author.Biography);
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenAuthorDoesNotExist()
    {
        Guid authorId = AuthorData.Id;

        _authorRepositoryMock
            .GetAsNoTrackingAsync(authorId, Arg.Any<CancellationToken>())
            .Returns((Author)null!);

        GetAuthorQuery query = new(authorId);

        Result<AuthorDto> result = await _queryHandler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldNotBeNull();
        result.Error.ShouldBe(AuthorErrors.NotFound);

        await _authorRepositoryMock
            .Received(1)
            .GetAsNoTrackingAsync(authorId);
    }
}