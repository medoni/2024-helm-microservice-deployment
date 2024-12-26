using POS.Domains.Payment.Service.Domain;

namespace POS.Domains.Payment.Service;
/// <summary>
/// Definition for a repository for loading and storing payments.
/// </summary>
public interface IPaymentRepository
{
    /// <summary>
    /// Adds and new entity.
    /// </summary>
    Task AddOrUpdateAsync(PaymentEntity entity);

    /// <summary>
    /// Loads a payment.
    /// </summary>
    Task<PaymentEntity> GetAsync(Guid paymentId);

    /// <summary>
    /// Loads a payment by the entity id.
    /// </summary>
    Task<PaymentEntity> GetByEntityIdAsync(EntityTypes type, string entityId);

    /// <summary>
    /// Loads a payment by the entity id.
    /// </summary>
    Task<PaymentEntity?> TryGetByEntityIdAsync(EntityTypes type, string entityId);
}
