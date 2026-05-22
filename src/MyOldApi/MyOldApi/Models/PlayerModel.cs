using System.ComponentModel.DataAnnotations;

namespace MyOldApi.Models;

public class PlayerModel
{
  [Required]
  [StringLength(100, MinimumLength = 1)]
  public string FirstName { get; set; } = string.Empty;

  [Required]
  [StringLength(100, MinimumLength = 1)]
  public string LastName { get; set; } = string.Empty;

  [Required]
  public DateOnly DateOfBirth { get; set; }

  [Required]
  [StringLength(100, MinimumLength = 2)]
  public string Nationality { get; set; } = string.Empty;

  [Required]
  [RegularExpression("^[A-Z]{3}$")]
  public string FifaCountryCode { get; set; } = string.Empty;

  [Required]
  [StringLength(50, MinimumLength = 2)]
  public string Position { get; set; } = string.Empty;

  [Range(1, 99)]
  public int JerseyNumber { get; set; }

  [Required]
  [RegularExpression("^(Left|Right|Both)$")]
  public string PreferredFoot { get; set; } = string.Empty;

  [Range(100, 250)]
  public int? HeightCm { get; set; }

  [Range(35, 200)]
  public int? WeightKg { get; set; }

  [Range(0, 300)]
  public int InternationalCaps { get; set; }

  [Range(0, 200)]
  public int InternationalGoals { get; set; }

  public bool IsCaptain { get; set; }

  [Range(1, int.MaxValue)]
  public int TeamId { get; set; }
}
