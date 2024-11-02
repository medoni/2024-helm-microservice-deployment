namespace POS.Shared.Persistence.PostgreSql.DbSeeds;
public interface ISeederProcessor
{
    Task ProcessAsync(IServiceProvider svcp);
}
