namespace TaskManager.API.Endpoints;

/// <summary>
/// Maker interface for all endpoints
/// </summary>
public interface IEndpoint
{ 
    void MapEndpoint(IEndpointRouteBuilder app);
}