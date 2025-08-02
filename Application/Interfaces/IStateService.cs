namespace Application.Interfaces;

public interface IStateService
{
    public Task<EnumDTO> GetStateEnum(int id);
}
