using Cqrs.Core.Domain;

namespace Cqrs.Core.Handlers;

public interface IEventSourcingHandler<T>
{
    Task<T> GetByIdAsync(Guid id);
    Task SaveAsync(AggregateRoot aggregate);
    Task RepublishEventsAsync();
}