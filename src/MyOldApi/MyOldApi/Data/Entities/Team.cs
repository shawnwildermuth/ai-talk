namespace MyOldApi.Data.Entities;

public class Team
{
  public int Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string City { get; set; } = string.Empty;
  public string League { get; set; } = string.Empty;
  public string Division { get; set; } = string.Empty;
  public ICollection<Player> Players { get; set; } = [];
}
