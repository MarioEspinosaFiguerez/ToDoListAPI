namespace Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<ToDoTaskDTO>> GetAllTasks();
}