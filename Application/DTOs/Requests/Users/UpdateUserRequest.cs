namespace Application.DTOs.Requests.Users;

public record UpdateUserRequest
{
    [SwaggerSchema($"User name can be null or \"\" ", Nullable = true)]
    public string? Name { get; init; }

    [SwaggerSchema($"User first surname can be null or \"\" ", Nullable = true)]
    public string? FirstSurname { get; init; }

    [SwaggerSchema($"User second surname can be null or \"\" ", Nullable = true)]
    public string? SecondSurname { get; init; }

    [SwaggerSchema($"User email can be null or \"\" ", Nullable = true)]
    public string? Email { get; init; }

    [SwaggerSchema($"User birthdate can be null or with (yyyy-MM-dd) format ", Nullable = true)]
    public string? BirthDate { get; init; }
}