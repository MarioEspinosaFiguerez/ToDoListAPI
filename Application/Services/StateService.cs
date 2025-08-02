
namespace Application.Services;

public class StateService : IStateService
{
    public readonly IStateRepository _stateRepository;

    public StateService(IStateRepository stateRepository) => _stateRepository = stateRepository;

    public async Task<EnumDTO> GetStateEnum(int id)
    {
       string? stateEnumName = await _stateRepository.GetStateEnumName(id);

        return stateEnumName is null ? 
            throw new NotFoundException($"The State with ID {id} not found", id) :
            new EnumDTO(id, stateEnumName);
    }
}
