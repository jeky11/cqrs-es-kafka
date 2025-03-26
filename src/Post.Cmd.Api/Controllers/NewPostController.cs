using Cqrs.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Cmd.Api.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class NewPostController(ILogger<NewPostController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
{
    private readonly ILogger<NewPostController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    [HttpPost]
    public async Task<IActionResult> NewPostAsync(NewPostCommand command)
    {
        var id = Guid.NewGuid();
        try
        {
            command = command with {Id = id};

            await _commandDispatcher.SendAsync(command);

            return StatusCode(
                StatusCodes.Status201Created, new NewPostResponse
                {
                    Message = "New Post Created",
                });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Client made a bad request!");
            return BadRequest(
                new NewPostResponse
                {
                    Message = ex.Message,
                });
        }
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while processing request to create a new post!";
            _logger.LogError(ex, safeErrorMessage);
            return StatusCode(
                StatusCodes.Status500InternalServerError, new NewPostResponse
                {
                    Id = id,
                    Message = safeErrorMessage,
                });
        }
    }
}