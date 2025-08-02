namespace Infrastructure.Repositories;

public class PriorityRepository : IPriorityRepository
{
    public readonly TodoListDbContext _context;

    public PriorityRepository(TodoListDbContext context) => _context = context;

    public async Task<string?> GetPriorityEnumName(int id)
    {
        string? priorityEnum = await _context.Priorities.Where(p => p.Id == id).Select(p => p.Name).SingleOrDefaultAsync();

        return priorityEnum is null ? null : priorityEnum;
    }
}