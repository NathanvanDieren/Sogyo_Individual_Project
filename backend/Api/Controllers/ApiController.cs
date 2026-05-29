using Microsoft.AspNetCore.Mvc;
using Persistence;
using Domain;
using Domain.DTOs;
using BC = BCrypt.Net.BCrypt;
namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserFacade _userFacade;       

    public UsersController(IUserFacade userFacade)
    {
        _userFacade = userFacade;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model)
    {   
        await _userFacade.CreateUser(model.Username, model.Email, model.Password);
        return Ok();
    }
}