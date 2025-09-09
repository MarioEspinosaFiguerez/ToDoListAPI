namespace Tests.ValidatorsTests.Users
{
    public class CreateUserValidatorTests
    {
        private readonly CreateUserValidator _validator;
        private readonly Fixture _fixture;

        public CreateUserValidatorTests()
        {
            _validator = new CreateUserValidator();

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Validator_WhenValidRequest_PassesValidation()
        {
            var request = _fixture.Build<CreateUserRequest>()
                                  .With(r => r.Name, "Juan")
                                  .With(r => r.FirstSurname, "Pérez")
                                  .With(r => r.SecondSurname, "García")
                                  .With(r => r.Email, "test@example.com")
                                  .With(r => r.Password, "ValidPass1")
                                  .With(r => r.BirthDate, "2000-01-01")
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validator_WhenNameIsEmpty_FailsValidation()
        {
            var request = _fixture.Build<CreateUserRequest>()
                                  .With(r => r.Name, string.Empty)
                                  .With(r => r.FirstSurname, "Pérez")
                                  .With(r => r.Email, "test@example.com")
                                  .With(r => r.Password, "ValidPass1")
                                  .With(r => r.BirthDate, "2000-01-01")
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Name");
        }

        [Fact]
        public async Task Validator_WhenEmailIsInvalid_FailsValidation()
        {
            var request = _fixture.Build<CreateUserRequest>()
                                  .With(r => r.Email, "not-an-email")
                                  .With(r => r.Name, "Juan")
                                  .With(r => r.FirstSurname, "Pérez")
                                  .With(r => r.Password, "ValidPass1")
                                  .With(r => r.BirthDate, "2000-01-01")
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public async Task Validator_WhenBirthDateIsFuture_FailsValidation()
        {
            var request = _fixture.Build<CreateUserRequest>()
                                  .With(r => r.Name, "Juan")
                                  .With(r => r.FirstSurname, "Pérez")
                                  .With(r => r.Email, "test@example.com")
                                  .With(r => r.Password, "ValidPass1")
                                  .With(r => r.BirthDate, DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"))
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "BirthDate");
        }
    }
}
