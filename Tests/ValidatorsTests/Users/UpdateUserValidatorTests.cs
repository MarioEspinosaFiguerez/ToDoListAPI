namespace Tests.ValidatorsTests.Users
{
    public class UpdateUserValidatorTests
    {
        private readonly UpdateUserValidator _validator;
        private readonly Fixture _fixture;

        public UpdateUserValidatorTests()
        {
            _validator = new UpdateUserValidator();

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        public async Task Validator_WhenAllFieldsAreValid_PassesValidation()
        {
            var request = _fixture.Build<UpdateUserRequest>()
                                  .With(r => r.Name, "Pedro")
                                  .With(r => r.FirstSurname, "López")
                                  .With(r => r.SecondSurname, "Martínez")
                                  .With(r => r.Email, "update@example.com")
                                  .With(r => r.BirthDate, "1995-05-05")
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.True(result.IsValid);
        }

        [Fact]
        public async Task Validator_WhenEmailIsInvalid_FailsValidation()
        {
            var request = _fixture.Build<UpdateUserRequest>()
                                  .With(r => r.Email, "invalid-email")
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public async Task Validator_WhenBirthDateHasInvalidFormat_FailsValidation()
        {
            var request = _fixture.Build<UpdateUserRequest>()
                                  .With(r => r.BirthDate, "05-05-1995")
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "BirthDate");
        }

        [Fact]
        public async Task Validator_WhenBirthDateIsFuture_FailsValidation()
        {
            var request = _fixture.Build<UpdateUserRequest>()
                                  .With(r => r.BirthDate, DateTime.Now.AddYears(1).ToString("yyyy-MM-dd"))
                                  .Create();

            var result = await _validator.ValidateAsync(request);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "BirthDate");
        }
    }
}
