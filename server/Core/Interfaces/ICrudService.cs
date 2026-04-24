using Core.Models;

namespace Core.Interfaces
{
    public interface ICrudService<TEntity, TCreateRequest, TUpdateRequest,TListItemDto, TDetailDto>
        where TEntity : IEntity
    {
        Task<IEnumerable<TListItemDto>> GetAllAsync();
        Task<TDetailDto?> GetByIdAsync(int id);
        Task<ChangedResponse> CreateAsync(TCreateRequest request);
        Task<ChangedResponse> UpdateAsync(int id, TUpdateRequest dto);
        Task<ChangedResponse> DeleteAsync(int id);
    }
}
