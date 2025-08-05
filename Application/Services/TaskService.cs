namespace Application.Services;

public class TaskService : ITaskService
{
    public readonly ITaskRepository _taskRepository;
    public readonly IUserRepository _userRepository;
    public readonly IPriorityRepository _priorityRepository;
    public readonly IStateRepository _stateRepository;

    public readonly int STATEID_PENDING_BY_DEFAULT = 1;

    public TaskService(ITaskRepository taskRepository, IUserRepository userRepository, IPriorityRepository priorityRepository, IStateRepository stateRepository)
    {
        _taskRepository = taskRepository;
        _userRepository = userRepository;
        _priorityRepository = priorityRepository;
        _stateRepository = stateRepository;
    }

    public IQueryable<ToDoTaskResponse> GetAllTasks()
    {
        return _taskRepository.GetAllTasks()
            .Select(task => new ToDoTaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                UserAssignedId = task.UserAssignedId,
                AssignedToUserName = task.AssignedTo != null ? $"{task.AssignedTo.Name} {task.AssignedTo.FirstSurname} {task.AssignedTo.SecondSurname}" : "Sin asignar",
                Priority = new EnumDTO(task.PriorityId, task.Priority.Name),
                State = new EnumDTO(task.StateId, task.State.Name),
                Deadline = task.Deadline,
            });
    }

    public async Task<bool> ExistTaskById(Guid id) => await _taskRepository.ExistTaskById(id);

    public async Task<ToDoTaskResponse?> GetTaskById(Guid id)
    {
        ToDoTask? task = await _taskRepository.GetTaskById(id);

        return task is null ? 
            throw new NotFoundException($"The task with Id {id} doesn't exist", id) :
            new ToDoTaskResponse
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                UserAssignedId = task.UserAssignedId,
                AssignedToUserName = task.AssignedTo != null ? $"{task.AssignedTo.Name} {task.AssignedTo.FirstSurname} {task.AssignedTo.SecondSurname}" : "Sin asignar",
                Priority = new EnumDTO(task.PriorityId, task.Priority.Name),
                State = new EnumDTO(task.StateId, task.State.Name),
                Deadline = task.Deadline
            };
    }

    public async Task<ToDoTaskResponse> CreateTask(CreateToDoTaskRequest request)
    {
        User? userExists = await _userRepository.GetUserById(request.UserAssignedId);

        if (userExists is null) throw new NotFoundException($"The User with ID {request.UserAssignedId} not found", request.UserAssignedId);

        ToDoTask entityRequest = new ToDoTask
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            AssignedTo = userExists,
            UserAssignedId = request.UserAssignedId,
            PriorityId = request.PriorityId,
            StateId = STATEID_PENDING_BY_DEFAULT,
            Deadline = DateTimeOffset.Parse(request.Deadline)
        };

        ToDoTask createdTask = await _taskRepository.CreateTask(entityRequest);

        var priorityEnumName = await _priorityRepository.GetPriorityEnumName(request.PriorityId);
        if (priorityEnumName is null) throw new NotFoundException($"The Priority with ID {entityRequest.PriorityId} not found", request.PriorityId);
        var stateEnumName = await _stateRepository.GetStateEnumName(entityRequest.StateId);
        if (stateEnumName is null) throw new NotFoundException($"The State with ID {entityRequest.StateId} not found", entityRequest.StateId);

        return new ToDoTaskResponse
        {
            Id = createdTask.Id,
            Title = createdTask.Title,
            Description = createdTask.Description,
            UserAssignedId = createdTask.UserAssignedId,
            AssignedToUserName = createdTask.AssignedTo != null ? $"{createdTask.AssignedTo.Name} {createdTask.AssignedTo.FirstSurname} {createdTask.AssignedTo.SecondSurname}" : "Sin asignar",
            Priority = new EnumDTO(createdTask.PriorityId, priorityEnumName),
            State = new EnumDTO(createdTask.StateId, stateEnumName),
            Deadline = createdTask.Deadline
        };
    }

    public async Task<ToDoTaskResponse> UpdateTask(Guid id, UpdateToDoTaskRequest request)
    {
        string? priorityEnum = "", stateEnum = "";

        ToDoTask? taskById = await _taskRepository.GetTaskById(id);
        if (taskById is null) throw new NotFoundException($"The task with Id {id} doesn't exist", id);

        taskById.Title = !string.IsNullOrEmpty(request.Title) ? request.Title : taskById.Title;
        taskById.Description = !string.IsNullOrEmpty(request.Description) ? request.Description : taskById.Description;
        taskById.Deadline = !string.IsNullOrEmpty(request.Deadline) ? DateTimeOffset.Parse(request.Deadline) : taskById.Deadline;
        taskById.PriorityId = request.PriorityId.HasValue ? request.PriorityId.Value : taskById.PriorityId;
        taskById.StateId = request.StateId.HasValue ? request.StateId.Value : taskById.StateId;
        taskById.UserAssignedId = request.UserAssignedId.HasValue ? request.UserAssignedId.Value : taskById.UserAssignedId;
        taskById.UpdatedAt = DateTimeOffset.Now;

        await _taskRepository.UpdateTask(taskById);

        if (request.PriorityId.HasValue)
        {
            priorityEnum = await _priorityRepository.GetPriorityEnumName(request.PriorityId.Value);
            if (priorityEnum is null) throw new NotFoundException($"The Priority with ID {request.PriorityId.Value} not found", request.PriorityId.Value);
        }
        else
        {
            priorityEnum = taskById.Priority.Name;
        }
        if (request.StateId.HasValue)
        {
            stateEnum = await _stateRepository.GetStateEnumName(request.StateId.Value);
            if (stateEnum is null) throw new NotFoundException($"The Priority with ID {request.StateId.Value} not found", request.StateId.Value);
        }
        else
        {
            priorityEnum = taskById.State.Name;
        }

            return new ToDoTaskResponse
            {
                Id = id,
                Title = taskById.Title,
                Description = taskById.Description,
                Priority = new EnumDTO(taskById.PriorityId, priorityEnum),
                State = new EnumDTO(taskById.StateId, stateEnum),
                UserAssignedId = taskById.UserAssignedId,
                AssignedToUserName = taskById.AssignedTo != null ? $"{taskById.AssignedTo.Name} {taskById.AssignedTo.FirstSurname} {taskById.AssignedTo.SecondSurname}" : "Sin asignar",
                Deadline = taskById.Deadline,
            };
    }

    public async Task<bool> DeleteTask(Guid id)
    {
        ToDoTask? task = await _taskRepository.GetTaskById(id);
        if (task is null) throw new NotFoundException($"The task with Id {id} doesn't exist", id);

        return await _taskRepository.DeleteTask(task);
    }
}