namespace POS.Shared.Persistence.UOW;
/// <summary>
/// A specialized dictionary that maps aggregate root types to their corresponding repository factory functions.
/// It serves as a registry for repositories used by the Unit of Work pattern to create repository instances
/// for different aggregate types on demand. The keys are aggregate types, and the values are factory functions
/// that accept an IServiceProvider and return repository instances.
/// </summary>
public sealed class BaseRepositoryFactory : Dictionary<Type, Func<IServiceProvider, object>>
{
}
