namespace POS.Shared.Persistence.UOW;
/// <summary>
///
/// </summary>
public sealed class BaseRepositoryFactory : Dictionary<Type, Func<IServiceProvider, object>>
{
    // key is Aggregate type of the Repository
}
