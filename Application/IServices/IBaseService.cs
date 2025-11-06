namespace Application.IServices;

public interface IBaseService<TEntity, TCreateDto, TUpdateDto, TReadDto>
    where TEntity : class
    where TCreateDto : class
    where TUpdateDto : class
{
    Task<TReadDto> CreateAsync(TCreateDto dto , CancellationToken cancellationToken = default) ;
    Task<TReadDto?> UpdateAsync(int id, TUpdateDto dto , CancellationToken cancellationToken = default);
    Task DeleteAsync(int id , CancellationToken cancellationToken = default);
    Task<List<TReadDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TReadDto?> GetByIdAsync(int id , CancellationToken cancellationToken = default);
}