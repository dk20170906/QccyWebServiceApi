using System.Threading;
using System.Threading.Tasks;

namespace EdaSample.Common.Events
{
    public abstract class EventHandler<TEntity> : IEventHandler<TEntity>
        where TEntity : IEvent
    {
        public bool CanHandle(IEvent @event)
            => typeof(TEntity).Equals(@event.GetType());

        public abstract Task<bool> HandleAsync(TEntity @event, CancellationToken cancellationToken = default);

        public Task<bool> HandleAsync(IEvent @event, CancellationToken cancellationToken = default)
            => CanHandle(@event) ? HandleAsync((TEntity)@event, cancellationToken) : Task.FromResult(false);
    }
}