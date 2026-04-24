using Core.Exceptions;
using Core.Interfaces;

namespace Core.Base
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _uow;

        protected BaseService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        protected async Task<TEntity> GetEntityAsync<TEntity>(int id) where TEntity : class, IEntity
        {
            var e = await _uow.Repository<TEntity>().GetFirstAsync(
                predicate: e => e.Id == id)
                ?? throw new NotFoundException($"{typeof(TEntity).Name} not found");
            return e;
        }
    }
}
