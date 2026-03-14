using Codex.Application.Commands.Comments;
using Codex.Application.Data;
using Codex.Application.Dtos;
using Codex.Domain.Entities;
using Codex.Domain.Errors;
using Codex.Domain.Outcomes;
using Codex.Tests.Data;
using NSubstitute;
using Shouldly;

namespace Codex.Application.UnitTests.Commands.Comments;

public sealed class CreateCommentCommandTests
{
    private readonly CreateCommentCommandHandler _commandHandler;

    private readonly ICommentRepository _commentRepositoryMock;
    private readonly IPostRepository _postRepositoryMock;
    private readonly IReaderRepository _readerRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    public CreateCommentCommandTests()
    {
        _commentRepositoryMock = Substitute.For<ICommentRepository>();
        _postRepositoryMock = Substitute.For<IPostRepository>();
        _readerRepositoryMock = Substitute.For<IReaderRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _commandHandler = new CreateCommentCommandHandler(
            _commentRepositoryMock,
            _postRepositoryMock,
            _readerRepositoryMock,
            _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_ShouldCreateComment_WhenParametersAreValid()
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        _postRepositoryMock
            .ExistsByIdAsync(comment.Post.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _readerRepositoryMock
            .ExistsByIdAsync(comment.Reader.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _unitOfWorkMock
            .SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(1));

        _commentRepositoryMock
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(comment);

        CreateCommentCommand command = new(
            comment.Content,
            comment.Post.Id,
            comment.Reader.Id);

        Result<CommentDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeTrue();
        result.IsFailure.ShouldBeFalse();

        result.Value.ShouldNotBeNull();
        result.Value.Id.ShouldNotBe(Guid.Empty);
        result.Value.Content.ShouldBe(comment.Content);
        result.Value.Post.Id.ShouldBe(comment.Post.Id);
        result.Value.Reader.Id.ShouldBe(comment.Reader.Id);

        await _postRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.PostId, Arg.Any<CancellationToken>());

        await _readerRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.ReaderId, Arg.Any<CancellationToken>());

        await _commentRepositoryMock
            .Received(1)
            .CreateAsync(Arg.Any<Comment>(), Arg.Any<CancellationToken>());

        await _unitOfWorkMock
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());

        await _commentRepositoryMock
            .Received(1)
            .GetByIdAsync(Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenPostDoesNotExist()
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        _postRepositoryMock
            .ExistsByIdAsync(comment.Post.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        CreateCommentCommand command = new(
            comment.Content,
            comment.Post.Id,
            comment.Reader.Id);

        Result<CommentDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(PostErrors.NotFound);

        await _postRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.PostId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnNotFoundError_WhenReaderDoesNotExist()
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        _postRepositoryMock
            .ExistsByIdAsync(comment.Post.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _readerRepositoryMock
            .ExistsByIdAsync(comment.Reader.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        CreateCommentCommand command = new(
            comment.Content,
            comment.Post.Id,
            comment.Reader.Id);

        Result<CommentDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(ReaderErrors.NotFound);

        await _postRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.PostId, Arg.Any<CancellationToken>());

        await _readerRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.ReaderId, Arg.Any<CancellationToken>());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public async Task Handle_ShouldReturnContentIsRequired_WhenContentIsEmptyOrWhitespace(string content)
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        _postRepositoryMock
            .ExistsByIdAsync(comment.Post.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _readerRepositoryMock
            .ExistsByIdAsync(comment.Reader.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        CreateCommentCommand command = new(
            content,
            comment.Post.Id,
            comment.Reader.Id);

        Result<CommentDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CommentErrors.ContentIsRequired);

        await _postRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.PostId, Arg.Any<CancellationToken>());

        await _readerRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.ReaderId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnPostIdIsRequired_WhenPostIdIsEmpty()
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        _postRepositoryMock
            .ExistsByIdAsync(Guid.Empty, Arg.Any<CancellationToken>())
            .Returns(true);

        _readerRepositoryMock
            .ExistsByIdAsync(comment.Reader.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        CreateCommentCommand command = new(
            comment.Content,
            Guid.Empty,
            comment.Reader.Id);

        Result<CommentDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CommentErrors.PostIdIsRequired);

        await _postRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.PostId, Arg.Any<CancellationToken>());

        await _readerRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.ReaderId, Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_ShouldReturnReaderIdIsRequired_WhenReaderIdIsEmpty()
    {
        Comment comment = CommentData.CommentWithPostAndReader();

        _postRepositoryMock
            .ExistsByIdAsync(comment.Post.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        _readerRepositoryMock
            .ExistsByIdAsync(Guid.Empty, Arg.Any<CancellationToken>())
            .Returns(true);

        CreateCommentCommand command = new(
            comment.Content,
            comment.Post.Id,
            Guid.Empty);

        Result<CommentDto> result = await _commandHandler.Handle(command, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.IsFailure.ShouldBeTrue();

        result.Error.ShouldBe(CommentErrors.ReaderIdIsRequired);

        await _postRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.PostId, Arg.Any<CancellationToken>());

        await _readerRepositoryMock
            .Received(1)
            .ExistsByIdAsync(command.ReaderId, Arg.Any<CancellationToken>());
    }
}