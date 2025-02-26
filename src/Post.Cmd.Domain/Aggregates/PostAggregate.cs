using Cqrs.Core.Domain;
using Post.Common.Events;

namespace Post.Cmd.Domain.Aggregates;

public class PostAggregate : AggregateRoot
{
    private Guid _id;
    private bool _active;
    private string _author = null!;
    private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

    public PostAggregate()
    { }

    public PostAggregate(Guid id, string author, string message)
    {
        RaiseEvent(new PostCreatedEvent(id, author, message, DateTime.UtcNow));
    }

    public void Apply(PostCreatedEvent @event)
    {
        _id = @event.Id;
        _active = true;
        _author = @event.Author;
    }

    public void EditMessage(string message)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit the message of an inactive post!");
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty!");
        }

        RaiseEvent(new MessageUpdatedEvent(_id, message));
    }

    public void Apply(MessageUpdatedEvent @event)
    {
        _id = @event.Id;
    }

    public void LikePost()
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot like an inactive post!");
        }

        RaiseEvent(new PostLikedEvent(_id));
    }

    public void Apply(PostLikedEvent @event)
    {
        _id = @event.Id;
    }

    public void AddComment(string comment, string userName)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot add a comment of an inactive post!");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty!");
        }

        RaiseEvent(new CommentAddedEvent(_id, Guid.NewGuid(), comment, userName, DateTime.UtcNow));
    }

    public void Apply(CommentAddedEvent @event)
    {
        _id = @event.Id;
        _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.UserName));
    }

    public void EditComment(Guid commentId, string comment, string userName)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot edit a comment of an inactive post!");
        }

        if (!_comments.TryGetValue(commentId, out var commentTuple))
        {
            throw new InvalidOperationException($"There is no comment with id {commentId}");
        }

        if (!string.Equals(commentTuple.Item2, userName, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user");
        }

        if (string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty!");
        }

        RaiseEvent(new CommentUpdatedEvent(_id, commentId, comment, userName, DateTime.UtcNow));
    }

    public void Apply(CommentUpdatedEvent @event)
    {
        _id = @event.Id;
        _comments[@event.CommentId] = new Tuple<string, string>(@event.Comment, @event.UserName);
    }

    public void DeleteComment(Guid commentId, string userName)
    {
        if (!_active)
        {
            throw new InvalidOperationException("You cannot remove a comment of an inactive post!");
        }

        if (!_comments.TryGetValue(commentId, out var commentTuple))
        {
            throw new InvalidOperationException($"There is no comment with id {commentId}");
        }

        if (!string.Equals(commentTuple.Item2, userName, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user");
        }

        RaiseEvent(new CommentRemovedEvent(_id, commentId));
    }

    public void Apply(CommentRemovedEvent @event)
    {
        _id = @event.Id;
        _comments.Remove(@event.CommentId);
    }

    public void DeletePost(string userName)
    {
        if (!_active)
        {
            throw new InvalidOperationException("The post has already been removed");
        }

        if (!string.Equals(_author, userName, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("You are not allowed to remove a post that was made by another user");
        }

        RaiseEvent(new PostRemovedEvent(_id));
    }

    public void Apply(PostRemovedEvent @event)
    {
        _id = @event.Id;
        _active = false;
    }
}