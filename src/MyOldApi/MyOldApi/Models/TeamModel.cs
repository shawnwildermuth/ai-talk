using System.ComponentModel.DataAnnotations;

namespace MyOldApi.Models;

public class TeamModel
{
  [Required]
  [StringLength(100, MinimumLength = 2)]
  public string Name { get; set; } = string.Empty;

  [Required]
  [StringLength(100, MinimumLength = 2)]
  public string Country { get; set; } = string.Empty;

  [Required]
  [RegularExpression("^[A-Z]{3}$")]
  public string FifaCountryCode { get; set; } = string.Empty;

  [Required]
  [StringLength(50, MinimumLength = 2)]
  public string Confederation { get; set; } = string.Empty;

  [Required]
  [StringLength(2, MinimumLength = 1)]
  public string Group { get; set; } = string.Empty;

  [Required]
  [StringLength(100, MinimumLength = 2)]
  public string HeadCoach { get; set; } = string.Empty;

  [Range(1, 210)]
  public int FifaRanking { get; set; }
}
