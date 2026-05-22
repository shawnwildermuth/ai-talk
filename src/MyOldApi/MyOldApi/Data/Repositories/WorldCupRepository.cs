using Microsoft.EntityFrameworkCore;
using MyOldApi.Data.Entities;

namespace MyOldApi.Data.Repositories;

public class WorldCupRepository(WorldCupContext context) : IWorldCupRepository
{
  public Task<List<Team>> GetAllTeamsAsync()
  {
    return context.Teams.ToListAsync();
  }

  public Task<Team?> GetTeamByIdAsync(int id)
  {
    return context.Teams.FindAsync(id).AsTask();
  }

  public Task<List<Player>> GetPlayersByTeamIdAsync(int teamId)
  {
    return context.Players.Where(p => p.TeamId == teamId).ToListAsync();
  }

  public Task<Player?> GetPlayerByTeamAndJerseyNumberAsync(int teamId, int jerseyNumber)
  {
    return context.Players.Where(p => p.TeamId == teamId && p.JerseyNumber == jerseyNumber).FirstOrDefaultAsync();
  }
}
