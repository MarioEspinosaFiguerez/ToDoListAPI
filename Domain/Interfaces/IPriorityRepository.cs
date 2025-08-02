namespace Domain.Interfaces;

public interface IPriorityRepository
{
    public Task<string?> GetPriorityEnumName(int id);
}