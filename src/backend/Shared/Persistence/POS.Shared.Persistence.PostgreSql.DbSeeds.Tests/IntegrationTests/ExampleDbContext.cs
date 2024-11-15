using Microsoft.EntityFrameworkCore;

namespace POS.Shared.Persistence.PostgreSql.DbSeeds.Tests.IntegrationTests;
public class ExampleDbContext : DbContext
{
    public ExampleDbContext(DbContextOptions options) : base(options)
    {
    }
}
