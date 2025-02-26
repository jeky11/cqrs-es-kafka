using Cqrs.Core.Events;

namespace Cqrs.Core.Domain;

public abstract class AggregateRoot
{
    public Guid Id { get; protected set; }

    public int Version { get; set; } = -1;

    private readonly List<BaseEvent> _changes = new();

    public IEnumerable<BaseEvent> GetUncommittedChanges() => _changes;

    public void MarkChangesAsCommitted() => _changes.Clear();

    private void ApplyChange(BaseEvent @event, bool isNew)
    {
        var method = GetType().GetMethod("Apply", new[] {@event.GetType()});
        if (method == null)
        {
            throw new ArgumentNullException(nameof(method), "The apply method could not be found.");
        }

        method.Invoke(this, new object[] {@event});

        if (isNew)
        {
            _changes.Add(@event);
        }
    }

    protected void RaiseEvent(BaseEvent @event)
    {
        ApplyChange(@event, true);
    }

    public void ReplayEvents(IEnumerable<BaseEvent> events)
    {
        foreach (var @event in events)
        {
            ApplyChange(@event, false);
        }
    }
}