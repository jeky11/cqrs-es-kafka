using Cqrs.Core.Exceptions;
using Cqrs.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RemoveCommentController(ILogger<RemoveCommentController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
{
    private readonly ILogger<RemoveCommentController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    [HttpDelete]
    public async Task<IActionResult> RemoveCommentAsync(RemoveCommentCommand command)
    {
        try
        {
            await _commandDispatcher.SendAsync(command);

            return Ok(
                new BaseResponse
                {
                    Message = "Remove comment request completed successfully!",
                });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Client made a bad request!");
            return BadRequest(
                new BaseResponse
                {
                    Message = ex.Message,
                });
        }
        catch (AggregateNotFoundException ex)
        {
            _logger.LogWarning(ex, "Could not retrieve aggregate, incorrect Id!");
            return BadRequest(
                new BaseResponse
                {
                    Message = ex.Message,
                });
        }
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while processing request to remove a comment from a post!";
            _logger.LogError(ex, safeErrorMessage);
            return StatusCode(
                StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = safeErrorMessage,
                });
        }
    }
}