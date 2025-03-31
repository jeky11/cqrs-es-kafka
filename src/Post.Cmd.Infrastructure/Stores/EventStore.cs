using Cqrs.Core.Domain;
using Cqrs.Core.Events;
using Cqrs.Core.Exceptions;
using Cqrs.Core.Infrastructure;
using Cqrs.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Stores;

public class EventStore : IEventStore
{
    private readonly IEventStoreRepository _eventStoreRepository;
    private readonly IEventProducer _eventProducer;

    public EventStore(IEventStoreRepository eventStoreRepository, IEventProducer eventProducer)
    {
        _eventStoreRepository = eventStoreRepository;
        _eventProducer = eventProducer;
    }

    public async Task<List<Guid>> GetAggregateIdsAsync()
    {
        var eventStream = await _eventStoreRepository.FindAllAsync();
        if (eventStream == null || eventStream.Count == 0)
        {
            throw new ArgumentNullException(nameof(eventStream), "No events found");
        }

        return eventStream.Select(x => x.AggregateIdentifier).Distinct().ToList();
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

        var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
        if (topic == null)
        {
            throw new Exception("Kafka topic environment variable is not set!");
        }

        foreach (var @event in events)
        {
            version++;
            @event.Version = version;
            var eventType = @event.GetType().Name;
            var eventModel = new EventModel
            {
                AggregateIdentifier = aggregateId,
                AggregateType = nameof(PostAggregate),
                Version = version,
                EventType = eventType,
                EventData = @event,
                TimeStamp = DateTime.UtcNow
            };

            await _eventStoreRepository.SaveAsync(eventModel);
            await _eventProducer.ProduceAsync(topic, @event);
        }
    }
}