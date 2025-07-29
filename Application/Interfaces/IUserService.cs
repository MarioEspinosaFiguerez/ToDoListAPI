namespace Application.Interfaces;

public interface IUserService
{
    public Task<bool> ExistsAsync(string email);
    public IQueryable<UserDTO> GetAllUsers();
    public Task<UserDTO> GetUserById(Guid id);
    public Task<UserDTO> CreateUser(CreateUserRequest request, IPasswordHasher passwordHasher);
    public Task<UserDTO> UpdateUser(Guid id, UpdateUserRequest request);
    public Task<bool> DeleteUser(Guid id);
}