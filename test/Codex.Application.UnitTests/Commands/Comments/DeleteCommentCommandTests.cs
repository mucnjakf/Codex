using Codex.Application.Commands.Comments;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Comments;

public sealed class DeleteCommentCommandTests
{
    private readonly DeleteCommentCommandHandler _commandHandler;

    private readonly ICommentRepository _commentRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public DeleteCommentCommandTests()
    {
        _commentRepositoryMock = Substitute.For<ICommentRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new DeleteCommentCommandHandler(
            _commentRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldDeleteComment_WhenParametersAreValid()
    {
        Comment comment = CommentData.Comment;

        _commentRepositoryMock
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>())
            .Returns(comment);

        _commentRepositoryMock
            .DeleteAsync(comment, Arg.Any<CancellationToken>())
            .Returns(Task.CompletedTask);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        DeleteCommentCommand command = new(comment.Id);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>());

        await _commentRepositoryMock
            .Received(1)
            .DeleteAsync(comment, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCommentDoesNotExist()
    {
        Guid commentId = CommentData.Id;

        _commentRepositoryMock
            .GetByIdAsync(commentId, Arg.Any<CancellationToken>())
            .Returns((Comment)null!);

        DeleteCommentCommand command = new(commentId);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CommentErrors.NotFound);

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(commentId, Arg.Any<CancellationToken>());
    }
}