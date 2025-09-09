namespace Tests.ValidatorsTests.Tasks;

public class UpdateToDoTaskValidatorTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UpdateToDoTaskDTOValidator _validator;
    private readonly Fixture _fixture;

    public UpdateToDoTaskValidatorTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _validator = new UpdateToDoTaskDTOValidator(_userServiceMock.Object);

        _fixture = new Fixture();
        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Fact]
    public async Task Validate_WhenAllFieldsAreValid_ShouldPass()
    {
        // Arrange
        var userId = Guid.NewGuid();

        var request = _fixture.Build<UpdateToDoTaskRequest>()
                              .With(r => r.Title, "Titulo válido")
                              .With(r => r.Description, "Descripción válida")
                              .With(r => r.PriorityId, 2)
                              .With(r => r.StateId, 2)
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(2).ToString())
                              .With(r => r.UserAssignedId, userId)
                              .Create();

        _userServiceMock.Setup(s => s.ExistsAsyncById(userId)).ReturnsAsync(true);

        // Act
        var result = await _validator.ValidateAsync(request);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task Validate_WhenDeadlineIsPast_ShouldFail()
    {
        var request = _fixture.Build<UpdateToDoTaskRequest>()
                              .With(r => r.Deadline, DateTimeOffset.Now.AddDays(-1).ToString())
                              .Create();

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Deadline");
    }

    [Fact]
    public async Task Validate_WhenUserDoesNotExist_ShouldFail()
    {
        var userId = Guid.NewGuid();
        var request = _fixture.Build<UpdateToDoTaskRequest>()
                              .With(r => r.UserAssignedId, userId)
                              .Create();

        _userServiceMock.Setup(s => s.ExistsAsyncById(userId)).ReturnsAsync(false);

        var result = await _validator.ValidateAsync(request);

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage.Contains(userId.ToString()));
    }
}