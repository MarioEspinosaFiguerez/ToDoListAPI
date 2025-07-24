namespace API.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try { await _next(context); }
        catch (Exception ex) 
        {
            Log.Error(ex, "Unhandled exception intercepted in middleware. Path of the request: {Path}, Method: {Method}", context.Request.Path, context.Request.Method);
            await HandleExceptionAsync(context, ex); 
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Standard - RFC 7807 (ProblemDetailsfor HTTP APIS) // Standard - RFC 7231 (Define Http State Codes)
        context.Response.ContentType = "application/problem+json";
        ProblemDetails errorProblemDetails;

        if (exception is ValidationException validationException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            // Grouping the errors by Property Name that is not valid -> make a dictionary with (err.properyName as Key & err.ErrorMessage as Value)
            var errors = validationException.Errors.GroupBy(err => err.PropertyName).ToDictionary(err => err.Key, err => err.Select(err => err.ErrorMessage).ToArray());

            // Custom Problem Details for Validation purposes (The class is extended from ProblemDetails class)
            ValidationProblemDetails validationProblemDetails = new ValidationProblemDetails(errors)
            {
                Title = "Validation errors occurred",
                Status = StatusCodes.Status400BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            // Serializing the data to send to the client as Json
            await context.Response.WriteAsync(JsonSerializer.Serialize(validationProblemDetails));
            return;
        }

        context.Response.StatusCode = GetExceptionErrorStatusCode(exception);

        errorProblemDetails = new ProblemDetails
        {
            Title = GetExceptionErrorTitle(exception, context.Response.StatusCode),
            Status = context.Response.StatusCode,
            Detail = exception.Message,
            Instance = context.Request.Path
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(errorProblemDetails));
    }

    private static int GetExceptionErrorStatusCode(Exception ex)
    {
        return ex switch
        {
            KeyNotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            DbUpdateConcurrencyException => StatusCodes.Status409Conflict,
            DbUpdateException => StatusCodes.Status500InternalServerError,
            SqlException => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
    }

    private static string GetExceptionErrorTitle(Exception ex, int statusCode) => ReasonPhrases.GetReasonPhrase(statusCode) ?? ex.GetType().Name;
}
