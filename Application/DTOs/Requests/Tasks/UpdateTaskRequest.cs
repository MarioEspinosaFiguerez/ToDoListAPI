namespace Application.DTOs.Requests.Tasks;

public record UpdateToDoTaskRequest
{
    public string? Title { get; init; } = string.Empty;
    public string? Description { get; init; } = string.Empty;
    public Guid? UserAssignedId { get; init; }
    public int? PriorityId { get; init; }
    public int? StateId { get; init; }
    public string? Deadline { get; init; }
}
