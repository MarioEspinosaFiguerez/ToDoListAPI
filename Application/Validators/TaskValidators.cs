namespace Application.Validators;

public class CreateToDoTaskDTOValidator : AbstractValidator<CreateToDoTaskRequest>
{
    public CreateToDoTaskDTOValidator(IUserService _userService)
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is mandatory")
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is mandatory")
            .MaximumLength(200);

        /*RuleFor(x => x.UserAssignedId)
            .NotEmpty().WithMessage("Assigned user is mandatory")
            .MustAsync(async (userId, cancellationToken) => await _userService.ExistsAsync(userId))
            .WithMessage(task => $"User with Id '{task.UserAssignedId}' doesn't exist.");*/

        RuleFor(x => x.Priority.Id)
            .GreaterThan(0).WithMessage("Priority must be one of the following values: Low (1), Medium (2), High (3).");

        RuleFor(x => x.Deadline)
            .GreaterThan(DateTimeOffset.Now).WithMessage("Deadline must be a future date");
    }
}

public class ToDoTaskDTOValidator : AbstractValidator<ToDoTaskDTO>
{
    public ToDoTaskDTOValidator(IUserService _userService)
    {
        Include(new CreateToDoTaskDTOValidator(_userService));

        RuleFor(x => x.State.Id)
           .GreaterThan(0).WithMessage("State must be one of the following values: Pending (1), InProgress (2), Completed (3).");
    }
}