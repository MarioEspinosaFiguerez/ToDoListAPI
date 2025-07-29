namespace API.Endpoints.User;

public class GetUserByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users/{id}", async (Guid id, [FromServices]IUserService _userService) =>
        {
            UserDTO userById = await _userService.GetUserById(id);
            return Results.Ok(userById);
        })
        .WithName("GetUserById")
        .WithTags("Users")
        .Produces<UserDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Get a user by their ID";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the User by Id" };
            return opt;
        });
    }
}
