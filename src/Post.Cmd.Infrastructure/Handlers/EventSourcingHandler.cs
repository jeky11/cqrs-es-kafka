using Cqrs.Core.Domain;
using Cqrs.Core.Handlers;
using Cqrs.Core.Infrastructure;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Infrastructure.Handlers;

public class EventSourcingHandler(IEventStore eventStore) : IEventSourcingHandler<PostAggregate>
{
    private readonly IEventStore _eventStore = eventStore;

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
}