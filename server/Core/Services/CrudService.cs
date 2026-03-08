using Core.Exceptions;
using Core.Interfaces;
using Core.Models;

namespace Core.Base
{
    public abstract class CrudService<TEntity, TCreateRequest, TUpdateRequest, TListItemDto, TDetailDto>
        : BaseService, ICrudService<TEntity, TCreateRequest, TUpdateRequest, TListItemDto, TDetailDto>
        where TEntity : class, IEntity
    {
        protected CrudService(IUnitOfWork uow) : base(uow)
        {
        }

        public async Task<ChangedResponse> CreateAsync(TCreateRequest request)
        {
            await ConfirmValidCreateDataAsync(request);
            var entity = await MapFromCreateToEntityAsync(request);
            await _uow.Repository<TEntity>().AddAsync(entity);
            await _uow.SaveChangesAsync();
            return new ChangedResponse
            {
                Id = entity.Id,
            };
        }

        public async Task<ChangedResponse> DeleteAsync(int id)
        {
            var entity = await _uow.Repository<TEntity>()
                .GetFirstAsync(e => e.Id == id) 
                ?? throw new NotFoundException("Entity to delete does not exist");
            await _uow.Repository<TEntity>().DeleteAsync(entity);
            await _uow.SaveChangesAsync();
            return new ChangedResponse
            {
                Id = entity.Id,
            };
        }

        public async Task<IEnumerable<TListItemDto>> GetAllAsync()
        {
            var entities = await _uow.Repository<TEntity>().GetAllAsync(
                predicate: e => true);
            return entities.Select(e => MapFromEntityToListItem(e));
        }

        public async Task<TDetailDto?> GetByIdAsync(int id)
        {
            var entity = await _uow.Repository<TEntity>().GetFirstAsync(
                predicate: e => e.Id == id);
            return entity != null ? MapFromEntityToDetail(entity) : default;
        }

        public async Task<ChangedResponse> UpdateAsync(int id, TUpdateRequest request)
        {
            var entity = await _uow.Repository<TEntity>()
                .GetFirstAsync(e => e.Id == id)
                ?? throw new NotFoundException("Entity to update does not exist");
            await ConfirmValidUpdateDataAsync(entity, request);
            await UpdateEntityAsync(entity, request);
            await _uow.Repository<TEntity>().UpdateAsync(entity);
            await _uow.SaveChangesAsync();
            return new ChangedResponse
            {
                Id = entity.Id,
            };
        }

        protected abstract Task ConfirmValidCreateDataAsync(TCreateRequest request);
        protected abstract Task ConfirmValidUpdateDataAsync(TEntity entity, TUpdateRequest request);
        protected abstract Task<TEntity> MapFromCreateToEntityAsync(TCreateRequest request);
        protected abstract TListItemDto MapFromEntityToListItem(TEntity entity);
        protected abstract TDetailDto MapFromEntityToDetail(TEntity entity);
        protected abstract Task UpdateEntityAsync(TEntity entity, TUpdateRequest request);
    }
}
