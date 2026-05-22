using Mapster;
using MyOldApi.Data.Repositories;
using MyOldApi.Models;

namespace MyOldApi.Controllers;

public static class PlayersController
{
  public static IEndpointRouteBuilder MapPlayersEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapGet("/teams/{teamId:int}/players", GetPlayers);
    app.MapGet("/teams/{teamId:int}/players/{id:int}", GetPlayer);
    return app;
  }

  public static async Task<IResult> GetPlayers(IWorldCupRepository repository, int teamId)
  {
    var results = await repository.GetPlayersByTeamIdAsync(teamId);
    if (!results.Any())
    {
      return Results.NotFound();
    }

    var models = results.Adapt<List<PlayerModel>>();
    return Results.Ok(models);
  }

  public static async Task<IResult> GetPlayer(IWorldCupRepository repository, int teamId, int id)
  {
    var result = await repository.GetPlayerByTeamAndJerseyNumberAsync(teamId, id);
    if (result is null)
    {
      return Results.NotFound();
    }

    var model = result.Adapt<PlayerModel>();
    return Results.Ok(model);
  }
}
