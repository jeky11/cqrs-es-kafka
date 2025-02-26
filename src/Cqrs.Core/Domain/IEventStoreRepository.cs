using Cqrs.Core.Events;

namespace Cqrs.Core.Domain;

public interface IEventStoreRepository
{
    Task<List<EventModel>> FindByAggregateId(Guid aggregateId);
    Task SaveAsync(EventModel @event);
}