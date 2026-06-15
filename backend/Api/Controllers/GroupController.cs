using Application.DTOs;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/group/")]
public class GroupController : ControllerBase
{
    private readonly IGroupFacade _groupFacade;

    public GroupController(IGroupFacade groupFacade)
    {
        _groupFacade = groupFacade;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateGroupDto model)
    {
        try
        {
            var response = await _groupFacade.CreateGroup(model);

            if (response == null)
            {
                return BadRequest(new { message = "Groep registratie mislukt. Probeer het opnieuw." });
            }

            return Ok(response);
        }
        catch (BadHttpRequestException ex)
        {

            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Er is een interne serverfout opgetreden." });
        }
    }

    [HttpPost("edit/{groupId}")]
    public async Task<IActionResult> CreateReview([FromRoute] Guid groupId, [FromBody] CreateGroupDto model)
    {
        try
        {
            var response = await _groupFacade.EditGroup(groupId, model);

            if (response == null)
            {
                return BadRequest(new { message = "Review aanpassen is mislukt. Probeer het opnieuw." });
            }

            return Ok(response);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new
            {
                message = "Er is een interne serverfout opgetreden.",
            });
        }
    }

    [HttpGet("getgroups")]
    public async Task<IActionResult> GetGroupsByUserId()
    {
        try
        {
            var response = await _groupFacade.GetGroups();

            if (response == null)
            {
                return BadRequest(new { message = "Groep registratie mislukt. Probeer het opnieuw." });
            }

            return Ok(response);
        }
        catch (BadHttpRequestException ex)
        {

            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Er is een interne serverfout opgetreden." });
        }
    }


    [HttpDelete("delete/{groupId}")]
    public async Task<IActionResult> DeleteGroupByGroupId(Guid groupId)
    {
        try
        {
            await _groupFacade.DeleteGroupByGroupId(groupId);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (UnauthorizedAccessException ex)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Er is een onverwachte fout opgetreden." });
        }
    }
}
