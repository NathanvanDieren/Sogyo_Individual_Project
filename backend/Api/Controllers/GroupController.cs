using Microsoft.AspNetCore.Mvc;
using Domain.Interfaces;
namespace Api.Controllers;

[ApiController]
[Route("api/group/")]
public class GroupController
{
    private readonly IGroupFacade _groupFacade;       

    public GroupController(IGroupFacade groupFacade)
    {
        _groupFacade = groupFacade;
    }
    
    
}