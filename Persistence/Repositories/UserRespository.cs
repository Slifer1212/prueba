using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistence.DbContext;

namespace Persistence.Repositories;

public class UserRespository : BaseRepository<User>, IUserRepository
{
    public UserRespository(ApplicationDbContext context, ILogger<BaseRepository<User>> logger) : base(context, logger)
    {
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _entities.Where(e => e.Email == email).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<User?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await _entities.Where(e => e.FirstName == name).FirstOrDefaultAsync(cancellationToken);
    }
}