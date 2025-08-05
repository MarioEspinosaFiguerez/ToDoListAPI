namespace Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    public readonly TodoListDbContext _context;

    public TaskRepository(TodoListDbContext context) => _context = context;

    public IQueryable<ToDoTask> GetAllTasks() => _context.Tasks.Include(task => task.Priority).Include(task => task.State).Include(task => task.AssignedTo).AsNoTracking();

    public async Task<bool> ExistTaskById(Guid id) => await _context.Tasks.AnyAsync(task => task.Id == id);

    public async Task<ToDoTask?> GetTaskById(Guid id) => await _context.Tasks.Include(task => task.Priority).Include(task => task.State).Include(task => task.AssignedTo).FirstOrDefaultAsync(task => task.Id == id);

    public async Task<ToDoTask> CreateTask(ToDoTask request)
    {
        await _context.Tasks.AddAsync(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<ToDoTask> UpdateTask(ToDoTask request)
    {
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<bool> DeleteTask(ToDoTask task)
    {
        _context.Tasks.Remove(task);
        int resultRowsAffected = await _context.SaveChangesAsync();

        return resultRowsAffected > 0;
    }
}