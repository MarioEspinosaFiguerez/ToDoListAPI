namespace Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name of the user is mandatory")
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

        RuleFor(x => x.FirstSurname)
            .NotEmpty().WithMessage("First surname of the user is mandatory")
            .MaximumLength(100).WithMessage("First Surname must not exceed 100 characters");

        RuleFor(x => x.SecondSurname)
            .MaximumLength(100).WithMessage("Second surname must not exceed 100 characters");

        // Only looks for 1 '@' in the string that is not at the start
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is mandatory")
            .EmailAddress().WithMessage("Invalid email format. You need the format like sample@sample.com")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is mandatory")
            .MinimumLength(8).WithMessage("Password must be greater than 8 characters")
            .MaximumLength(128).WithMessage("Password must not exceed 128 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter");
        // To Add Special Charac to Password -> .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        RuleFor(x => x.BirthDate)
            .NotEmpty().WithMessage("User birthdate is mandatory");

        RuleFor(x => x.BirthDate)
            .Must(birthdate =>
            {
                if (!DateOnly.TryParseExact(birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)) return false;

                return parsedDate < DateOnly.FromDateTime(DateTime.Now);
            })
            .WithMessage("Birthdate should be a past date")
            .When(x => DateOnly.TryParseExact(x.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
     }
}

public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(50).WithMessage("Name must not exceed 50 characters");

        RuleFor(x => x.FirstSurname)
            .MaximumLength(100).WithMessage("First Surname must not exceed 100 characters");

        RuleFor(x => x.SecondSurname)
            .MaximumLength(100).WithMessage("Second surname must not exceed 100 characters");

        // Only looks for 1 '@' in the string that is not at the start
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format. You need the format like test@test.com")
            .MaximumLength(255).WithMessage("Email must not exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.BirthDate)
            .Must(birthdate => birthdate == null || DateOnly.TryParseExact(birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            .WithMessage("BirthDate must be null or a valid date with 'yyyy-MM-dd' format");

        RuleFor(x => x.BirthDate)
            .Must(birthdate =>
            {
                if (!DateOnly.TryParseExact(birthdate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)) return false;

                return parsedDate < DateOnly.FromDateTime(DateTime.Now);
            })
            .WithMessage("Birthdate should be a past date")
            .When(x => DateOnly.TryParseExact(x.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _));
    }
}