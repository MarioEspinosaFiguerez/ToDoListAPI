namespace Domain.Interfaces;

public interface ITaskRepository
{
    IQueryable<ToDoTask> GetAllTasks();
    Task<bool> ExistTaskById(Guid id);
    Task<ToDoTask?> GetTaskById(Guid id);
    Task<ToDoTask> CreateTask(ToDoTask request);
    Task<ToDoTask> UpdateTask(ToDoTask request);
    Task<bool> DeleteTask(ToDoTask task);
}