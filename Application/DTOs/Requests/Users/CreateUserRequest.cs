namespace Application.DTOs.Requests.Users;

public record CreateUserRequest
{
    public required string Name { get; init; }

    public required string FirstSurname { get; init; }

    public string? SecondSurname { get; init; }

    public required string Email { get; init; }

    public required string Password { get; init; }

    public DateOnly BirthDate { get; init; }
}