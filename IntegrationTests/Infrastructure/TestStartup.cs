using Domain;
using Domain.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using Persistence.Repository;
using VoucherApi.Controllers;
using VoucherApi.Mapping;

namespace IntegrationTests.Infrastructure;

public class TestStartup
{
    private readonly ILogger<TestStartup> _logger;
    public IConfiguration Configuration { get; }
    public TestStartup(IConfiguration configuration)
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        _logger = loggerFactory.CreateLogger<TestStartup>();
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(MappingProfile));
        services.AddDomainService();
        AddRepositoryServices(services);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddMvc().AddApplicationPart(typeof(VoucherController).Assembly);
        services.AddSwaggerGen();


        var sp = services.BuildServiceProvider();

        using var scope = sp.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var db = scopedServices.GetRequiredService<VoucherContext>();
        try
        {
            db.Database.EnsureDeleted();
        }
        catch (SqlException e)
        {
            _logger.LogError(e.Message, e);
        }
        db.Database.EnsureCreated();
    }

    private void AddRepositoryServices(IServiceCollection services)
    {
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddDbContext<VoucherContext>(options => options.UseSqlServer(Configuration["Database:ConnectionString"]));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}