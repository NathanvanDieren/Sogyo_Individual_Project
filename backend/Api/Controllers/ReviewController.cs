using Microsoft.AspNetCore.Mvc;
using Application.DTOs;
using Application.Interfaces;
using Domain.Classes;

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
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto model)
    {   
        try
        {
            ReviewResponseDto response = await _reviewFacade.CreateReview(model);
            if (response == null)
            {
                return BadRequest(new { message = "Review aanmaken is mislukt. Probeer het opnieuw." });
            }
        
            return Ok(response);
        }
        catch (BadHttpRequestException ex)
        {

            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "Er is een interne serverfout opgetreden.",
                error = ex.Message,             // Wat is er precies kapot? (bijv. NullReferenceException)
                detail = ex.StackTrace,         // Op welke regel in welke file ging het mis?
                innerError = ex.InnerException?.Message // Soms zit de échte fout hierin (bijv. bij Database fouten)
            });
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
    
    [HttpGet("getreviews")]
    public async Task<IActionResult> GetReviews()
    {
        try
        {
            ReviewListDto response = await _reviewFacade.GetReviews();
        
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