namespace POS.Shared.Persistence.PostgreSql.DbSeeds;
public interface ISeeder
{
    string Name { get; }
    DateTimeOffset AddedAt { get; }

    Task SeedAsync();
}
