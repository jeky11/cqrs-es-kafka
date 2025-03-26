using Cqrs.Core.Consumers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Post.Query.Infrastructure.Consumers;

public class ConsumerHostedService : IHostedService
{
    private readonly ILogger<ConsumerHostedService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly string _topic;

    public ConsumerHostedService(ILogger<ConsumerHostedService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC") ?? throw new InvalidOperationException("KAFKA_TOPIC is not configured");
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting consumer.");

        using (var scope = _scopeFactory.CreateScope())
        {
            var eventConsumer = scope.ServiceProvider.GetRequiredService<IEventConsumer>();

            Task.Run(() => eventConsumer.Consume(_topic), cancellationToken);
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Stopping consumer.");

        return Task.CompletedTask;
    }
}