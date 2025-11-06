using Application.Interfaces;
using Application.IServices;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services;

public class BaseService<TEntity, TCreateDto, TUpdateDto, TReadDto> :
    IBaseService<TEntity, TCreateDto, TUpdateDto, TReadDto>
    where TEntity : class, new()
    where TCreateDto : class
    where TUpdateDto : class
{
    protected readonly IBaseRepository<TEntity> Repository;
    protected readonly IMapper Mapper;
    protected readonly ILogger<BaseService<TEntity, TCreateDto, TUpdateDto, TReadDto>> Logger;


    public BaseService(IMapper mapper, IBaseRepository<TEntity> repository, ILogger<BaseService<TEntity, TCreateDto, TUpdateDto, TReadDto>> logger)
    {
        Mapper = mapper;
        Repository = repository;
        Logger = logger;
    }

    public virtual async Task<TReadDto> CreateAsync(TCreateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = Mapper.Map<TEntity>(dto);
            await Repository.AddAsync(entity, cancellationToken);
            Logger.LogInformation($"Created entity: {entity}");
            return Mapper.Map<TReadDto>(entity);
        }
        catch(Exception ex)
        {
            Logger.LogError($"Failed to create entity: {ex.Message}");
            throw;
        }
    }

    public virtual async Task<TReadDto?> UpdateAsync(int id ,TUpdateDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                Logger.LogInformation($"Entity with id: {id} not found");
                return default;
            }

            dto.Adapt(entity);
            var updatedEntity = Repository.UpdateAsync(entity, cancellationToken);
            Logger.LogInformation($"Updated entity: {updatedEntity}");
            return updatedEntity.Adapt<TReadDto>();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to update entity: {ex.Message}");
            throw;
        }
    }

    public virtual async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                Logger.LogInformation($"Entity with id: {id} not found");
                return;
            }

            await Repository.DeleteAsync(id, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to delete entity: {ex.Message}");
            throw;
        }
     
    }


    public virtual async Task<List<TReadDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var entities = await Repository.GetAllAsync(cancellationToken);
            return entities.Adapt<List<TReadDto>>();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to get all entities: {ex.Message}");
            throw;
        }

    }

    public virtual async Task<TReadDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var entity = await Repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                Logger.LogInformation($"Entity with id: {id} not found");
                return default;
            }

            return Mapper.Adapt<TReadDto>();
        }
        catch (Exception ex)
        {
            Logger.LogError($"Failed to get entity: {ex.Message}");
            throw;
        }
    }
}