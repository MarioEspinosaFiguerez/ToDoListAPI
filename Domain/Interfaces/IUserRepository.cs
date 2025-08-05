namespace Domain.Interfaces;

public interface IUserRepository
{
    public Task<bool> ExistsAsync(string email);
    public Task<bool> ExistAsyncById(Guid id);
    public IQueryable<User> GetAllUsers();
    public Task<User?> GetUserById(Guid id);
    public Task<User> CreateUser(User request);
    public Task<User> UpdateUser(User request);
    public Task<bool> DeleteUser(User request);
    public IQueryable<ToDoTask> GetAllTasksAssignedToUser(Guid userId);
    public IQueryable<ToDoTask> GetTaskByIdAssignedToUser(Guid taskId, Guid userId);
}