using Core.Interfaces;
using Core.Models;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;

namespace Core.Base
{
    public abstract class CrudWithPagingService<TEntity, TCreateRequest, TUpdateRequest, TListItemDto, TDetailDto, TSearchRequest>
        : CrudService<TEntity, TCreateRequest, TUpdateRequest, TListItemDto, TDetailDto>, 
        ICrudWithPagingService<TEntity, TCreateRequest, TUpdateRequest, TListItemDto, TDetailDto, TSearchRequest>
        where TEntity : class, IEntity 
        where TSearchRequest : PagingRequest 
    {
        protected CrudWithPagingService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<Paginated<TListItemDto>> GetPagingAsync(TSearchRequest request)
        { 
            var entities = await _uow.Repository<TEntity>().GetAllAsync(
                predicate: GetPagingFilter(request),
                pageIndex: request.PageIndex,
                pageSize: request.PageSize,
                orderBy: GetOrderBy(request.OrderBy),
                asc: request.IsAsc);
            
            var totalItems = await _uow.Repository<TEntity>().CountAsync(
                predicate: GetPagingFilter(request));
            
            var items = entities.Select(e => MapFromEntityToListItem(e)).ToList();
            
            return new Paginated<TListItemDto>
            {
                Items = items,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalItems = totalItems
            };
        }

        protected abstract Expression<Func<TEntity, bool>> GetPagingFilter(TSearchRequest request);
        protected abstract Expression<Func<TEntity, object>> GetOrderBy(string? orderBy);
    }
}
