using Microsoft.AspNetCore.Mvc;
using Domain;
using Domain.DTOs;
using Domain.Services;

namespace Api.Controllers;

[ApiController]
[Route("api/user/")]
public class UserController : ControllerBase
{
    private readonly IUserFacade _userFacade;       
    private readonly CurrentUserService _currentUserService;

    public UserController(IUserFacade userFacade, CurrentUserService currentUserService)
    {
        _userFacade = userFacade;
        _currentUserService = currentUserService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model)
    {   
        try 
        {
            UserResponseDto response = await _userFacade.CreateUser(model.Username, model.Email, model.Password);
        
            if (response == null)
            {
                return BadRequest(new { message = "Registratie mislukt. Probeer het opnieuw." });
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
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        LoginResponseDto? response = await _userFacade.UserLogin(model.Email, model.Password);
        
        if (response == null)
        {
            return Unauthorized(new { message = "Ongeldig e-mailadres of wachtwoord." });
        }
        
        Response.Cookies.Append("UserId", response.Id.ToString(), new CookieOptions
        {
            HttpOnly = true, 
            Expires = DateTime.UtcNow.AddDays(7)
        });
        
       
        return Ok(new { 
            message = "Succesvol ingelogd",
            Id = response.Id 
        });
    }

    [HttpGet("test")]
    public IActionResult GetMe()
    {
        if (!_currentUserService.IsAuthenticated)
        {
            return Unauthorized(new { message = "Geen actieve gebruiker ingeladen via cookie." });
        }

        return Ok(new
        {
            message = "Het werkt!",
            username = _currentUserService.User!.Username,
            email = _currentUserService.User!.Email
        });
    }
}
