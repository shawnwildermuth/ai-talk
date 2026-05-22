using MyOldApi.Data.Entities;

namespace MyOldApi.Data.Repositories;

public interface IWorldCupRepository
{
  Task<List<Team>> GetAllTeamsAsync();
  Task<Team?> GetTeamByIdAsync(int id);
  Task<List<Player>> GetPlayersByTeamIdAsync(int teamId);
  Task<Player?> GetPlayerByTeamAndJerseyNumberAsync(int teamId, int jerseyNumber);
}
