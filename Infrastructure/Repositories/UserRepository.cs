namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    public readonly TodoListDbContext _context;

    public UserRepository(TodoListDbContext context) => _context = context;

    public async Task<bool> ExistsAsync(string email) => await _context.Users.AnyAsync(user => user.Email == email);

    // IQueryable doesn't execute the query, only create it. It executes when doing the ToList / First() / CountAsync / AnyAsync()
    public IQueryable<User> GetAllUsers() => _context.Users.AsNoTracking();

    public async Task<User?> GetUserById(Guid id) => await _context.Users.FindAsync(id);

    public async Task<User> CreateUser(User request)
    {
        // EF Core automatically creates a Transaction and make a Rollback if error is thrown
        await _context.Users.AddAsync(request);
        await _context.SaveChangesAsync();

        return request;
    }

    public async Task<User> UpdateUser(User request)
    {
        // Tracked auto because of the tracking in the service with the GetUserById() - FindAsync tracked the Entity to EF Core
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