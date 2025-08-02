namespace Application.DTOs.Requests.Users;

public record CreateUserRequest
{
    [SwaggerSchema($"User name must not be null or \"\" ", Nullable = false)]
    public required string Name { get; init; }

    [SwaggerSchema($"User first must not be null or \"\"", Nullable = false)]
    public required string FirstSurname { get; init; }

    [SwaggerSchema($"User second surname can be null or \"\" ", Nullable = true)]
    public string? SecondSurname { get; init; }

    [SwaggerSchema($"User email must follow sample@sample.com format ", Nullable = false)]
    public required string Email { get; init; }

    [SwaggerSchema($"User Password must follow the following format:\n" +
        $"1. Greater than 8 characters\n" +
        $"2. At least one uppercase letter\n" +
        $"3. At least one lowercase letter",
        Nullable = false)]

    public required string Password { get; init; }

    [SwaggerSchema($"User birthdate must follow (yyyy-MM-dd) format ", Nullable = false)]
    public required string BirthDate { get; init; }
}