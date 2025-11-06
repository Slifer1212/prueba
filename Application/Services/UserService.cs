using Application.Dto.UserDto;
using Application.Interfaces;
using Application.IServices;
using Application.Validations;
using Domain.Entities;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class UserService: BaseService<User, CreateUserDto, UpdateUserDto, ReadUserDto>, IUserService
{
    private  CreateUserValidator _createUserValidator;
    private UpdateUserValidator _updateUserValidator;
    private readonly IUserRepository _userRepository;
    

    public UserService(IMapper mapper, IBaseRepository<User> repository, ILogger<BaseService<User, CreateUserDto, UpdateUserDto, ReadUserDto>> logger, CreateUserValidator validator, IUserRepository userRepository, UpdateUserValidator updateUserValidator) : base(mapper, repository, logger)
    {
        _createUserValidator = validator;
        _userRepository = userRepository;
        _updateUserValidator = updateUserValidator;
    }

    public async override Task<ReadUserDto> CreateAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
    {
        var existingEmail = CheckUniqueEmail(dto.Email);
        
        if (existingEmail != null)
        {
            throw new InvalidOperationException("El email ya está registrado");
        }
        
        _createUserValidator.ValidateAndThrow(dto);

        var user = new User()
        {
            Email = dto.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            FirstName = dto.FirstName,
            LastName = dto.LastName,
        };
        
        await _userRepository.AddAsync(user, cancellationToken);
        Logger.LogInformation($"Creating new user: {user.Email}");
        return Mapper.Adapt<ReadUserDto>();
    }

    public async override Task<ReadUserDto?> UpdateAsync(int id, UpdateUserDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (user == null)
        {
            throw new KeyNotFoundException($"No user found with id: {id}");
        }

        if (dto.Email != user.Email)
        {
            var existingEmail = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);
            if (existingEmail != null && existingEmail.Email != user.Email)
            {
                throw new KeyNotFoundException($"Email {dto.Email} already exists");
            }
        }
        _updateUserValidator.ValidateAndThrow(dto);

        
        user.Email = dto.Email;
        if (dto.Password != user.Password)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        }
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        
        await _userRepository.UpdateAsync(user, cancellationToken);
        Logger.LogInformation($"Updating user: {user.Email}");
        return Mapper.Adapt<ReadUserDto>();
    }

    public async Task<bool> CheckUniqueEmail(string email, CancellationToken cancellationToken = default)
    {
        var existingUser = await _userRepository.GetByEmailAsync(email, cancellationToken);
        return existingUser != null;
    }
}