namespace API.Contracts;

public interface IEndpoint
{
    void MapEndpoint(IEndpointRouteBuilder app);
}