using Cqrs.Core.Handlers;
using Post.Cmd.Domain.Aggregates;

namespace Post.Cmd.Api.Commands;

public class CommandHandler(IEventSourcingHandler<PostAggregate> eventSourcingHandler) : ICommandHandler
{
    private readonly IEventSourcingHandler<PostAggregate> _eventSourcingHandler = eventSourcingHandler;

    public async Task HandleAsync(NewPostCommand command)
    {
        var aggregate = new PostAggregate(command.Id, command.Author, command.Message);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditMessageCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.EditMessage(command.Message);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(LikePostCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.LikePost();
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(AddCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.AddComment(command.Comment, command.UserName);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(EditCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.EditComment(command.CommentId, command.Comment, command.UserName);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RemoveCommentCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.RemoveComment(command.CommentId, command.UserName);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(DeletePostCommand command)
    {
        var aggregate = await _eventSourcingHandler.GetByIdAsync(command.Id);
        aggregate.DeletePost(command.UserName);
        await _eventSourcingHandler.SaveAsync(aggregate);
    }

    public async Task HandleAsync(RestoreReadDbCommand command)
    {
        await _eventSourcingHandler.RepublishEventsAsync();
    }
}