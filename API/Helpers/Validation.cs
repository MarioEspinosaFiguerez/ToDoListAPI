namespace API.Helpers;

public static class Validation
{
    public static IResult GetValidationProblemErrors(ValidationResult result)
    {
        return Results.ValidationProblem(result.Errors.GroupBy(err => err.PropertyName)
                        .ToDictionary(err => err.Key, errorValue => errorValue.Select(value => value.ErrorMessage).ToArray()));
    }
}