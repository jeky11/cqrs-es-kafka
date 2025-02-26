using Cqrs.Core.Events;

namespace Cqrs.Core.Infrastructure;

public interface IEventStore
{
    Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId);
    Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);
}