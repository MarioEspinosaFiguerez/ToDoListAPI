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

    public DateTimeOffset Deadline { get; set; }

    public ToDoTask() { }
    public ToDoTask(string title, string description, Guid userAssignedId, int priorityId, int stateId, DateTimeOffset deadline)
    {
        Title = title;
        Description = description;
        UserAssignedId = userAssignedId;
        PriorityId = priorityId;
        StateId = stateId;
        Deadline = deadline;
    }
}