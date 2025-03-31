using Cqrs.Core.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Post.Cmd.Api.Commands;
using Post.Common.DTOs;

namespace Post.Cmd.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RestoreReadDbController(ILogger<RestoreReadDbController> logger, ICommandDispatcher commandDispatcher) : ControllerBase
{
    private readonly ILogger<RestoreReadDbController> _logger = logger;
    private readonly ICommandDispatcher _commandDispatcher = commandDispatcher;

    [HttpPost]
    public async Task<IActionResult> RestoreReadDbAsync()
    {
        try
        {
            await _commandDispatcher.SendAsync(new RestoreReadDbCommand(Guid.Empty));

            return StatusCode(
                StatusCodes.Status201Created, new BaseResponse
                {
                    Message = "Read database restore request completed",
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
        catch (Exception ex)
        {
            const string safeErrorMessage = "Error while processing request to restore read database!";
            _logger.LogError(ex, safeErrorMessage);
            return StatusCode(
                StatusCodes.Status500InternalServerError, new BaseResponse
                {
                    Message = safeErrorMessage,
                });
        }
    }
}