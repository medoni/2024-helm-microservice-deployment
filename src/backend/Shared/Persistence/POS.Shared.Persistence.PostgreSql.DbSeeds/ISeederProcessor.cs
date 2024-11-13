namespace POS.Shared.Persistence.PostgreSql.DbSeeds;

/// <summary>
/// Definition to process <see cref="ISeeder"/>
/// </summary>
public interface ISeederProcessor
{
    /// <summary>
    /// Processes the <see cref="ISeeder"/>.
    /// </summary>
    Task ProcessAsync(IServiceProvider svcp);
}
