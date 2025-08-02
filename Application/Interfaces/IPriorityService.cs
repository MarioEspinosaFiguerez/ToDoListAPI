namespace Application.Interfaces;

public interface IPriorityService
{
    public Task<EnumDTO> GetPriorityEnum(int id);
}
