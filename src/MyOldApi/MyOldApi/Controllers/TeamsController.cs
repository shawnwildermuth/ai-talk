using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyOldApi.Data;
namespace MyOldApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TeamsController(MlbContext context) : ControllerBase
{
  [HttpGet(Name = "GetAllTeams")]
  public async Task<IResult> Get()
  {
    var results = await context.Teams.ToListAsync();
    if (!results.Any())
    {
      return Results.NotFound();
    }

    return Results.Ok(results);
  }

  [HttpGet("{id:int}", Name = "GetTeam")]
  public async Task<IResult> Get(int id)
  {
    var result = await context.Teams.FindAsync(id);
    if (result is null)
    {
      return Results.NotFound();
    }

    return Results.Ok(result);
  }
}
