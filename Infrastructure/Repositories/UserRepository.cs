namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public readonly TodoListDbContext _context;

    public UserRepository(TodoListDbContext context) => _context = context;

    public async Task<bool> ExistsAsync(string email) => await _context.Users.AnyAsync(user => user.Email == email);

    public async Task<bool> ExistAsyncById(Guid id) => await _context.Users.AnyAsync(user => user.Id == id);

    public IQueryable<User> GetAllUsers() => _context.Users.AsNoTracking();

    public async Task<User?> GetUserById(Guid id) => await _context.Users.FindAsync(id);

    public async Task<User> CreateUser(User request)
    {
        await _context.Users.AddAsync(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<User> UpdateUser(User request)
    {
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<bool> DeleteUser(User request)
    {
        var tasksAssignedToUser = _context.Tasks.Where(task => task.UserAssignedId == request.Id);
        _context.Tasks.RemoveRange(tasksAssignedToUser);
        _context.Users.Remove(request);
        int resultRowsAffected = await _context.SaveChangesAsync();

        return resultRowsAffected > 0;
    }

    public IQueryable<ToDoTask> GetAllTasksAssignedToUser(Guid userId) => 
        _context.Tasks.Include(t => t.Priority).Include(t => t.State).Include(t => t.AssignedTo)
        .Where(t => t.UserAssignedId == userId).AsNoTracking();

    public IQueryable<ToDoTask> GetTaskByIdAssignedToUser(Guid taskId, Guid userId) => 
        _context.Tasks.Include(t => t.Priority).Include(t => t.State).Include(t => t.AssignedTo)
        .Where(t => t.UserAssignedId == userId && t.Id == taskId).AsNoTracking();
}