namespace Application.DTOs.Responses.Users;

public record UserDTO
{
    public Guid Id { get; init; }

    public required string FullName { get; init; }

    public required string Email { get; init; }

    public DateOnly BirthDate { get; init; }
}