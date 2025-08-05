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

        RuleFor(x => x.UserAssignedId)
            .NotEmpty().WithMessage("Assigned user is mandatory")
            .MustAsync(async (userId, cancellationToken) => await _userService.ExistsAsyncById(userId) )
            .WithMessage(task => $"User with Id '{task.UserAssignedId}' doesn't exist.");

        RuleFor(x => x.PriorityId)
            .InclusiveBetween(1, 3).WithMessage("PriorityId must be one of the following values: Low (1), Medium (2), High (3).");

        RuleFor(x => x.Deadline)
            .Cascade(CascadeMode.Stop) // If the first rule is false then the second one doesn't execute
            .NotEmpty().WithMessage("Deadline is mandatory")
            .Must(dateStr => DateTimeOffset.TryParse(dateStr, out var date) && date > DateTimeOffset.Now)
            .WithMessage("Deadline must be a future date");
    }
}
public class UpdateToDoTaskDTOValidator : AbstractValidator<UpdateToDoTaskRequest>
{
    public UpdateToDoTaskDTOValidator(IUserService _userService)
    {
        RuleFor(x => x.Title)
           .MaximumLength(50);

        RuleFor(x => x.Description)
            .MaximumLength(200);

        RuleFor(x => x.UserAssignedId)
            .MustAsync(async (userId, cancellationToken) => 
             userId.HasValue &&  await _userService.ExistsAsyncById(userId.Value))
            .When(x => x.UserAssignedId is not null)
            .WithMessage(task => $"User with Id '{task.UserAssignedId}' doesn't exist.");

        RuleFor(x => x.PriorityId)
            .InclusiveBetween(1, 3)
            .When(x => x.PriorityId.HasValue)
            .WithMessage("PriorityId must be one of the following values: Low (1), Medium (2), High (3).");

        RuleFor(x => x.StateId)
           .InclusiveBetween(1, 3)
           .When(x => x.PriorityId.HasValue)
           .WithMessage("State must be one of the following values: Pending (1), InProgress (2), Completed (3).");

        RuleFor(x => x.Deadline)
            .Must(dateStr => DateTimeOffset.TryParse(dateStr, out var date) && date > DateTimeOffset.Now)
            .When(x => !string.IsNullOrEmpty(x.Deadline))
            .WithMessage("Deadline must be a future date");
    }
}