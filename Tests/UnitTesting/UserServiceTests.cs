namespace Tests.UnitTesting;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<ITaskRepository> _taskRepository = new();
    private readonly Mock<IPriorityRepository> _priorityRepository = new();
    private readonly Mock<IStateRepository> _stateRepository = new();
    private readonly Mock<IPasswordHasher> _passwordHasher = new();
    private readonly Fixture _fixture = new();

    public UserServiceTests()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private UserService CreateService() => new(_userRepository.Object, _taskRepository.Object, _priorityRepository.Object, _stateRepository.Object);

    [Fact]
    public void GetAllUsers_WhenContentExists_ReturnsAllUsers()
    {
        var fakeUsers = _fixture.Build<User>()
                                .With(u => u.Id, Guid.NewGuid())
                                .With(u => u.BirthDate, new DateOnly(1990, 1, 1))
                                .CreateMany(5)
                                .ToList();

        _userRepository.Setup(r => r.GetAllUsers()).Returns(fakeUsers.AsQueryable());

        var service = CreateService();
        var result = service.GetAllUsers().ToList();

        Assert.Equal(5, result.Count);
        for (int i = 0; i < 5; i++)
        {
            Assert.Equal(fakeUsers[i].Id, result[i].Id);
            Assert.Equal($"{fakeUsers[i].Name} {fakeUsers[i].FirstSurname} {fakeUsers[i].SecondSurname}", result[i].FullName);
        }
    }

    [Fact]
    public void GetAllUsers_WhenNoContent_ReturnsEmpty()
    {
        _userRepository.Setup(r => r.GetAllUsers()).Returns(Enumerable.Empty<User>().AsQueryable());

        var service = CreateService();
        var result = service.GetAllUsers();

        Assert.Empty(result);
    }

    [Fact]
    public async Task GetUserById_WhenUserExists_ReturnsUser()
    {
        var user = _fixture.Build<User>()
                           .With(u => u.Id, Guid.NewGuid())
                           .With(u => u.BirthDate, new DateOnly(1995, 5, 5))
                           .Create();

        _userRepository.Setup(r => r.GetUserById(user.Id)).ReturnsAsync(user);

        var service = CreateService();
        var result = await service.GetUserById(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserById_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();
        _userRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync((User?)null);

        var service = CreateService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.GetUserById(userId));
        Assert.Contains(userId.ToString(), exception.Message);
    }

    [Fact]
    public async Task ExistUserById_WhenUserExists_ReturnsTrue()
    {
        var userId = Guid.NewGuid();
        _userRepository.Setup(r => r.ExistAsyncById(userId)).ReturnsAsync(true);

        var service = CreateService();
        var result = await service.ExistsAsyncById(userId);

        Assert.True(result);
    }

    [Fact]
    public async Task ExistUserById_WhenUserDoesNotExist_ReturnsFalse()
    {
        var userId = Guid.NewGuid();
        _userRepository.Setup(r => r.ExistAsyncById(userId)).ReturnsAsync(false);

        var service = CreateService();
        var result = await service.ExistsAsyncById(userId);

        Assert.False(result);
    }

    [Fact]
    public async Task CreateUser_WhenValidRequest_ReturnsCreatedUser()
    {
        var request = _fixture.Build<CreateUserRequest>()
                              .With(r => r.BirthDate, "2000-01-01")
                              .With(r => r.Email, "test@example.com")
                              .Create();

        var createdUser = _fixture.Build<User>()
                                  .With(u => u.Id, Guid.NewGuid())
                                  .With(u => u.Name, request.Name)
                                  .With(u => u.Email, request.Email)
                                  .With(u => u.BirthDate, new DateOnly(2000, 1, 1))
                                  .Create();

        _userRepository.Setup(r => r.CreateUser(It.IsAny<User>())).ReturnsAsync(createdUser);

        var service = CreateService();
        var result = await service.CreateUser(request, _passwordHasher.Object);

        Assert.Equal(createdUser.Id, result.Id);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public async Task UpdateUser_WhenUserExists_UpdatesAndReturnsUser()
    {
        var userId = Guid.NewGuid();
        var existingUser = _fixture.Build<User>()
                                   .With(u => u.Id, userId)
                                   .With(u => u.BirthDate, new DateOnly(1990, 1, 1))
                                   .Create();

        var request = _fixture.Build<UpdateUserRequest>()
                              .With(r => r.Email, "updated@example.com")
                              .With(r => r.BirthDate, "2000-01-01")
                              .Create();

        var updatedUser = existingUser;
        updatedUser.Email = request.Email;

        _userRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync(existingUser);
        _userRepository.Setup(r => r.UpdateUser(existingUser)).ReturnsAsync(updatedUser);

        var service = CreateService();
        var result = await service.UpdateUser(userId, request);

        Assert.Equal("updated@example.com", result.Email);
    }

    [Fact]
    public async Task UpdateUser_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();
        var request = _fixture.Create<UpdateUserRequest>();

        _userRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync((User?)null);

        var service = CreateService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateUser(userId, request));
        Assert.Contains(userId.ToString(), exception.Message);
    }

    [Fact]
    public async Task DeleteUser_WhenUserExists_ReturnsTrue()
    {
        var user = _fixture.Build<User>()
                           .With(u => u.Id, Guid.NewGuid())
                           .With(u => u.BirthDate, new DateOnly(1990, 1, 1))
                           .Create();

        _userRepository.Setup(r => r.GetUserById(user.Id)).ReturnsAsync(user);
        _userRepository.Setup(r => r.DeleteUser(user)).ReturnsAsync(true);

        var service = CreateService();
        var result = await service.DeleteUser(user.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteUser_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        var userId = Guid.NewGuid();

        _userRepository.Setup(r => r.GetUserById(userId)).ReturnsAsync((User?)null);

        var service = CreateService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteUser(userId));
        Assert.Contains(userId.ToString(), exception.Message);
    }
}
