using POS.Domains.Payment.Service.Domain;
using POS.Shared.Persistence.Repositories;

namespace POS.Domains.Payment.Service.Persistence;
/// <summary>
///
/// </summary>
public interface IPaymentRepository : IGenericRepository<PaymentAggregate>
{
}
