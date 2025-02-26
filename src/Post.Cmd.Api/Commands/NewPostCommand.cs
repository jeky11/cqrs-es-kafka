using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record NewPostCommand(Guid Id, string Author, string Message) : BaseCommand(Id)
{ }