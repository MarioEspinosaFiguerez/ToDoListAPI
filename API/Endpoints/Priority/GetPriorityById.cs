

namespace API.Endpoints.Priority;

public class GetPriorityById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/priority", async (int id, [FromServices] IPriorityService _priorityService) =>
        {
            EnumDTO priorityEnum = await _priorityService.GetPriorityEnum(id);

            return Results.Ok(priorityEnum);
        })
        .WithName("GetPriorityById")
        .WithTags("Priority")
        .Produces<EnumDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Get a priority by its ID";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the Priority by Id" };
            return opt;
        });
    }
}
