namespace Domain.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Name { get; set; }

    public required string FirstSurname { get; set; }

    public string? SecondSurname { get; set; }

    public DateTimeOffset BirthDate { get; set; }
}