namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    public readonly TodoListDbContext _context;

    public TaskRepository(TodoListDbContext context) => _context = context;

    public async Task<IEnumerable<ToDoTask>> GetAllTasks() => await _context.Tasks
                                                                .Include(task => task.Priority)
                                                                .Include(task => task.State).AsNoTracking().ToListAsync();
}