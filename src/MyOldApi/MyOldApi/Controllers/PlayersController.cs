using Microsoft.EntityFrameworkCore;
using MyOldApi.Data;

namespace MyOldApi.Controllers;

public static class PlayersController
{
  public static IEndpointRouteBuilder MapPlayersEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapGet("/teams/{teamId:int}/players", GetPlayers);
    app.MapGet("/teams/{teamId:int}/players/{id:int}", GetPlayer);
    return app;
  }

  public static async Task<IResult> GetPlayers(WorldCupContext context, int teamId)
  {
    var results = await context.Players.Where(p => p.TeamId == teamId).ToListAsync();
    if (!results.Any())
    {
      return Results.NotFound();
    }

    return Results.Ok(results);
  }

  public static async Task<IResult> GetPlayer(WorldCupContext context, int teamId, int id)
  {
    var result = await context.Players.Where(p => p.TeamId == teamId && p.JerseyNumber == id).FirstOrDefaultAsync();
    if (result is null)
    {
      return Results.NotFound();
    }

    return Results.Ok(result);
  }
}
