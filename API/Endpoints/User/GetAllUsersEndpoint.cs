namespace API.Endpoints.User;

public class GetAllUsersEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async ([FromServices] IUserService _userService) =>
        {
            IEnumerable<UserDTO> users = await _userService.GetAllUsers().ToListAsync();

            return Results.Ok(users);
        })
        .WithName("GetAllUsers")
        .WithTags("Users")
        .Produces<IEnumerable<UserDTO>>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "List all users";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. Return the list of users, empty if there's no users" };

            return opt;
        });
    }
}