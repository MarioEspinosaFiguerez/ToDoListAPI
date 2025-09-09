namespace Tests.UnitTesting;

public class TasksServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepository = new();
    private readonly Mock<IUserRepository> _userRepository = new();
    private readonly Mock<IPriorityRepository> _priorityRepository = new();
    private readonly Mock<IStateRepository> _stateRepository = new();
    private readonly Fixture _fixture = new();

    public TasksServiceTests()
    {
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    private TaskService CreateService() => new TaskService(_taskRepository.Object, _userRepository.Object, _priorityRepository.Object, _stateRepository.Object);

    [Fact]
    public void GetAllTasks_WhenContentExists_ReturnsAllTasks()
    {
        // Arrange

        // Craetes valid data for testing
        var fixture = new Fixture();

        // Quitting recursion & Add new politic
        fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => fixture.Behaviors.Remove(b));
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        var fakeUser = fixture.Build<User>()
                                .With(u => u.Id, Guid.NewGuid())
                                .With(u => u.BirthDate, new DateOnly(1990, 1, 1))
                                .Create();

        var priority = LookupValues.LowPriority;
        var state = LookupValues.ComplemetedState;

        var fakeTasks = fixture.Build<ToDoTask>()
                                .With(t => t.AssignedTo, fakeUser)
                                .With(t => t.UserAssignedId, fakeUser.Id)
                                .With(t => t.Priority, priority)
                                .With(t => t.State, state)
                                .CreateMany(5)
                                .ToList();

        _taskRepository.Setup(r => r.GetAllTasks()).Returns(fakeTasks.AsQueryable());

        // Act
        var service = CreateService();
        var result = service.GetAllTasks().ToList();


        // Assert
        Assert.Equal(5, result.Count);

        // Verifying some mapped fields
        for (int i = 0; i < 5; i++)
        {
            Assert.Equal(fakeTasks[i].Title, result[i].Title);
            Assert.Equal(fakeTasks[i].PriorityId, result[i].Priority.Id);
            Assert.Equal(fakeTasks[i].Priority.Name, result[i].Priority.Name);
            Assert.Equal(fakeTasks[i].StateId, result[i].State.Id);
            Assert.Equal(fakeTasks[i].State.Name, result[i].State.Name);
            Assert.Equal(fakeUser.Id, result[i].UserAssignedId);
        }
    }

    [Fact]
    public void GetAllTasks_WhenNoContent_ReturnsEmpty()
    {
        // Arrange
        _taskRepository.Setup(r => r.GetAllTasks()).Returns(Enumerable.Empty<ToDoTask>().AsQueryable());

        var service = CreateService();

        // Act
        var result = service.GetAllTasks();

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskExists_ReturnsTask()
    {
        // Arrange
        var fakeUser = new User
        {
            Id = Guid.NewGuid(),
            Name = "Juan",
            FirstSurname = "Pérez",
            SecondSurname = "Gómez",
            Email = null,
            PasswordHash = null,
            BirthDate = new DateOnly(1990, 1, 1)
        };

        var priority = LookupValues.LowPriority;
        var state = LookupValues.InProgressState;

        var task = new ToDoTask
        {
            Id = Guid.NewGuid(),
            UserAssignedId = fakeUser.Id,
            AssignedTo = fakeUser,
            Title = "Task 1",
            Description = "Desc",
            Priority = priority,     
            PriorityId = priority.Id,
            State = state,           
            StateId = state.Id,
            Deadline = DateTimeOffset.Now
        };

        _taskRepository.Setup(r => r.GetTaskById(task.Id)).ReturnsAsync(task);

        // Act
        var service = CreateService();
        var result = await service.GetTaskById(task.Id);


        // Assert
        Assert.NotNull(result);
        Assert.Equal(task.Id, result.Id);
        Assert.Equal(fakeUser.Id, result.UserAssignedId);
        Assert.Equal($"{fakeUser.Name} {fakeUser.FirstSurname} {fakeUser.SecondSurname}", result.AssignedToUserName);
        Assert.Equal(priority.Id, result.Priority.Id);
        Assert.Equal(priority.Name, result.Priority.Name);
        Assert.Equal(state.Id, result.State.Id);
        Assert.Equal(state.Name, result.State.Name);
    }

    [Fact]
    public async Task GetTaskById_WhenTaskDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var taskId = Guid.NewGuid();

        // Null to simulated NotFound exception
        _taskRepository.Setup(r => r.GetTaskById(taskId)).ReturnsAsync((ToDoTask?)null);

        // Act & Assert
        var service = CreateService();
        var exception = await Assert.ThrowsAsync<NotFoundException>(async () => await service.GetTaskById(taskId));

        Assert.Contains(taskId.ToString(), exception.Message);
    }

    [Fact]
    public async Task ExistTaskById_TaskExists_ReturnTrue()
    {
        var taskId = Guid.NewGuid();
        _taskRepository.Setup(r => r.ExistTaskById(taskId)).ReturnsAsync(true);

        var service = CreateService();

        var result = await service.ExistTaskById(taskId);
        Assert.True(result);
    }

    [Fact]
    public async Task ExistTaskById_TasksDoesNotExists_ReturnFalse()
    {
        var taskId = Guid.NewGuid();
        _taskRepository.Setup(r => r.ExistTaskById(taskId)).ReturnsAsync(false);

        var service = CreateService();

        var result = await service.ExistTaskById(taskId);
        Assert.False(result);
    }

    [Fact]
    public async Task CreateTask_WhenUserPriorityAndStateExists_ReturnsCreatedTask()
    {
        var user = _fixture.Build<User>()
                           .With(u => u.Id, Guid.NewGuid())
                           .With(u => u.Name, "Ana")
                           .With(u => u.BirthDate, new DateOnly(2005, 1, 1))
                           .Create();

        var request = _fixture.Build<CreateToDoTaskRequest>()
                              .With(r => r.UserAssignedId, user.Id)
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(1).ToString())
                              .Create();

        var createdTask = _fixture.Build<ToDoTask>()
                                  .With(t => t.UserAssignedId, user.Id)
                                  .With(t => t.AssignedTo, user)
                                  .With(t => t.PriorityId, request.PriorityId)
                                  .With(t => t.StateId, 1)
                                  .With(t => t.Deadline, DateTimeOffset.Parse(request.Deadline))
                                  .Create();

        _userRepository.Setup(r => r.GetUserById(user.Id)).ReturnsAsync(user);
        _taskRepository.Setup(r => r.CreateTask(It.IsAny<ToDoTask>())).ReturnsAsync(createdTask);
        _priorityRepository.Setup(r => r.GetPriorityEnumName(request.PriorityId)).ReturnsAsync("Low");
        _stateRepository.Setup(r => r.GetStateEnumName(createdTask.StateId)).ReturnsAsync("Pending");

        var service = CreateService();

        var result = await service.CreateTask(request);

        Assert.Equal(createdTask.Title, result.Title);
        Assert.Equal("Low", result.Priority.Name);
        Assert.Equal("Pending", result.State.Name);
        Assert.Equal($"{user.Name} {user.FirstSurname} {user.SecondSurname}", result.AssignedToUserName);
    }

    [Fact]
    public async Task CreateTask_WhenUserDoesNotExist_ThrowsNotFoundException()
    {
        var request = _fixture.Build<CreateToDoTaskRequest>()
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(1).ToString())
                              .Create();

        _userRepository.Setup(r => r.GetUserById(request.UserAssignedId)).ReturnsAsync((User?)null);

        var service = CreateService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.CreateTask(request));
        Assert.Contains(request.UserAssignedId.ToString(), exception.Message);
    }

    [Fact]
    public async Task CreateTask_WhenPriorityDoesNotExist_ThrowsNotFoundException()
    {
        var user = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .With(u => u.BirthDate, new DateOnly(1990, 1, 1))
            .Create();
        var request = _fixture.Build<CreateToDoTaskRequest>()
                              .With(r => r.UserAssignedId, user.Id)
                              .With(r => r.PriorityId, 999)
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(1).ToString())
                              .Create();

        var createdTask = _fixture.Build<ToDoTask>()
                                  .With(t => t.UserAssignedId, user.Id)
                                  .With(t => t.AssignedTo, user)
                                  .With(t => t.PriorityId, request.PriorityId)
                                  .With(t => t.StateId, 1)
                                  .Create();

        _userRepository.Setup(r => r.GetUserById(user.Id)).ReturnsAsync(user);
        _taskRepository.Setup(r => r.CreateTask(It.IsAny<ToDoTask>())).ReturnsAsync(createdTask);
        _priorityRepository.Setup(r => r.GetPriorityEnumName(request.PriorityId)).ReturnsAsync((string?)null);

        var service = CreateService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.CreateTask(request));
        Assert.Contains(request.PriorityId.ToString(), exception.Message);
    }

    [Fact]
    public async Task UpdateTask_WhenTaskExists_UpdatesAndReturnsTask()
    {
        var taskId = Guid.NewGuid();
        var fakeUser = _fixture.Build<User>()
            .With(u => u.Id, Guid.NewGuid())
            .With(u => u.BirthDate, new DateOnly(2005, 1, 1))
            .Create();
        var existingTask = _fixture.Build<ToDoTask>()
                                   .With(t => t.Id, taskId)
                                   .With(t => t.UserAssignedId, fakeUser.Id)
                                   .With(t => t.AssignedTo, fakeUser)
                                   .With(t => t.PriorityId, 1)
                                   .With(t => t.StateId, 1)
                                   .With(t => t.Deadline, DateTimeOffset.Now)
                                   .Create();

        var request = _fixture.Build<UpdateToDoTaskRequest>()
                              .With(r => r.PriorityId, 2)
                              .With(r => r.StateId, 2)
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(1).ToString())
                              .Create();

        _taskRepository.Setup(r => r.GetTaskById(taskId)).ReturnsAsync(existingTask);
        _taskRepository.Setup(r => r.UpdateTask(existingTask)).ReturnsAsync(existingTask);
        _priorityRepository.Setup(r => r.GetPriorityEnumName(2)).ReturnsAsync("Medium");
        _stateRepository.Setup(r => r.GetStateEnumName(2)).ReturnsAsync("InProgress");

        var service = CreateService();

        var result = await service.UpdateTask(taskId, request);

        Assert.Equal(request.Title ?? existingTask.Title, result.Title);
        Assert.Equal("Medium", result.Priority.Name);
        Assert.Equal("InProgress", result.State.Name);
    }

    [Fact]
    public async Task UpdateTask_WhenTaskDoesNotExist_ThrowsNotFoundException()
    {
        var taskId = Guid.NewGuid();
        var request = _fixture.Create<UpdateToDoTaskRequest>();

        _taskRepository.Setup(r => r.GetTaskById(taskId)).ReturnsAsync((ToDoTask?)null);

        var service = CreateService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.UpdateTask(taskId, request));
        Assert.Contains(taskId.ToString(), exception.Message);
    }

    [Fact]
    public async Task DeleteTask_WhenTaskExists_ReturnsTrue()
    {
        var task = _fixture.Build<ToDoTask>()
                           .With(t => t.Id, Guid.NewGuid())
                           .With(t => t.UserAssignedId, Guid.NewGuid())
                           .With(t => t.AssignedTo, _fixture.Build<User>()
                                                      .With(u => u.Id, Guid.NewGuid())
                                                      .With(u => u.BirthDate, new DateOnly(2005, 1, 1)) 
                                                      .Create())
                           .Create();

        _taskRepository.Setup(r => r.GetTaskById(task.Id)).ReturnsAsync(task);
        _taskRepository.Setup(r => r.DeleteTask(task)).ReturnsAsync(true);

        var service = CreateService();

        var result = await service.DeleteTask(task.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteTask_WhenTaskDoesNotExist_ThrowsNotFoundException()
    {
        var taskId = Guid.NewGuid();

        _taskRepository.Setup(r => r.GetTaskById(taskId)).ReturnsAsync((ToDoTask?)null);

        var service = CreateService();

        var exception = await Assert.ThrowsAsync<NotFoundException>(() => service.DeleteTask(taskId));
        Assert.Contains(taskId.ToString(), exception.Message);
    }
}