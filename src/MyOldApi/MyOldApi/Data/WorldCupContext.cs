using Microsoft.EntityFrameworkCore;
using MyOldApi.Data.Entities;

namespace MyOldApi.Data;

public class WorldCupContext(DbContextOptions<WorldCupContext> options) : DbContext(options)
{
  public DbSet<Team> Teams => Set<Team>();
  public DbSet<Player> Players => Set<Player>();
}
