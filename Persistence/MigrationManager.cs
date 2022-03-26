using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Persistence;

public class MigrationManager
{
    private readonly ILogger<MigrationManager> _logger;
    private readonly ConfigurationManager _configuration;
    private readonly DatabaseFacade _databaseFacade;

    public MigrationManager(ILogger<MigrationManager> logger, ConfigurationManager configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _databaseFacade = GetDatabase();
    }

    public void Migrate()
    {
        var pending = _databaseFacade.GetPendingMigrations().ToList();
        var applied = _databaseFacade.GetAppliedMigrations().ToList();

        if (applied.Any())
        {
            var appliedText = string.Join(Environment.NewLine, applied);
            _logger.LogInformation("These are the applied migrations: {newline}{appliedMigrations}", Environment.NewLine, appliedText);
        }

        if (pending.Any())
        {
            var pendingText = string.Join(Environment.NewLine, pending);
            _logger.LogInformation("These are the pending migrations: {newline}{pendingMigrations}", Environment.NewLine, pendingText);
            _logger.LogInformation("Starting migration...");
            _databaseFacade.Migrate();
            _logger.LogInformation("Migration is finished.");
        }
    }

    private DatabaseFacade GetDatabase()
    {
        var factory = new VoucherContextFactory();
        var dbContext = factory.CreateDbContext(new string[] { _configuration["Database:ConnectionString"] });
        return dbContext.Database;
    }
}