namespace Post.Cmd.Infrastructure.Config;

public record MongoDbConfig
{
    public string? ConnectionString { get; init; }
    public string? Database { get; init; }
    public string? Collection { get; init; }
}