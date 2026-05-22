using Microsoft.EntityFrameworkCore;
using MyOldApi.Data;
using MyOldApi.Data.Entities;
using MyOldApi.Data.Repositories;
using Xunit;

namespace MyOldApi.Tests.Data.Repositories;

public class WorldCupRepositoryTests
{
  [Fact]
  public async Task GetAllTeamsAsync_ReturnsSeededTeams()
  {
    await using var context = CreateContext();
    var repository = new WorldCupRepository(context);

    var result = await repository.GetAllTeamsAsync();

    Assert.Equal(2, result.Count);
  }

  [Fact]
  public async Task GetTeamByIdAsync_ReturnsExpectedTeam()
  {
    await using var context = CreateContext();
    var repository = new WorldCupRepository(context);

    var result = await repository.GetTeamByIdAsync(1);

    Assert.NotNull(result);
    Assert.Equal("Netherlands", result.Name);
  }

  [Fact]
  public async Task GetPlayersByTeamIdAsync_ReturnsTeamPlayers()
  {
    await using var context = CreateContext();
    var repository = new WorldCupRepository(context);

    var result = await repository.GetPlayersByTeamIdAsync(1);

    Assert.Equal(2, result.Count);
    Assert.All(result, p => Assert.Equal(1, p.TeamId));
  }

  [Fact]
  public async Task GetPlayerByTeamAndJerseyNumberAsync_ReturnsMatchingPlayer()
  {
    await using var context = CreateContext();
    var repository = new WorldCupRepository(context);

    var result = await repository.GetPlayerByTeamAndJerseyNumberAsync(1, 9);

    Assert.NotNull(result);
    Assert.Equal("Vivianne", result.FirstName);
  }

  private static WorldCupContext CreateContext()
  {
    var options = new DbContextOptionsBuilder<WorldCupContext>()
      .UseInMemoryDatabase(Guid.NewGuid().ToString())
      .Options;

    var context = new WorldCupContext(options);

    context.Teams.AddRange(
      new Team
      {
        Id = 1,
        Name = "Netherlands",
        Country = "Netherlands",
        FifaCountryCode = "NED",
        Confederation = "UEFA",
        Group = "E",
        HeadCoach = "Andries Jonker",
        FifaRanking = 7
      },
      new Team
      {
        Id = 2,
        Name = "Sweden",
        Country = "Sweden",
        FifaCountryCode = "SWE",
        Confederation = "UEFA",
        Group = "F",
        HeadCoach = "Peter Gerhardsson",
        FifaRanking = 6
      });

    context.Players.AddRange(
      new Player
      {
        Id = 1,
        FirstName = "Vivianne",
        LastName = "Miedema",
        DateOfBirth = new DateOnly(1996, 7, 15),
        Nationality = "Dutch",
        FifaCountryCode = "NED",
        Position = "Forward",
        JerseyNumber = 9,
        PreferredFoot = "Right",
        HeightCm = 175,
        WeightKg = 67,
        InternationalCaps = 120,
        InternationalGoals = 98,
        IsCaptain = false,
        TeamId = 1
      },
      new Player
      {
        Id = 2,
        FirstName = "Lieke",
        LastName = "Martens",
        DateOfBirth = new DateOnly(1992, 12, 16),
        Nationality = "Dutch",
        FifaCountryCode = "NED",
        Position = "Forward",
        JerseyNumber = 11,
        PreferredFoot = "Right",
        HeightCm = 170,
        WeightKg = 62,
        InternationalCaps = 160,
        InternationalGoals = 60,
        IsCaptain = false,
        TeamId = 1
      },
      new Player
      {
        Id = 3,
        FirstName = "Stina",
        LastName = "Blackstenius",
        DateOfBirth = new DateOnly(1996, 2, 5),
        Nationality = "Swedish",
        FifaCountryCode = "SWE",
        Position = "Forward",
        JerseyNumber = 15,
        PreferredFoot = "Right",
        HeightCm = 174,
        WeightKg = 66,
        InternationalCaps = 100,
        InternationalGoals = 35,
        IsCaptain = false,
        TeamId = 2
      });

    context.SaveChanges();
    return context;
  }
}
