using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;

namespace Api.Controllers;

[ApiController]
[Route("api/review/")]
public class ReviewController: ControllerBase
{
    private readonly IReviewFacade _reviewFacade;       

    public ReviewController(IReviewFacade groupFacade)
    {
        _reviewFacade = groupFacade;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser([FromBody] CreateReviewDto model)
    {   
        try
        {
            ReviewResponseDto response = await _reviewFacade.CreateReview(model);
        
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

    [HttpGet("getitemtypes")]
    public async Task<IActionResult> GetItemTypes()
    {
        try
        {
            var itemTypes = await _reviewFacade.GetItemTypes();

            if (itemTypes == null)
            {
                return BadRequest(new { message = "Categorieen laden mislukt. Probeer het opnieuw." });
            }

            return Ok(itemTypes);
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