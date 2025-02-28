using Cqrs.Core.Events;

namespace Cqrs.Core.Producers;

public interface IEventProducer
{
    Task ProduceAsync<T>(string message, T @event) where T : BaseEvent;
}