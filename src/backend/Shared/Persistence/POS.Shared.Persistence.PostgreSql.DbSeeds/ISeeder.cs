namespace POS.Shared.Persistence.PostgreSql.DbSeeds;

/// <summary>
/// Definition for a Seeder to fill the database with example data.
/// </summary>
public interface ISeeder
{
    /// <summary>
    /// Name of the seeder.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Date and time when the Seeder was created.
    /// </summary>
    DateTimeOffset AddedAt { get; }

    /// <summary>
    /// The implementation to seed the database.
    /// </summary>
    Task SeedAsync();
}
