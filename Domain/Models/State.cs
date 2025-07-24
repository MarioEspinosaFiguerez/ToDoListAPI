namespace Domain.Models;

public class State
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Foreign Key
    public ICollection<ToDoTask> ToDoTasks { get; set; } = new List<ToDoTask>();
}