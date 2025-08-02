namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public readonly TodoListDbContext _context;

    public UserRepository(TodoListDbContext context) => _context = context;

    public async Task<bool> ExistsAsync(string email) => await _context.Users.AnyAsync(user => user.Email == email);

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
        _context.Users.Remove(request);
        int resultRowsAffected = await _context.SaveChangesAsync();

        return resultRowsAffected > 0;
    }      
}