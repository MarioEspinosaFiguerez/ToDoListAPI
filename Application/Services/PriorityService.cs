namespace Application.Services;

public class PriorityService : IPriorityService
{
    public readonly IPriorityRepository _priorityRepository;

    public PriorityService(IPriorityRepository priorityRepository) => _priorityRepository = priorityRepository;

    public async Task<EnumDTO> GetPriorityEnum(int id)
    {
        string? priorityEnumName = await _priorityRepository.GetPriorityEnumName(id);

       return priorityEnumName is null ? 
            throw new NotFoundException($"The Priority with ID {id} not found", id) :
            new EnumDTO(id, priorityEnumName);
    }
}