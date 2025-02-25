using Cqrs.Core.Commands;
using Cqrs.Core.Infrastructure;

namespace Post.Cmd.Infrastructure.Dispatchers;

public class CommandDispatcher : ICommandDispatcher
{
    private readonly Dictionary<Type, Func<BaseCommand, Task>> _handlers = new();

    public void RegisterHandler<T>(Func<T, Task> handler) where T : BaseCommand
    {
        if (_handlers.ContainsKey(typeof(T)))
        {
            throw new IndexOutOfRangeException($"Handler for type {typeof(T)} already registered.");
        }

        _handlers.Add(typeof(T), x => handler((T)x));
    }

    public async Task SendAsync(BaseCommand command)
    {
        if (!_handlers.TryGetValue(command.GetType(), out var handler))
        {
            throw new ArgumentNullException($"Handler for type {command.GetType()} not registered.");
        }

        await handler(command);
    }
}