using Microsoft.AspNetCore.Mvc;
using Persistence;
using Domain;
using Domain.DTOs;
using BC = BCrypt.Net.BCrypt;
namespace Api.Controllers;

[ApiController]
[Route("api/")]
public class MainController : ControllerBase
{
    private readonly IUserFacade _userFacade;       

    public MainController(IUserFacade userFacade)
    {
        _userFacade = userFacade;
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
        string token = response.Token;
        
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,               
            Secure = true,
            SameSite = SameSiteMode.Lax, 
            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
        };
        
        Response.Cookies.Append("X-Access-Token", token, cookieOptions);
        
        return Ok(new { 
            message = "Succesvol ingelogd",
            Id = response.Id 
        });
    }
}
