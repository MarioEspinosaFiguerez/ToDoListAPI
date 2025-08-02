
namespace API.Endpoints.State;

public class GetStateById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/state", async (int id, [FromServices] IStateService _stateService) =>
        {
            EnumDTO stateEnum = await _stateService.GetStateEnum(id);

            return Results.Ok(stateEnum);
        })
        .WithName("GetStateById")
        .WithTags("State")
        .Produces<EnumDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Get a priority by its ID";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the Priority by Id" };
            return opt;
        }); ;
    }
}
