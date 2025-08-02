
namespace Infrastructure.Repositories;

public class StateRepository : IStateRepository
{
    public readonly TodoListDbContext _context;

    public StateRepository(TodoListDbContext context) => _context = context;

    public async Task<string?> GetStateEnumName(int id)
    {
        string? stateEnum = await _context.States.Where(s => s.Id == id).Select(s => s.Name).SingleOrDefaultAsync();

        return stateEnum is null ? null : stateEnum;
    }
}
