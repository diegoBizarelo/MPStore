using MPStore.Core.DomainObjects;
using NetDevPack.Data;

namespace MPStore.Core.Data
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
