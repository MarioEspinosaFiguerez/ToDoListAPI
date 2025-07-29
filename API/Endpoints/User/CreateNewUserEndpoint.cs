namespace API.Endpoints.User;

public class CreateNewUserEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/users", async (IUserService _userService, IValidator<CreateUserRequest> validator, IPasswordHasher passwordHasher, CreateUserRequest request) =>
        {
            ValidationResult result = await validator.ValidateAsync(request);

            if (!result.IsValid) { return Validation.GetValidationProblemErrors(result); }

            UserDTO createdUser = await _userService.CreateUser(request, passwordHasher);
            return Results.Created($"/users/{createdUser.Id}", createdUser);
        })
        .WithName("CreateUser")
        .WithTags("Users")
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .Produces<UserDTO>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status409Conflict)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithOpenApi(opt =>
        {
            opt.Summary = "Create new User";
            opt.Responses["201"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Successfull operation, user created correctly" };
            opt.Responses["409"] = new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Error in Operation. Email already exist" };

            return opt;
        });
    }
}