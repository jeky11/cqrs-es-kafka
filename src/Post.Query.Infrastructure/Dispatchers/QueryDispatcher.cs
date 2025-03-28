using Cqrs.Core.Infrastructure;
using Cqrs.Core.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.Dispatchers;

public class QueryDispatcher : IQueryDispatcher<PostEntity>
{
    private readonly Dictionary<Type, Func<BaseQuery, Task<List<PostEntity>>>> _handlers = new();

    public void RegisterHandler<TQuery>(Func<TQuery, Task<List<PostEntity>>> handler) where TQuery : BaseQuery
    {
        if (_handlers.ContainsKey(typeof(TQuery)))
        {
            throw new IndexOutOfRangeException($"Handler for {typeof(TQuery).FullName} has already been registered.");
        }

        _handlers.Add(typeof(TQuery), x => handler((TQuery)x));
    }

    public async Task<List<PostEntity>> SendAsync(BaseQuery query)
    {
        if (!_handlers.TryGetValue(query.GetType(), out var handlers))
        {
            throw new ArgumentNullException(nameof(query), "Query is not registered.");
        }

        return await handlers(query);
    }
}