using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Events
{
    public interface IEventHandler
    {
        Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default);

        bool CanHandle(IEvent @event);
    }

    public interface IEventHandler<in TEntity> : IEventHandler
        where TEntity : IEvent
    {
        Task<bool> HandleAsync(TEntity @event, CancellationToken cancellationToken = default);
    }
}