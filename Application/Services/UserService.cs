namespace Application.Services;

public class UserService : IUserService
{
    public readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository) => _userRepository = userRepository;

    public async Task<bool> ExistsAsync(string email) => await _userRepository.ExistsAsync(email);

    public async Task<bool> ExistsAsyncById(Guid id) => await _userRepository.ExistAsyncById(id);

    public IQueryable<UserDTO> GetAllUsers()
    {
        return _userRepository.GetAllUsers()
            .Select(user => new UserDTO
            {
                Id = user.Id,
                FullName = Helper.MergeIntoFullName(user.Name, user.FirstSurname, user.SecondSurname),
                Email = user.Email,
                BirthDate = user.BirthDate
            });
    }

    public async Task<UserDTO> GetUserById(Guid id)
    {
        User? user = await _userRepository.GetUserById(id);

        return user is null ? throw new NotFoundException($"The user with Id {id} doesn't exist", id) : new UserDTO
        {
            Id = user.Id,
            FullName = Helper.MergeIntoFullName(user.Name, user.FirstSurname, user.SecondSurname),
            Email = user.Email,
            BirthDate = user.BirthDate,
        };
    }

    public async Task<UserDTO> CreateUser(CreateUserRequest request, IPasswordHasher passwordHasher)
    {
        if (await _userRepository.ExistsAsync(request.Email)) throw new AlreadyExistException($"The user with the email {request.Email} already exists", request.Email);

        User entityRequest = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            FirstSurname = request.FirstSurname.Trim(),
            SecondSurname = String.IsNullOrEmpty(request.SecondSurname) ? null : request.SecondSurname.Trim(),
            Email = request.Email,
            PasswordHash = passwordHasher.Hash(request.Password),
            BirthDate = DateOnly.ParseExact(request.BirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture),
        };

        User createdUser = await _userRepository.CreateUser(entityRequest);

        return new UserDTO
        {
            Id = createdUser.Id,
            FullName = $"{request.Name} {request.FirstSurname} {request.SecondSurname}".Trim(),
            Email = createdUser.Email,
            BirthDate = createdUser.BirthDate,
        };
    }

    public async Task<UserDTO> UpdateUser(Guid id, UpdateUserRequest request)
    {
        User? userById = await _userRepository.GetUserById(id);

        if (userById is null) throw new NotFoundException($"The user with Id {id} doesn't exist", id);

        userById.Name = !string.IsNullOrEmpty(request.Name) ? request.Name : userById.Name;
        userById.FirstSurname = !string.IsNullOrEmpty(request.FirstSurname) ? request.FirstSurname : userById.FirstSurname;
        userById.SecondSurname = !string.IsNullOrEmpty(request.SecondSurname) ? request.SecondSurname : userById.SecondSurname;
        userById.BirthDate = !string.IsNullOrEmpty(request.BirthDate) ? DateOnly.Parse(request.BirthDate) : userById.BirthDate; ;
            
        userById.UpdatedAt = DateTime.UtcNow;
        
        if (!string.IsNullOrEmpty(request.Email))
        {
            bool alreadyExistEmail = await _userRepository.ExistsAsync(request.Email);
            userById.Email = alreadyExistEmail ? throw new AlreadyExistException($"The email {request.Email} is already in use", request.Email) : request.Email;
            userById.JwtTokenVersion++; // Invalidate past Tokens, needing to Re-login
        }

        await _userRepository.UpdateUser(userById);

        return new UserDTO
        {
            Id = id,
            FullName = Helper.MergeIntoFullName(userById.Name, userById.FirstSurname, userById.SecondSurname),
            Email = userById.Email,
            BirthDate = userById.BirthDate,
        };
    }

    public async Task<bool> DeleteUser(Guid id)
    {
        User? userToDelete = await _userRepository.GetUserById(id);

        if (userToDelete is null) throw new NotFoundException($"The user with Id {id} doesn't exist", id);

        return await _userRepository.DeleteUser(userToDelete);
    }
}