using Cqrs.Core.Commands;

namespace Post.Cmd.Api.Commands;

public record RestoreReadDbCommand(Guid Id) : BaseCommand(Id);