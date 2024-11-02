using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace POS.Shared.Persistence.PostgreSql.DbSeeds;
internal class PostgreSqlSeederProcessor : ISeederProcessor
{
    private readonly ILogger<PostgreSqlSeederProcessor> Logging;
    private readonly Func<IServiceProvider, DbContext> DbContextAccessor;

    public PostgreSqlSeederProcessor(
        ILogger<PostgreSqlSeederProcessor> logging,
        Func<IServiceProvider, DbContext> dbContextAccessor)
    {
        Logging = logging ?? throw new ArgumentNullException(nameof(logging));
        DbContextAccessor = dbContextAccessor ?? throw new ArgumentNullException(nameof(dbContextAccessor));
    }

    public async Task ProcessAsync(IServiceProvider svcp)
    {
        await using var scope = svcp.CreateAsyncScope();
        var dbContext = DbContextAccessor(svcp);


        try
        {
            Logging.LogInformation("Start seeding database ...");

            await EnsureSeedTableExistsAsync(dbContext);
            await ProcessSeedersAsync(svcp, dbContext);

            Logging.LogInformation("Successful seeded database.");
        }
        catch (Exception ex)
        {
            Logging.LogCritical(ex, "Error seeding database.");
            throw;
        }
    }

    private async Task ProcessSeedersAsync(
        IServiceProvider svcp,
        DbContext dbContext
    )
    {
        var seeders = svcp.GetRequiredService<IEnumerable<ISeeder>>()
            .OrderBy(x => x.AddedAt).ThenBy(x => x.Name)
            .ToArray();

        if (seeders.Length == 0) throw new InvalidOperationException($"No '{typeof(ISeeder).Name}' found. Are their any instances registered?");

        foreach (var seeder in seeders)
        {
            var fullSeederName = $"{seeder.AddedAt:u}-{seeder.Name}";
            await ProcessSingleSeederAsync(
                dbContext,
                seeder,
                fullSeederName
            );
        }
    }

    private async Task EnsureSeedTableExistsAsync(DbContext dbContext)
    {
        var createTableSql =
            """
            CREATE TABLE IF NOT EXISTS __DBSeedHistory (
                SeederId TEXT PRIMARY KEY,
                SeededAt TIMESTAMPTZ
            );

            CREATE INDEX IF NOT EXISTS idx_DBSeedHistory_SeederId ON __DBSeedHistory (SeederId);
            """;

        await dbContext.Database.ExecuteSqlRawAsync(createTableSql);
    }

    private async Task ProcessSingleSeederAsync(
        DbContext dbContext,
        ISeeder seeder,
        string fullSeederName
    )
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync();
        try
        {
            if (await TryAddSeederAsync(dbContext, fullSeederName))
            {
                Logging.LogInformation("Processing seeder '{seederName}' ...", fullSeederName);

                await seeder.SeedAsync();

                Logging.LogInformation("Successful processed '{seederName}'.", fullSeederName);
            }
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            Logging.LogError(ex, "Error processing seeder '{seederName}'.", fullSeederName);
            await transaction.RollbackAsync();
            throw;
        }
    }

    private static async Task<bool> TryAddSeederAsync(DbContext dbContext, string fullSeederName)
    {
        var sql = """
            INSERT INTO __DBSeedHistory(SeederId, SeededAt)
            VALUES ({0}, {1});
            """;

        try
        {
            await dbContext.Database.ExecuteSqlRawAsync(sql, fullSeederName, DateTimeOffset.UtcNow);
            return true;
        }
        catch (PostgresException ex) when (ex.SqlState == "23505")
        {
            return false;
        }
    }
}
