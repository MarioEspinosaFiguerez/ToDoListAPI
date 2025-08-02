namespace Domain.Interfaces;

public interface IStateRepository
{
    public Task<string?> GetStateEnumName(int id);
}