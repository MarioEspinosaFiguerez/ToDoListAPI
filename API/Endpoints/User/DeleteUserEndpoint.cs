
namespace API.Endpoints.User;

public class DeleteUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/users/{id}", async (Guid id, [FromServices]IUserService _userService) => Results.Ok(await _userService.DeleteUser(id)))
        .WithName("DeleteUser")
        .WithTags("Users")
        .Produces<bool>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Delete a user by their ID";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. User deleted" };

            return opt;
        }); ;
    }
}
