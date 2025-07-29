namespace API.Endpoints.User;

public class UpdateUserEndpoint : IEndpoint
{
    // (Mandatory) MapPatch don't know the request is from Body then we have to assign or we can assign [FromServices] to the other parameters 

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/users/{id}", async (Guid id, UpdateUserRequest request, [FromServices]IUserService _userService, [FromServices]IValidator <UpdateUserRequest> validator) =>
        {
            ValidationResult result = await validator.ValidateAsync(request);

            if (!result.IsValid) { return Validation.GetValidationProblemErrors(result); }

            UserDTO updatedUser = await _userService.UpdateUser(id, request);
            return Results.Ok(updatedUser);
        })
        .WithName("UpdateUser")
        .WithTags("Users")
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces<UserDTO>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Update a user";
            opt.Responses["200"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation. User updated" };
            opt.Responses["404"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Error in operation. User with Id not found" };
            opt.Responses["409"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Error in Operation. Email already exist" };
            return opt;
        });
    }
}