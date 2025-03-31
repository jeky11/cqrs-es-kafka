using Cqrs.Core.Domain;
using Cqrs.Core.Handlers;
using Cqrs.Core.Infrastructure;
using Cqrs.Core.Producers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;

public class EventSourcingHandler(IEventStore eventStore, IEventProducer eventProducer) : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore = eventStore;
    private readonly IEventProducer _eventProducer = eventProducer;

    public async Task<PostAggregate> GetByIdAsync(Guid id)
    {
        var aggregate = new PostAggregate();
        var events = await _eventStore.GetEventsAsync(id);
        if (events.Count == 0)
        {
            return aggregate;
        }

        aggregate.ReplayEvents(events);
        aggregate.Version = events.Max(e => e.Version);

        return aggregate;
    }

    public async Task SaveAsync(AggregateRoot aggregate)
    {
        await _eventStore.SaveEventsAsync(aggregate.Id, aggregate.GetUncommittedChanges(), aggregate.Version);
        aggregate.MarkChangesAsCommitted();
    }

    public async Task RepublishEventsAsync()
    {
        var aggregateIds = await _eventStore.GetAggregateIdsAsync();
        if (aggregateIds.Count == 0)
        {
            return;
        }

        var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
        if (topic == null)
        {
            throw new Exception("Kafka topic environment variable is not set!");
        }

        foreach (var aggregateId in aggregateIds)
        {
            var aggregate = await GetByIdAsync(aggregateId);
            if (!aggregate.Active)
            {
                continue;
            }

            var events = await _eventStore.GetEventsAsync(aggregateId);
            foreach (var @event in events)
            {
                await _eventProducer.ProduceAsync(topic, @event);
            }
        }
    }
}