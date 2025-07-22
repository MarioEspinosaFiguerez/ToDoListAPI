namespace Domain.Models;

public class ToDoTask
{
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required(ErrorMessage = "The title is mandatory"), MaxLength(50)]
    public required string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "The description is mandatory"), MaxLength(200)]
    public required string Description { get; set; } = string.Empty;

    [Required]
    public required Guid UserAssignedId { get; set; }

    public User AssignedTo { get; set; }

    public Priority Priority { get; set; }

    public State State { get; set; }

    public DateTimeOffset Deadline { get; set; }

    public ToDoTask() { }
    public ToDoTask(string title, string description, Guid userAssignedId, Priority priority, State state, DateTimeOffset deadline)
    {
        Title = title;
        Description = description;
        UserAssignedId = userAssignedId;
        Priority = priority;
        State = State.Pending;
        Deadline = deadline;
    }
}