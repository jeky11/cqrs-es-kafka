using Cqrs.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Common.DTOs;
using Post.Query.Api.DTOs;
using Post.Query.Api.Queries;
using Post.Query.Domain.Entities;

namespace Post.Query.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PostLookupController(ILogger<PostLookupController> logger, IQueryDispatcher<PostEntity> queryDispatcher)
    : ControllerBase
{
    private readonly ILogger<PostLookupController> _logger = logger;
    private readonly IQueryDispatcher<PostEntity> _queryDispatcher = queryDispatcher;

    [HttpGet]
    public async Task<IActionResult> GetAllPostsAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindAllPostsQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    [HttpGet("byId/{id}")]
    public async Task<IActionResult> GetPostByIdAsync(Guid id)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostByIdQuery {Id = id});
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    [HttpGet("byAuthor/{author}")]
    public async Task<IActionResult> GetPostsByAuthorAsync(string author)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsByAuthorQuery {Author = author});
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    [HttpGet("withComments")]
    public async Task<IActionResult> GetPostsWithCommentsAsync()
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostWithCommentsQuery());
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    [HttpGet("withLikes/{numberOfLikes}")]
    public async Task<IActionResult> GetPostsWithLikesAsync(int numberOfLikes)
    {
        try
        {
            var posts = await _queryDispatcher.SendAsync(new FindPostsWithLikesQuery {NumberOfLikes = numberOfLikes});
            return NormalResponse(posts);
        }
        catch (Exception ex)
        {
            return ErrorResponse(ex);
        }
    }

    private IActionResult NormalResponse(List<PostEntity> posts)
    {
        if (posts.Count == 0)
        {
            return NoContent();
        }

        return Ok(
            new PostLookupResponse
            {
                Posts = posts,
                Message = $"Found {posts.Count} posts"
            });
    }

    private IActionResult ErrorResponse(Exception ex)
    {
        const string safeErrorMessage = "An error occured while fetching posts";
        _logger.LogError(ex, safeErrorMessage);
        return StatusCode(
            500, new BaseResponse
            {
                Message = safeErrorMessage
            });
    }
}