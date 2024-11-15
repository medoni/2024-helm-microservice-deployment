namespace POS.Shared.Persistence.UOW;

/// <summary>
/// Definition of an factory to create a new <see cref="IUnitOfWork"/> instance.
/// </summary>
public delegate IUnitOfWork UnitOfWorkFactory();
