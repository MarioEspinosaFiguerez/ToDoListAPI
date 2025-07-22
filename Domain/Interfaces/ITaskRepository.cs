namespace Domain.Interfaces;

public interface ITaskRepository
{
    Task<IEnumerable<ToDoTask>> GetAllTasks();
}