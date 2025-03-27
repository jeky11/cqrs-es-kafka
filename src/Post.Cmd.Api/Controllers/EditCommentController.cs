using Cqrs.Core.Exceptions;
using Cqrs.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class EditCommentController(ILogger<EditCommentController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
{
    private readonly ILogger<EditCommentController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    [HttpPut]
    public async Task<IActionResult> EditCommentAsync(EditCommentCommand command)
    {
        try
        {
            await _commandDispatcher.SendAsync(command);

            return Ok(
                new BaseResponse
                {
                    Message = "Edit comment request completed successfully!",
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
            const string safeErrorMessage = "Error while processing request to edit a comment to a post!";
            _logger.LogError(ex, safeErrorMessage);
            return StatusCode(
                StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = safeErrorMessage,
                });
        }
    }
}