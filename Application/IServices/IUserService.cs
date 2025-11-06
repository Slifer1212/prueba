using Application.Dto;
using Application.Dto.UserDto;
using Domain.Entities;

namespace Application.IServices;

public interface IUserService : IBaseService<User, CreateUserDto, UpdateUserDto, ReadUserDto>
{
    Task<bool> CheckUniqueEmail(string email , CancellationToken cancellationToken = default);
}