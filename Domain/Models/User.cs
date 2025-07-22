namespace Domain.Models;

public class User
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required, MaxLength(50)]
    public required string Name { get; set; }

    [Required, MaxLength(50)]
    public required string FirstSurname { get; set; }

    [MaxLength(50)]
    public string? SecondSurname { get; set; }

    [Required]
    public DateTimeOffset BirthDate { get; set; }
}