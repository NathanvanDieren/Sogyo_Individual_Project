using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain;

namespace Api.Controllers;

[ApiController]
[Route("api/group/")]
public class GroupController: ControllerBase
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
            GroupResponseDto response = await _groupFacade.CreateGroup(model.Name, model.Emails);
        
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
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Er is een interne serverfout opgetreden." });
        }
    }
    
    [HttpGet("getgroups")]
    public async Task<IActionResult> GetGroupsByUserId()
    {   
        try
        {
            GroupListDto response = await _groupFacade.GetGroups();
        
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
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Er is een interne serverfout opgetreden." });
        }
    }
    
    
}