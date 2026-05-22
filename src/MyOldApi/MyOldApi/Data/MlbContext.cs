using Microsoft.EntityFrameworkCore;
using MyOldApi.Data.Entities;

namespace MyOldApi.Data;

public class MlbContext(DbContextOptions<MlbContext> options) : DbContext(options)
{
  public DbSet<Team> Teams => Set<Team>();
  public DbSet<Player> Players => Set<Player>();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Team>(entity =>
    {
      entity.HasMany(t => t.Players)
                .WithOne(p => p.Team)
                .HasForeignKey(p => p.TeamId);

      entity.HasData(
              new Team { Id = 1, Name = "Braves", City = "Atlanta", League = "NL", Division = "East" },
              new Team { Id = 2, Name = "Mets", City = "New York", League = "NL", Division = "East" }
          );
    });

    modelBuilder.Entity<Player>().HasData(
        // Atlanta Braves
        new Player { Id = 1, FirstName = "Ronald", LastName = "Acuña Jr.", Position = "RF", JerseyNumber = 13, TeamId = 1 },
        new Player { Id = 2, FirstName = "Ozzie", LastName = "Albies", Position = "2B", JerseyNumber = 1, TeamId = 1 },
        new Player { Id = 3, FirstName = "Matt", LastName = "Olson", Position = "1B", JerseyNumber = 28, TeamId = 1 },
        new Player { Id = 4, FirstName = "Austin", LastName = "Riley", Position = "3B", JerseyNumber = 27, TeamId = 1 },
        new Player { Id = 5, FirstName = "Sean", LastName = "Murphy", Position = "C", JerseyNumber = 12, TeamId = 1 },
        new Player { Id = 6, FirstName = "Michael", LastName = "Harris II", Position = "CF", JerseyNumber = 23, TeamId = 1 },
        new Player { Id = 7, FirstName = "Marcell", LastName = "Ozuna", Position = "LF", JerseyNumber = 20, TeamId = 1 },
        new Player { Id = 8, FirstName = "Chris", LastName = "Sale", Position = "SP", JerseyNumber = 51, TeamId = 1 },
        new Player { Id = 9, FirstName = "Spencer", LastName = "Strider", Position = "SP", JerseyNumber = 99, TeamId = 1 },
        new Player { Id = 10, FirstName = "Spencer", LastName = "Schwellenbach", Position = "SP", JerseyNumber = 56, TeamId = 1 },

        // New York Mets
        new Player { Id = 11, FirstName = "Francisco", LastName = "Lindor", Position = "SS", JerseyNumber = 12, TeamId = 2 },
        new Player { Id = 12, FirstName = "Juan", LastName = "Soto", Position = "RF", JerseyNumber = 22, TeamId = 2 },
        new Player { Id = 13, FirstName = "Pete", LastName = "Alonso", Position = "1B", JerseyNumber = 20, TeamId = 2 },
        new Player { Id = 14, FirstName = "Mark", LastName = "Vientos", Position = "3B", JerseyNumber = 27, TeamId = 2 },
        new Player { Id = 15, FirstName = "Francisco", LastName = "Alvarez", Position = "C", JerseyNumber = 4, TeamId = 2 },
        new Player { Id = 16, FirstName = "Jeff", LastName = "McNeil", Position = "2B", JerseyNumber = 1, TeamId = 2 },
        new Player { Id = 17, FirstName = "Brandon", LastName = "Nimmo", Position = "CF", JerseyNumber = 9, TeamId = 2 },
        new Player { Id = 18, FirstName = "Kodai", LastName = "Senga", Position = "SP", JerseyNumber = 34, TeamId = 2 },
        new Player { Id = 19, FirstName = "Sean", LastName = "Manaea", Position = "SP", JerseyNumber = 59, TeamId = 2 },
        new Player { Id = 20, FirstName = "Luis", LastName = "Severino", Position = "SP", JerseyNumber = 40, TeamId = 2 }
    );
  }
}
