using Microsoft.EntityFrameworkCore;
using POS.Domains.Payment.Service;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Exceptions;
using POS.Persistence.PostgreSql.Data;

namespace POS.Persistence.PostgreSql.Repositories;
internal class PostgresPaymentRepository(
    POSDbContext dbContext
) : IPaymentRepository
{
    public async Task AddAsync(PaymentEntity entity)
    {
        dbContext.Payments.Add(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(PaymentEntity entity)
    {
        dbContext.Payments.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<PaymentEntity> GetAsync(Guid paymentId)
    {
        var payment = await dbContext.Payments.FindAsync(paymentId)
            ?? throw new PaymentNotFoundException(paymentId);

        return payment;
    }

    public async Task<PaymentEntity> GetByEntityIdAsync(EntityTypes type, string entityId)
    {
        return (await TryGetByEntityIdAsync(type, entityId))
            ?? throw new PaymentNotFoundException(type, entityId);
    }

    public async Task<PaymentEntity?> TryGetByEntityIdAsync(EntityTypes type, string entityId)
    {
        var payment = await dbContext.Payments
            .Where(x => x.EntityType == type && x.EntityId == entityId)
            .FirstOrDefaultAsync();

        return payment;
    }
}
