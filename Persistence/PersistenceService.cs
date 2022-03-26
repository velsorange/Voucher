using Domain.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repository;

namespace Persistence;
public static class PersistenceService
{
    public static void AddPersistenceService(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddScoped<IVoucherRepository, VoucherRepository>();
        services.AddDbContext<VoucherContext>(options => options.UseSqlServer(configuration["Database:ConnectionString"]));
    }
}