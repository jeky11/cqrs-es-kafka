using Cqrs.Core.Events;

namespace Cqrs.Core.Domain;

public interface IEventStoreRepository
{
    Task SaveAsync(EventModel @event);

    Task<List<EventModel>> FindByAggregateId(Guid aggregateId);
}