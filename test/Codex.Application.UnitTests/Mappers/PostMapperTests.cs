using Codex.Application.Dtos.Mappers;
using Codex.Domain.Entities;
using Codex.Tests.Data;
using Shouldly;

namespace Codex.Application.UnitTests.Mappers;

public sealed class PostMapperTests
{
    [Fact]
    public void ToPostDto_ShouldReturnPostDto()
    {
        Post post = PostData.PostWithAuthorAndCategory();

        var postDto = post.ToPostDto();

        postDto.ShouldNotBeNull();
        postDto.Id.ShouldBe(post.Id);
        postDto.CreatedAtUtc.ShouldBe(post.CreatedAtUtc);
        postDto.UpdatedAtUtc.ShouldBe(post.UpdatedAtUtc);
        postDto.Title.ShouldBe(post.Title);
        postDto.Content.ShouldBe(post.Content);
        postDto.Status.ShouldBe(post.Status);
        postDto.PublishedAtUtc.ShouldBe(post.PublishedAtUtc);
        postDto.Author.Id.ShouldBe(post.Author.Id);
        postDto.Category.Id.ShouldBe(post.Category.Id);
    }
}