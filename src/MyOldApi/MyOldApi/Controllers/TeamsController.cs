using Mapster;
using Microsoft.EntityFrameworkCore;
using MyOldApi.Data;
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

  public static async Task<IResult> GetAllTeams(WorldCupContext context)
  {
    var results = await context.Teams.ToListAsync();
    if (!results.Any())
    {
      return Results.NotFound();
    }

    var models = results.Adapt<List<TeamModel>>();
    return Results.Ok(models);
  }

  public static async Task<IResult> GetTeam(WorldCupContext context, int id)
  {
    var result = await context.Teams.FindAsync(id);
    if (result is null)
    {
      return Results.NotFound();
    }

    var model = result.Adapt<TeamModel>();
    return Results.Ok(model);
  }
}
