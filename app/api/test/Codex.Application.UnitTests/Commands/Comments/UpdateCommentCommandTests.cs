using Codex.Application.Commands.Comments;
using Codex.Application.Data;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Comments;

public sealed class UpdateCommentCommandTests
{
    private readonly UpdateCommentCommandHandler _commandHandler;

    private readonly ICommentRepository _commentRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public UpdateCommentCommandTests()
    {
        _commentRepositoryMock = Substitute.For<ICommentRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new UpdateCommentCommandHandler(
            _commentRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldUpdateComment_WhenParametersAreValid()
    {
        Comment comment = CommentData.Comment;

        _commentRepositoryMock
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>())
            .Returns(comment);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        UpdateCommentCommand command = new(
            comment.Id,
            "New post content");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenCommentDoesNotExist()
    {
        Comment comment = CommentData.Comment;

        _commentRepositoryMock
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>())
            .Returns((Comment)null!);

        UpdateCommentCommand command = new(
            comment.Id,
            "New post content");

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CommentErrors.NotFound);

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnContentIsRequiredError_WhenContentIsEmptyOrWhitespace(string content)
    {
        Comment comment = CommentData.Comment;

        _commentRepositoryMock
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>())
            .Returns(comment);

        UpdateCommentCommand command = new(
            comment.Id,
            content);

        Result result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CommentErrors.ContentIsRequired);

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(comment.Id, Arg.Any<CancellationToken>());
    }
}