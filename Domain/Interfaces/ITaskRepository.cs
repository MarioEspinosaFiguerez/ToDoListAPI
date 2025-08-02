namespace Domain.Interfaces;

public interface ITaskRepository
{
    IQueryable<ToDoTask> GetAllTasks();
    Task<ToDoTask?> GetTaskById(Guid id);
    Task<ToDoTask> CreateTask(ToDoTask request);
}