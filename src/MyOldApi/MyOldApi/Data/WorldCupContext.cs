using Microsoft.EntityFrameworkCore;
using MyOldApi.Data.Entities;

namespace MyOldApi.Data;

public class WorldCupContext(DbContextOptions<WorldCupContext> options) : DbContext(options)
{
  public DbSet<Team> Teams => Set<Team>();
  public DbSet<Player> Players => Set<Player>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<Team>().HasData(
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

    modelBuilder.Entity<Player>().HasData(
      // Netherlands
      new Player { Id = 1, FirstName = "Daphne", LastName = "van Domselaar", DateOfBirth = new DateOnly(2000, 3, 6), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Goalkeeper", JerseyNumber = 1, PreferredFoot = "Right", HeightCm = 175, WeightKg = 65, InternationalCaps = 35, InternationalGoals = 0, IsCaptain = false, TeamId = 1 },
      new Player { Id = 2, FirstName = "Lize", LastName = "Kop", DateOfBirth = new DateOnly(1998, 3, 17), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Goalkeeper", JerseyNumber = 16, PreferredFoot = "Right", HeightCm = 173, WeightKg = 63, InternationalCaps = 10, InternationalGoals = 0, IsCaptain = false, TeamId = 1 },
      new Player { Id = 3, FirstName = "Jacintha", LastName = "Weimar", DateOfBirth = new DateOnly(1998, 6, 5), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Goalkeeper", JerseyNumber = 23, PreferredFoot = "Right", HeightCm = 178, WeightKg = 67, InternationalCaps = 2, InternationalGoals = 0, IsCaptain = false, TeamId = 1 },
      new Player { Id = 4, FirstName = "Stefanie", LastName = "van der Gragt", DateOfBirth = new DateOnly(1992, 8, 16), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Defender", JerseyNumber = 3, PreferredFoot = "Right", HeightCm = 178, WeightKg = 68, InternationalCaps = 105, InternationalGoals = 15, IsCaptain = false, TeamId = 1 },
      new Player { Id = 5, FirstName = "Dominique", LastName = "Janssen", DateOfBirth = new DateOnly(1995, 1, 17), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Defender", JerseyNumber = 20, PreferredFoot = "Right", HeightCm = 174, WeightKg = 66, InternationalCaps = 95, InternationalGoals = 18, IsCaptain = false, TeamId = 1 },
      new Player { Id = 6, FirstName = "Aniek", LastName = "Nouwen", DateOfBirth = new DateOnly(1999, 3, 9), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Defender", JerseyNumber = 5, PreferredFoot = "Right", HeightCm = 174, WeightKg = 66, InternationalCaps = 45, InternationalGoals = 5, IsCaptain = false, TeamId = 1 },
      new Player { Id = 7, FirstName = "Lynn", LastName = "Wilms", DateOfBirth = new DateOnly(2000, 10, 15), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Defender", JerseyNumber = 2, PreferredFoot = "Right", HeightCm = 168, WeightKg = 61, InternationalCaps = 40, InternationalGoals = 3, IsCaptain = false, TeamId = 1 },
      new Player { Id = 8, FirstName = "Merel", LastName = "van Dongen", DateOfBirth = new DateOnly(1993, 2, 11), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Defender", JerseyNumber = 4, PreferredFoot = "Left", HeightCm = 170, WeightKg = 62, InternationalCaps = 60, InternationalGoals = 2, IsCaptain = false, TeamId = 1 },
      new Player { Id = 9, FirstName = "Kerstin", LastName = "Casparij", DateOfBirth = new DateOnly(2000, 10, 19), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Defender", JerseyNumber = 14, PreferredFoot = "Right", HeightCm = 167, WeightKg = 60, InternationalCaps = 20, InternationalGoals = 1, IsCaptain = false, TeamId = 1 },
      new Player { Id = 10, FirstName = "Caitlin", LastName = "Dijkstra", DateOfBirth = new DateOnly(1999, 2, 2), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Defender", JerseyNumber = 22, PreferredFoot = "Left", HeightCm = 175, WeightKg = 65, InternationalCaps = 8, InternationalGoals = 0, IsCaptain = false, TeamId = 1 },
      new Player { Id = 11, FirstName = "Sherida", LastName = "Spitse", DateOfBirth = new DateOnly(1990, 5, 29), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Midfielder", JerseyNumber = 8, PreferredFoot = "Right", HeightCm = 168, WeightKg = 60, InternationalCaps = 230, InternationalGoals = 46, IsCaptain = true, TeamId = 1 },
      new Player { Id = 12, FirstName = "Jackie", LastName = "Groenen", DateOfBirth = new DateOnly(1994, 12, 17), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Midfielder", JerseyNumber = 10, PreferredFoot = "Right", HeightCm = 167, WeightKg = 59, InternationalCaps = 95, InternationalGoals = 8, IsCaptain = false, TeamId = 1 },
      new Player { Id = 13, FirstName = "Daniëlle", LastName = "van de Donk", DateOfBirth = new DateOnly(1991, 8, 5), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Midfielder", JerseyNumber = 6, PreferredFoot = "Right", HeightCm = 160, WeightKg = 54, InternationalCaps = 155, InternationalGoals = 35, IsCaptain = false, TeamId = 1 },
      new Player { Id = 14, FirstName = "Jill", LastName = "Roord", DateOfBirth = new DateOnly(1997, 4, 22), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Midfielder", JerseyNumber = 18, PreferredFoot = "Right", HeightCm = 176, WeightKg = 68, InternationalCaps = 90, InternationalGoals = 30, IsCaptain = false, TeamId = 1 },
      new Player { Id = 15, FirstName = "Damaris", LastName = "Egurrola", DateOfBirth = new DateOnly(1999, 8, 26), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Midfielder", JerseyNumber = 15, PreferredFoot = "Right", HeightCm = 177, WeightKg = 68, InternationalCaps = 20, InternationalGoals = 0, IsCaptain = false, TeamId = 1 },
      new Player { Id = 16, FirstName = "Victoria", LastName = "Pelova", DateOfBirth = new DateOnly(1999, 6, 3), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Midfielder", JerseyNumber = 17, PreferredFoot = "Right", HeightCm = 163, WeightKg = 57, InternationalCaps = 45, InternationalGoals = 3, IsCaptain = false, TeamId = 1 },
      new Player { Id = 17, FirstName = "Lieke", LastName = "Martens", DateOfBirth = new DateOnly(1992, 12, 16), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Forward", JerseyNumber = 11, PreferredFoot = "Right", HeightCm = 170, WeightKg = 62, InternationalCaps = 160, InternationalGoals = 60, IsCaptain = false, TeamId = 1 },
      new Player { Id = 18, FirstName = "Lineth", LastName = "Beerensteyn", DateOfBirth = new DateOnly(1996, 10, 11), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Forward", JerseyNumber = 7, PreferredFoot = "Right", HeightCm = 161, WeightKg = 56, InternationalCaps = 100, InternationalGoals = 30, IsCaptain = false, TeamId = 1 },
      new Player { Id = 19, FirstName = "Katja", LastName = "Snoeijs", DateOfBirth = new DateOnly(1996, 8, 31), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Forward", JerseyNumber = 19, PreferredFoot = "Right", HeightCm = 173, WeightKg = 64, InternationalCaps = 25, InternationalGoals = 9, IsCaptain = false, TeamId = 1 },
      new Player { Id = 20, FirstName = "Esmee", LastName = "Brugts", DateOfBirth = new DateOnly(2003, 7, 28), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Forward", JerseyNumber = 12, PreferredFoot = "Left", HeightCm = 168, WeightKg = 59, InternationalCaps = 25, InternationalGoals = 7, IsCaptain = false, TeamId = 1 },
      new Player { Id = 21, FirstName = "Romée", LastName = "Leuchter", DateOfBirth = new DateOnly(2001, 1, 12), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Forward", JerseyNumber = 21, PreferredFoot = "Right", HeightCm = 167, WeightKg = 60, InternationalCaps = 15, InternationalGoals = 4, IsCaptain = false, TeamId = 1 },
      new Player { Id = 22, FirstName = "Renate", LastName = "Jansen", DateOfBirth = new DateOnly(1990, 12, 17), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Forward", JerseyNumber = 13, PreferredFoot = "Right", HeightCm = 171, WeightKg = 62, InternationalCaps = 60, InternationalGoals = 16, IsCaptain = false, TeamId = 1 },
      new Player { Id = 23, FirstName = "Vivianne", LastName = "Miedema", DateOfBirth = new DateOnly(1996, 7, 15), Nationality = "Dutch", FifaCountryCode = "NED", Position = "Forward", JerseyNumber = 9, PreferredFoot = "Right", HeightCm = 175, WeightKg = 67, InternationalCaps = 120, InternationalGoals = 98, IsCaptain = false, TeamId = 1 },

      // Sweden
      new Player { Id = 24, FirstName = "Zecira", LastName = "Musovic", DateOfBirth = new DateOnly(1996, 5, 26), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Goalkeeper", JerseyNumber = 1, PreferredFoot = "Right", HeightCm = 180, WeightKg = 70, InternationalCaps = 35, InternationalGoals = 0, IsCaptain = false, TeamId = 2 },
      new Player { Id = 25, FirstName = "Jennifer", LastName = "Falk", DateOfBirth = new DateOnly(1993, 4, 5), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Goalkeeper", JerseyNumber = 12, PreferredFoot = "Right", HeightCm = 172, WeightKg = 64, InternationalCaps = 45, InternationalGoals = 0, IsCaptain = false, TeamId = 2 },
      new Player { Id = 26, FirstName = "Tove", LastName = "Enblom", DateOfBirth = new DateOnly(1994, 11, 20), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Goalkeeper", JerseyNumber = 23, PreferredFoot = "Right", HeightCm = 177, WeightKg = 66, InternationalCaps = 5, InternationalGoals = 0, IsCaptain = false, TeamId = 2 },
      new Player { Id = 27, FirstName = "Magdalena", LastName = "Eriksson", DateOfBirth = new DateOnly(1993, 9, 8), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Defender", JerseyNumber = 6, PreferredFoot = "Left", HeightCm = 173, WeightKg = 64, InternationalCaps = 115, InternationalGoals = 13, IsCaptain = false, TeamId = 2 },
      new Player { Id = 28, FirstName = "Jonna", LastName = "Andersson", DateOfBirth = new DateOnly(1993, 1, 2), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Defender", JerseyNumber = 2, PreferredFoot = "Left", HeightCm = 167, WeightKg = 60, InternationalCaps = 95, InternationalGoals = 2, IsCaptain = false, TeamId = 2 },
      new Player { Id = 29, FirstName = "Amanda", LastName = "Ilestedt", DateOfBirth = new DateOnly(1993, 1, 17), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Defender", JerseyNumber = 13, PreferredFoot = "Right", HeightCm = 178, WeightKg = 68, InternationalCaps = 80, InternationalGoals = 15, IsCaptain = false, TeamId = 2 },
      new Player { Id = 30, FirstName = "Nathalie", LastName = "Björn", DateOfBirth = new DateOnly(1997, 5, 4), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Defender", JerseyNumber = 14, PreferredFoot = "Right", HeightCm = 174, WeightKg = 65, InternationalCaps = 55, InternationalGoals = 3, IsCaptain = false, TeamId = 2 },
      new Player { Id = 31, FirstName = "Hanna", LastName = "Lundkvist", DateOfBirth = new DateOnly(2002, 7, 17), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Defender", JerseyNumber = 4, PreferredFoot = "Right", HeightCm = 168, WeightKg = 60, InternationalCaps = 20, InternationalGoals = 1, IsCaptain = false, TeamId = 2 },
      new Player { Id = 32, FirstName = "Linda", LastName = "Sembrant", DateOfBirth = new DateOnly(1987, 5, 15), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Defender", JerseyNumber = 3, PreferredFoot = "Right", HeightCm = 173, WeightKg = 64, InternationalCaps = 140, InternationalGoals = 7, IsCaptain = false, TeamId = 2 },
      new Player { Id = 33, FirstName = "Kosovare", LastName = "Asllani", DateOfBirth = new DateOnly(1989, 7, 29), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Midfielder", JerseyNumber = 9, PreferredFoot = "Right", HeightCm = 166, WeightKg = 58, InternationalCaps = 185, InternationalGoals = 46, IsCaptain = true, TeamId = 2 },
      new Player { Id = 34, FirstName = "Filippa", LastName = "Angeldahl", DateOfBirth = new DateOnly(1997, 7, 14), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Midfielder", JerseyNumber = 16, PreferredFoot = "Right", HeightCm = 168, WeightKg = 59, InternationalCaps = 45, InternationalGoals = 12, IsCaptain = false, TeamId = 2 },
      new Player { Id = 35, FirstName = "Caroline", LastName = "Seger", DateOfBirth = new DateOnly(1985, 3, 19), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Midfielder", JerseyNumber = 17, PreferredFoot = "Right", HeightCm = 171, WeightKg = 62, InternationalCaps = 240, InternationalGoals = 33, IsCaptain = false, TeamId = 2 },
      new Player { Id = 36, FirstName = "Elin", LastName = "Rubensson", DateOfBirth = new DateOnly(1993, 5, 11), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Midfielder", JerseyNumber = 18, PreferredFoot = "Right", HeightCm = 166, WeightKg = 58, InternationalCaps = 70, InternationalGoals = 6, IsCaptain = false, TeamId = 2 },
      new Player { Id = 37, FirstName = "Hanna", LastName = "Bennison", DateOfBirth = new DateOnly(2002, 10, 16), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Midfielder", JerseyNumber = 19, PreferredFoot = "Right", HeightCm = 170, WeightKg = 61, InternationalCaps = 35, InternationalGoals = 3, IsCaptain = false, TeamId = 2 },
      new Player { Id = 38, FirstName = "Rebecka", LastName = "Blomqvist", DateOfBirth = new DateOnly(1997, 7, 24), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 11, PreferredFoot = "Right", HeightCm = 167, WeightKg = 60, InternationalCaps = 30, InternationalGoals = 2, IsCaptain = false, TeamId = 2 },
      new Player { Id = 39, FirstName = "Fridolina", LastName = "Rolfö", DateOfBirth = new DateOnly(1993, 11, 24), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 10, PreferredFoot = "Left", HeightCm = 178, WeightKg = 70, InternationalCaps = 95, InternationalGoals = 29, IsCaptain = false, TeamId = 2 },
      new Player { Id = 40, FirstName = "Stina", LastName = "Blackstenius", DateOfBirth = new DateOnly(1996, 2, 5), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 15, PreferredFoot = "Right", HeightCm = 174, WeightKg = 66, InternationalCaps = 100, InternationalGoals = 35, IsCaptain = false, TeamId = 2 },
      new Player { Id = 41, FirstName = "Sofia", LastName = "Jakobsson", DateOfBirth = new DateOnly(1990, 4, 23), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 7, PreferredFoot = "Right", HeightCm = 174, WeightKg = 65, InternationalCaps = 155, InternationalGoals = 23, IsCaptain = false, TeamId = 2 },
      new Player { Id = 42, FirstName = "Madelen", LastName = "Janogy", DateOfBirth = new DateOnly(1995, 11, 29), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 20, PreferredFoot = "Right", HeightCm = 169, WeightKg = 62, InternationalCaps = 35, InternationalGoals = 8, IsCaptain = false, TeamId = 2 },
      new Player { Id = 43, FirstName = "Johanna", LastName = "Rytting Kaneryd", DateOfBirth = new DateOnly(1997, 2, 12), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 21, PreferredFoot = "Right", HeightCm = 165, WeightKg = 58, InternationalCaps = 45, InternationalGoals = 6, IsCaptain = false, TeamId = 2 },
      new Player { Id = 44, FirstName = "Olivia", LastName = "Schough", DateOfBirth = new DateOnly(1991, 3, 11), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 8, PreferredFoot = "Left", HeightCm = 172, WeightKg = 63, InternationalCaps = 105, InternationalGoals = 14, IsCaptain = false, TeamId = 2 },
      new Player { Id = 45, FirstName = "Lina", LastName = "Hurtig", DateOfBirth = new DateOnly(1995, 9, 5), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 22, PreferredFoot = "Right", HeightCm = 180, WeightKg = 72, InternationalCaps = 70, InternationalGoals = 20, IsCaptain = false, TeamId = 2 },
      new Player { Id = 46, FirstName = "Anna", LastName = "Anvegård", DateOfBirth = new DateOnly(1997, 5, 3), Nationality = "Swedish", FifaCountryCode = "SWE", Position = "Forward", JerseyNumber = 5, PreferredFoot = "Right", HeightCm = 168, WeightKg = 61, InternationalCaps = 40, InternationalGoals = 11, IsCaptain = false, TeamId = 2 });
  }
}
