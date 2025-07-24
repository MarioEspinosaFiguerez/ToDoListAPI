
namespace Application.Services;

public class TaskService : ITaskService
{
    public readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository) => _taskRepository = taskRepository;

    public async Task<IEnumerable<ToDoTaskDTO>> GetAllTasks()
    {
        var tasks = await _taskRepository.GetAllTasks();

        return tasks.Select(task => new ToDoTaskDTO
        {
            Title = task.Title,
            Description = task.Description,
            UserAssignedId = task.UserAssignedId,
            Priority = new EnumDTO(task.PriorityId, task.Priority.Name),
            State = new EnumDTO(task.StateId, task.State.Name),
            Deadline = task.Deadline,
        });
    }
}
