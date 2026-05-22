namespace MyOldApi.Data.Entities;

public class Player
{
  public int Id { get; set; }

  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;

  // World Cup-specific profile fields
  public DateOnly DateOfBirth { get; set; }
  public string Nationality { get; set; } = string.Empty;
  public string FifaCountryCode { get; set; } = string.Empty; // e.g. "ARG", "FRA"

  public string Position { get; set; } = string.Empty;
  public int JerseyNumber { get; set; }
  public string PreferredFoot { get; set; } = string.Empty; // Left, Right, Both

  public int? HeightCm { get; set; }
  public int? WeightKg { get; set; }

  public int InternationalCaps { get; set; }
  public int InternationalGoals { get; set; }
  public bool IsCaptain { get; set; }

  public int TeamId { get; set; }
  public Team Team { get; set; } = null!;
}
