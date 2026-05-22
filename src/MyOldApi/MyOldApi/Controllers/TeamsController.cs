using Mapster;
using MyOldApi.Data.Repositories;
using MyOldApi.Models;

namespace MyOldApi.Controllers;

public static class TeamsController
{
  public static IEndpointRouteBuilder MapTeamsEndpoints(this IEndpointRouteBuilder app)
  {
    app.MapGet("/teams", GetAllTeams).WithName("GetAllTeams");
    app.MapGet("/teams/{id:int}", GetTeam).WithName("GetTeam");
    return app;
  }

  public static async Task<IResult> GetAllTeams(IWorldCupRepository repository)
  {
    var results = await repository.GetAllTeamsAsync();
    if (!results.Any())
    {
      return Results.NotFound();
    }

    var models = results.Adapt<List<TeamModel>>();
    return Results.Ok(models);
  }

  public static async Task<IResult> GetTeam(IWorldCupRepository repository, int id)
  {
    var result = await repository.GetTeamByIdAsync(id);
    if (result is null)
    {
      return Results.NotFound();
    }

    var model = result.Adapt<TeamModel>();
    return Results.Ok(model);
  }
}
