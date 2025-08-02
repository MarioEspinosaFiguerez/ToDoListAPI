namespace Application.DTOs.Responses.Tasks;

public record ToDoTaskResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid UserAssignedId { get; init; }
    public EnumDTO State { get; init; }
    public EnumDTO Priority { get; init; }
    public DateTimeOffset Deadline { get; init; }
}