using System.ComponentModel.DataAnnotations;

namespace MyOldApi.Data.Entities;

public class Team
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Country { get; set; } = string.Empty;
  [RegularExpression("^[A-Z]{3}$")]
  public string FifaCountryCode { get; set; } = string.Empty;
  public string Confederation { get; set; } = string.Empty;
  public string Group { get; set; } = string.Empty;
  public string HeadCoach { get; set; } = string.Empty;
  public int FifaRanking { get; set; }
  public ICollection<Player> Players { get; set; } = [];
}
