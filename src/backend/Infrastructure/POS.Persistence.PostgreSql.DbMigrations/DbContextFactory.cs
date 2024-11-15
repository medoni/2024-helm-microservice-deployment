using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using POS.Persistence.PostgreSql.Data;

namespace POS.Persistence.PostgreSql.DbMigrations;

internal class DbContextFactory : IDesignTimeDbContextFactory<POSDbContext>
{
    private const string CONNECTION_STRING_ENV = "CONNECTION_STRING";

    public POSDbContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(CONNECTION_STRING_ENV) ??
            throw new InvalidOperationException($"The environment variable '{CONNECTION_STRING_ENV}' was not set.");

        var dbContextBuilder = new DbContextOptionsBuilder<POSDbContext>();
        dbContextBuilder.UseNpgsql(connectionString, options => options.MigrationsAssembly(typeof(DbContextFactory).Assembly.FullName));

        return new POSDbContext(dbContextBuilder.Options);
    }
}
