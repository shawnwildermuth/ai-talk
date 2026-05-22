using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace MyOldApi.Tests.Endpoints;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureAppConfiguration((_, configBuilder) =>
    {
      var dbDirectory = Path.Combine(Path.GetTempPath(), "MyOldApiTests", Guid.NewGuid().ToString("N"));
      Directory.CreateDirectory(dbDirectory);
      var dbPath = Path.Combine(dbDirectory, "worldcup.db");

      configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
      {
        ["ConnectionStrings:WorldCupConnection"] = $"Data Source={dbPath}"
      });
    });
  }
}
