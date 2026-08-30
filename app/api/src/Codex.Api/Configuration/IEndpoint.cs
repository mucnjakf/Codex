namespace Codex.Api.Configuration;

internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}