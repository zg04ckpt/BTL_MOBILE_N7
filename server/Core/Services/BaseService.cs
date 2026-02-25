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
    }
}
