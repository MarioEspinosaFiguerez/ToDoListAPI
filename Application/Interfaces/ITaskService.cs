namespace Application.Interfaces;

public interface ITaskService
{
    IQueryable<ToDoTaskResponse> GetAllTasks();
    Task<bool> ExistTaskById(Guid id);
    Task<ToDoTaskResponse?> GetTaskById(Guid id);
    Task<ToDoTaskResponse> CreateTask(CreateToDoTaskRequest request);
    Task<ToDoTaskResponse> UpdateTask(Guid id, UpdateToDoTaskRequest request);
    Task<bool> DeleteTask(Guid id);
}