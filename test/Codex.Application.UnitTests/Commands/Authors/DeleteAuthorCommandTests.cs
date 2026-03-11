using Codex.Application.Commands.Authors;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Authors;

public sealed class DeleteAuthorCommandTests
{
    private readonly DeleteAuthorCommandHandler _commandHandler;

    private readonly IAuthorRepository _authorRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public DeleteAuthorCommandTests()
    {
        _authorRepositoryMock = Substitute.For<IAuthorRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new DeleteAuthorCommandHandler(_authorRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldDeleteAuthor_WhenParametersAreValid()
    {
        Author author = AuthorData.Author;

        _authorRepositoryMock
            .GetWithPostsAsync(author.Id, Arg.Any<CancellationToken>())
            .Returns(author);

        _authorRepositoryMock
            .DeleteAsync(author, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        DeleteAuthorCommand command = new(author.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _authorRepositoryMock
            .Received(1)
            .GetWithPostsAsync(author.Id, Arg.Any<CancellationToken>());

        await _authorRepositoryMock
            .Received(1)
            .DeleteAsync(author, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenAuthorDoesNotExist()
    {
        Guid authorId = AuthorData.Id;

        _authorRepositoryMock
            .GetWithPostsAsync(authorId, Arg.Any<CancellationToken>())
            .Returns((Author)null!);

        DeleteAuthorCommand command = new(authorId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.NotFound);

        await _authorRepositoryMock
            .Received(1)
            .GetWithPostsAsync(authorId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnCannotDeleteContainsPostsError_WhenAuthorContainsPosts()
    {
        Author author = AuthorData.AuthorWithPosts();

        _authorRepositoryMock
            .GetWithPostsAsync(author.Id, Arg.Any<CancellationToken>())
            .Returns(author);

        DeleteAuthorCommand command = new(author.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(AuthorErrors.CannotDeleteContainsPosts);

        await _authorRepositoryMock
            .Received(1)
            .GetWithPostsAsync(author.Id, Arg.Any<CancellationToken>());
    }
}