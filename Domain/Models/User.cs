namespace Domain.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Name { get; set; }

    public required string FirstSurname { get; set; }

    public string? SecondSurname { get; set; }

    public required string Email { get; set; }

    public required string PasswordHash { get; set; }

    public DateOnly BirthDate { get; set; }

    // Security
    public int JwtTokenVersion { get; set; } = 1;

    public string? RefreshTokenHashed { get; set; }

    public DateTimeOffset? RefreshTokenExpiresAt { get; set; }

    public DateTimeOffset? PasswordChangedAt { get; set; }

    // Foreign Key
    public ICollection<ToDoTask> TasksAssigned {  get; set; } = new List<ToDoTask>();

    // Audit
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
}