using Microsoft.AspNetCore.Mvc;
using Persistence;
using Domain;
using Api.DTOs;
using BC = BCrypt.Net.BCrypt;
namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto model)
    {   
        string hashedPassword = BC.HashPassword(model.Password);
        var newUser = new User(model.UserName, model.Email, hashedPassword, "user");
        

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        var response = new UserResponseDto(
            newUser.Id,
            newUser.Name, 
            newUser.Email, 
            newUser.RoleId
        );

        return CreatedAtAction(nameof(CreateUser), new { id = response.Id }, response);
    }
}