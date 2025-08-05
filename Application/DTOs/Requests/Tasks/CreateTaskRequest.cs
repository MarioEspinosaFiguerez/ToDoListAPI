namespace Application.DTOs.Requests.Tasks;

public record CreateToDoTaskRequest
{
    public required string Title { get; init; } = string.Empty;
    public required string Description { get; init; } = string.Empty;
    public Guid UserAssignedId { get; init; }
    public int PriorityId { get; init; }
    public required string Deadline { get; init; }
}