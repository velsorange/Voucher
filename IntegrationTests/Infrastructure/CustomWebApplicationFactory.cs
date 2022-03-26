using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IntegrationTests.Infrastructure;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IHostBuilder CreateHostBuilder()
    {
        var builder = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("appsettings.Test.json", optional: true, reloadOnChange: true);
                config.AddJsonFile("appsettings.Test.Development.json", optional: true, reloadOnChange: true);
            })
            .ConfigureLogging(f => f.AddConsole())
            .ConfigureWebHostDefaults(x =>
            {
                x.UseStartup<TStartup>();
            });

        return builder;
    }
}