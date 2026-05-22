using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using MyOldApi.Models;
using Xunit;

namespace MyOldApi.Tests.Endpoints;

public class ApiEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
  private readonly HttpClient _client;

  public ApiEndpointsTests(CustomWebApplicationFactory factory)
  {
    _client = factory.CreateClient();
  }

  [Fact]
  public async Task GetTeams_ReturnsOkWithTeams()
  {
    var response = await _client.GetAsync("/teams");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var teams = await response.Content.ReadFromJsonAsync<List<TeamModel>>();
    Assert.NotNull(teams);
    Assert.NotEmpty(teams);
  }

  [Fact]
  public async Task GetTeamById_ReturnsOkWithTeam()
  {
    var response = await _client.GetAsync("/teams/1");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var team = await response.Content.ReadFromJsonAsync<TeamModel>();
    Assert.NotNull(team);
    Assert.Equal("Netherlands", team.Name);
  }

  [Fact]
  public async Task GetPlayersByTeam_ReturnsOkWithPlayers()
  {
    var response = await _client.GetAsync("/teams/1/players");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var players = await response.Content.ReadFromJsonAsync<List<PlayerModel>>();
    Assert.NotNull(players);
    Assert.NotEmpty(players);
    Assert.All(players, p => Assert.Equal(1, p.TeamId));
  }

  [Fact]
  public async Task GetPlayerByTeamAndJerseyNumber_ReturnsOkWithPlayer()
  {
    var response = await _client.GetAsync("/teams/1/players/1");

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    var player = await response.Content.ReadFromJsonAsync<PlayerModel>();
    Assert.NotNull(player);
    Assert.Equal(1, player.TeamId);
    Assert.Equal(1, player.JerseyNumber);
  }

  [Fact]
  public async Task GetMissingTeam_ReturnsNotFound()
  {
    var response = await _client.GetAsync("/teams/999");

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}
