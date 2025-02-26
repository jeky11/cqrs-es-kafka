namespace Post.Cmd.Infrastructure.Config;

public record MongoDbConfig(string ConnectionString, string Database, string Collection);