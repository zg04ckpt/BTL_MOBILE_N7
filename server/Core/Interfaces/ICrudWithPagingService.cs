using Core.Models;

namespace Core.Interfaces
{
    public interface ICrudWithPagingService<TEntity, TCreateRequest, TUpdateRequest, TListItemDto, TDetailDto, TPagingRequest>
        : ICrudService<TEntity, TCreateRequest, TUpdateRequest, TListItemDto, TDetailDto>
        where TEntity : IEntity
        where TPagingRequest : PagingRequest
    {
        Task<Paginated<TListItemDto>> GetPagingAsync(TPagingRequest request);
    }
}
