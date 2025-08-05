namespace Domain.Models;

public class ToDoTask
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public required string Title { get; set; } = string.Empty;

    public required string Description { get; set; } = string.Empty;

    public required Guid UserAssignedId { get; set; }

    public User AssignedTo { get; set; }

    public required int PriorityId { get; set; }
    public Priority Priority { get; set; }

    public required int StateId { get; set; }
    public State State { get; set; }

    public required DateTimeOffset Deadline { get; set; }


    // Audit
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.Now;
}