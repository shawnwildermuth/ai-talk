using Microsoft.AspNetCore.Mvc;
using MyOldApi.Data;
using Microsoft.EntityFrameworkCore;

namespace MyOldApi.Controllers;

[ApiController]
[Route("teams/{teamId}/[controller]")]
public class PlayersController(MlbContext context) : ControllerBase
{
  [HttpGet()]
  public async Task<IResult> GetPlayers(int teamId)
  {
    var results = await context.Players.Where(p => p.TeamId == teamId).ToListAsync();
    if (!results.Any())
    {
      return Results.NotFound();
    }

    return Results.Ok(results);

  }

  [HttpGet("{id:int}")]
  public async Task<IResult> GetPlayer(int teamId, int id)
  {
    var result = await context.Players.Where(p => p.TeamId == teamId && p.JerseyNumber == id).FirstOrDefaultAsync();
    if (result is null)
    {
      return Results.NotFound();
    }

    return Results.Ok(result);

  }
}
