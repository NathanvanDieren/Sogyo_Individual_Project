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
            });
        }
    }
    
    [HttpPost("edit/{reviewId}")]
    public async Task<IActionResult> CreateReview([FromRoute] Guid reviewId, [FromBody] CreateReviewDto model)
    {   
        try
        {
            ReviewResponseDto response = await _reviewFacade.EditReview(reviewId, model);
        
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
        catch (Exception ex)
        {
            return StatusCode(500, new 
            { 
                message = "Er is een interne serverfout opgetreden.",
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
                return BadRequest(new { message = "Reviews laden mislukt. Probeer het opnieuw." });
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
    
    [HttpGet("getreviews/{groupId}")]
    public async Task<IActionResult> GetReviewsByGroup([FromRoute] Guid groupId)
    {
        try
        {
            ReviewListDto response = await _reviewFacade.GetReviewsByGroupId(groupId);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Fout bij ophalen van groepsreviews." });
        }
    }
    
    [HttpDelete("delete/{reviewId}")]
    public async Task<IActionResult> DeleteReviewByReviewId(Guid reviewId)
    {
        try
        {
            await _reviewFacade.DeleteReviewByReviewId(reviewId);
            
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
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Er is een onverwachte fout opgetreden." });
        }
    }
    
}