namespace Application.Validators;

public class ToDoTaskDtoValidator : AbstractValidator<ToDoTaskDTO>
{
    public ToDoTaskDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is mandatory")
            .MaximumLength(50);

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is mandatory")
            .MaximumLength(200);

        RuleFor(x => x.UserAssignedId)
            .NotEmpty().WithMessage("Assigned user is mandatory");
    }
}