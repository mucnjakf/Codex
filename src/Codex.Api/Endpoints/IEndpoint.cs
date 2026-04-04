namespace Codex.Api.Endpoints;

// TODO: move
internal interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}