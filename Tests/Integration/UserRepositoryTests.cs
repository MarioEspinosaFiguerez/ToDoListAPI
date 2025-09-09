namespace Tests.Integration
{
    public class UserRepositoryTests
    {
        private TodoListDbContext CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TodoListDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new TodoListDbContext(options);
        }

        [Fact]
        public async Task CreateUser_WhenValidRequest_SavesAndReturnsUser()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var userRepository = new UserRepository(context);
            var taskRepository = new TaskRepository(context);
            var priorityRepository = new PriorityRepository(context);
            var stateRepository = new StateRepository(context);
            var service = new UserService(userRepository, taskRepository, priorityRepository, stateRepository);

            var request = new CreateUserRequest
            {
                Name = "Juan",
                FirstSurname = "Pérez",
                SecondSurname = "García",
                Email = "juan@example.com",
                Password = "Password123",
                BirthDate = "1990-05-05"
            };

            var passwordHasher = new PasswordHasher();

            // Act
            var result = await service.CreateUser(request, passwordHasher);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("juan@example.com", result.Email);

            var dbUser = await context.Users.FirstOrDefaultAsync(u => u.Email == "juan@example.com");
            Assert.NotNull(dbUser);
        }

        [Fact]
        public async Task GetUserById_WhenUserExists_ReturnsUserFromDb()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var userRepository = new UserRepository(context);
            var taskRepository = new TaskRepository(context);
            var priorityRepository = new PriorityRepository(context);
            var stateRepository = new StateRepository(context);
            var service = new UserService(userRepository, taskRepository, priorityRepository, stateRepository);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Ana",
                FirstSurname = "Lopez",
                Email = "ana@example.com",
                BirthDate = new DateOnly(1995, 1, 1),
                PasswordHash = "hash"
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await service.GetUserById(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("ana@example.com", result.Email);
            Assert.Equal("Ana Lopez", result.FullName);
        }

        [Fact]
        public async Task UpdateUser_WhenUserExists_UpdatesAndPersists()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var userRepository = new UserRepository(context);
            var taskRepository = new TaskRepository(context);
            var priorityRepository = new PriorityRepository(context);
            var stateRepository = new StateRepository(context);
            var service = new UserService(userRepository, taskRepository, priorityRepository, stateRepository);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Carlos",
                FirstSurname = "Sánchez",
                Email = "carlos@example.com",
                BirthDate = new DateOnly(1980, 10, 10),
                PasswordHash = "hash"
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            var updateRequest = new UpdateUserRequest
            {
                Name = "Carlos",
                FirstSurname = "Updated",
                Email = "carlos.updated@example.com"
            };

            // Act
            var result = await service.UpdateUser(user.Id, updateRequest);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Carlos Updated", result.FullName);
            Assert.Equal("carlos.updated@example.com", result.Email);

            var dbUser = await context.Users.FindAsync(user.Id);
            Assert.Equal("carlos.updated@example.com", dbUser!.Email);
        }

        [Fact]
        public async Task DeleteUser_WhenUserExists_RemovesFromDb()
        {
            // Arrange
            var context = CreateInMemoryDbContext();
            var userRepository = new UserRepository(context);
            var taskRepository = new TaskRepository(context);
            var priorityRepository = new PriorityRepository(context);
            var stateRepository = new StateRepository(context);
            var service = new UserService(userRepository, taskRepository, priorityRepository, stateRepository);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Laura",
                FirstSurname = "Gómez",
                Email = "laura@example.com",
                BirthDate = new DateOnly(1985, 3, 15),
                PasswordHash = "hash"
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            // Act
            var result = await service.DeleteUser(user.Id);

            // Assert
            Assert.True(result);
            Assert.False(await context.Users.AnyAsync(u => u.Id == user.Id));
        }
    }
}
