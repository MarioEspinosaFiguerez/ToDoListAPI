namespace Application.DTOs.Requests.Users;

public record UpdateUserRequest
{
    public string? Name { get; init; }

    public string? FirstSurname { get; init; }

    public string? SecondSurname { get; init; }

    public string? Email { get; init; }

    public DateOnly? BirthDate { get; init; }
}