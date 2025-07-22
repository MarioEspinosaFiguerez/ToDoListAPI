namespace Application.DTOs;

public record ToDoTaskDTO
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public Guid UserAssignedId { get; init; }
    public Priority Priority { get; init; }
    public State State { get; init; } = State.Pending;
    public DateTimeOffset Deadline { get; init; }
}