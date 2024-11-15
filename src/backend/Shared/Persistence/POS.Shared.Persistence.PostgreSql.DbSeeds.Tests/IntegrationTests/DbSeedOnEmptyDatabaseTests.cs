using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using POS.Shared.Testing;
using Testcontainers.PostgreSql;

namespace POS.Shared.Persistence.PostgreSql.DbSeeds.Tests.IntegrationTests;

[TestFixture]
[Category(TestCategories.LongRunningIntegration)]
public class DbSeedOnEmptyDatabaseTests : BaseDatabaseTests<ExampleDbContext>
{
    private PostgreSqlContainer Container { get; set; }

    [OneTimeSetUp]
    public async Task SetUpAsync()
    {
        Container = new PostgreSqlBuilder()
            .Build();
        await Container.StartAsync();

        var services = new ServiceCollection()
            .AddLogging()
            .AddSeederSupport<ExampleDbContext>()
            .AddDbContext<ExampleDbContext>(options =>
            {
                options.UseNpgsql(Container.GetConnectionString());
            })
            .AddScoped<ISeeder, Seeder1>()
            .AddScoped<ISeeder, Seeder2>();

        ServiceProvider = services.BuildServiceProvider();
    }

    [OneTimeTearDown]
    public async Task TearDownAsync()
    {
        await Container.DisposeAsync();
    }
    [Order(1)]
    [Test]
    public async Task Should_Execute_Seeder_And_Store_Result()
    {
        // arrange
        var seeder1Invoked = false;
        Seeder1.OnSeedAsync += () => seeder1Invoked = true;
        var seeder2Invoked = false;
        Seeder2.OnSeedAsync += () => seeder2Invoked = true;

        // act
        using (var scope = ServiceProvider.CreateScope())
        {
            var processor = scope.ServiceProvider.GetRequiredService<ISeederProcessor>();
            await processor.ProcessAsync(scope.ServiceProvider);
        }

        // assert
        Assert.That(seeder1Invoked, Is.True);
        Assert.That(seeder2Invoked, Is.True);

        VerifyThatSeederTableContainsEntry<Seeder1>();
        VerifyThatSeederTableContainsEntry<Seeder2>();
    }

    [Order(2)]
    [Test]
    public async Task Should_Not_Execute_Previously_Executed_Seeders()
    {
        // arrange
        var seeder1Invoked = false;
        Seeder1.OnSeedAsync += () => seeder1Invoked = true;

        // act
        using (var scope = ServiceProvider.CreateScope())
        {
            var processor = scope.ServiceProvider.GetRequiredService<ISeederProcessor>();
            await processor.ProcessAsync(scope.ServiceProvider);
        }

        // assert
        Assert.That(seeder1Invoked, Is.False);

        VerifyThatSeederTableContainsEntry<Seeder1>();
    }

    public class Seeder1 : ISeeder
    {
        public string Name => "Seeder1";

        public DateTimeOffset AddedAt => new DateTimeOffset(2024, 11, 12, 06, 50, 23, TimeSpan.Zero);

        public Task SeedAsync()
        {
            OnSeedAsync?.Invoke();

            return Task.CompletedTask;
        }
        public static Action OnSeedAsync { get; set; }
    }

    public class Seeder2 : ISeeder
    {
        public string Name => "Seeder2";

        public DateTimeOffset AddedAt => new DateTimeOffset(2024, 11, 12, 06, 50, 23, TimeSpan.Zero);

        public Task SeedAsync()
        {
            OnSeedAsync?.Invoke();

            return Task.CompletedTask;
        }
        public static Action OnSeedAsync { get; set; }
    }
}
