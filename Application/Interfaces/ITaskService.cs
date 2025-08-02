namespace Application.Interfaces;

public interface ITaskService
{
    IQueryable<ToDoTaskResponse> GetAllTasks();
    Task<ToDoTaskResponse> GetTaskById(Guid id);
    Task<ToDoTaskResponse> CreateTask(CreateToDoTaskRequest request);
}