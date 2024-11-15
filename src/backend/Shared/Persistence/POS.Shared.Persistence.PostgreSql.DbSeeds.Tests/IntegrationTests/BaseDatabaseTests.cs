using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace POS.Shared.Persistence.PostgreSql.DbSeeds.Tests.IntegrationTests;

public abstract class BaseDatabaseTests<TDbContext>
where TDbContext : DbContext
{
    protected IServiceProvider ServiceProvider { get; set; }

    protected void VerifyThatSeederTableContainsEntry<TSeeder>()
    where TSeeder : ISeeder, new()
    {
        var seeder = new TSeeder();
        var fullSeederName = $"{seeder.AddedAt:u}-{seeder.Name}";

        using var scope = ServiceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();

        var result = dbContext.Database.SqlQuery<int>(@$"SELECT Count(SeederId) As ""Value"" FROM __DBSeedHistory WHERE SeederId={fullSeederName}").First();

        Assert.That(result, Is.EqualTo(1));
    }
}
