using Cqrs.Core.Domain;
using Cqrs.Core.Events;
using Cqrs.Core.Exceptions;
using Cqrs.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;

    public EventStore(IEventStoreRepository eventStoreRepository)
    {
        _eventStoreRepository = eventStoreRepository;
    }

    public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (eventStream == null || eventStream.Count == 0)
        {
            throw new AggregateNotFoundException("Incorrect post ID provided!");
        }

        return eventStream.OrderBy(x => x.Version).Select(x => x.EventData).ToList();
    }

    public async Task SaveEventsAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
    {
        var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
        if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)
        {
            throw new ConcurrencyException();
        }

        var version = expectedVersion;

        foreach (var @event in events)
        {
            version--;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel(
                aggregateId, nameof(PostAggregate), version, eventType, @event,
                DateTime.UtcNow);

            await _eventStoreRepository.SaveAsync(eventModel);
        }
    }
}