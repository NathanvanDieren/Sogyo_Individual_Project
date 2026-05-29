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
        UserResponseDto response = await _userFacade.CreateUser(model.Username, model.Email, model.Password);
        return CreatedAtAction(nameof(Login), new { id = response.Id }, response);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        LoginResponseDto? response = await _userFacade.UserLogin(model.Email, model.Password);
        
        if (response == null)
        {
            return Unauthorized(new { message = "Ongeldig e-mailadres of wachtwoord." });
        }
        
        return Ok(response);
    }
}
