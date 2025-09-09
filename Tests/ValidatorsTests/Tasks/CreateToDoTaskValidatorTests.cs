namespace Tests.ValidatorsTests.Tasks;

public class CreateToDoTaskDTOValidatorTests
{
    private readonly Mock<IUserService> _userServiceMock = new();
    private readonly CreateToDoTaskDTOValidator _validator;
    private readonly Fixture _fixture;

    public CreateToDoTaskDTOValidatorTests()
    {
        _validator = new CreateToDoTaskDTOValidator(_userServiceMock.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Validator_WhenValidRequest_PassesValidation()
    {
        var userId = Guid.NewGuid();

        var request = _fixture.Build<CreateToDoTaskRequest>()
                              .With(r => r.UserAssignedId, userId)
                              .With(r => r.Title, "Título válido")
                              .With(r => r.Description, "Descripción válida") 
                              .With(r => r.PriorityId, 2)                      
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(1).ToString())
                              .Create();

        _userServiceMock.Setup(s => s.ExistsAsyncById(userId)).ReturnsAsync(true);

        var result = await _validator.ValidateAsync(request);

        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validator_WhenMissingTitle_FailsValidation()
    {
        var userId = Guid.NewGuid();

        var request = _fixture.Build<CreateToDoTaskRequest>()
                              .With(r => r.Title, string.Empty)
                              .With(r => r.UserAssignedId, userId)
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(1).ToString())
                              .Create();

        _userServiceMock.Setup(s => s.ExistsAsyncById(userId)).ReturnsAsync(true);

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public async Task Validator_WhenUserDoesNotExist_FailsValidation()
    {
        var userId = Guid.NewGuid();

        var request = _fixture.Build<CreateToDoTaskRequest>()
                              .With(r => r.UserAssignedId, userId)
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(1).ToString())
                              .Create();

        _userServiceMock.Setup(s => s.ExistsAsyncById(userId)).ReturnsAsync(false);

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(userId.ToString()));
    }

    [Fact]
    public async Task Validator_WhenDeadlineIsPast_FailsValidation()
    {
        var userId = Guid.NewGuid();

        var request = _fixture.Build<CreateToDoTaskRequest>()
                              .With(r => r.UserAssignedId, userId)
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(-1).ToString())
                              .Create();

        _userServiceMock.Setup(s => s.ExistsAsyncById(userId)).ReturnsAsync(true);

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Deadline");
    }
}
