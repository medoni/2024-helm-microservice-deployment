using Microsoft.EntityFrameworkCore;
using POS.Domains.Payment.Service.Domain;
using POS.Domains.Payment.Service.Persistence;
using POS.Persistence.PostgreSql.Data;
using POS.Persistence.PostgreSql.Data.Payment;
using POS.Persistence.PostgreSql.Mapper.Payment;

namespace POS.Persistence.PostgreSql.Repositories;
partial class PostgresPaymentRepository :
    PostgresAggregateRootRepository<PaymentAggregate, PaymentAggregateState, PaymentEntity>,
    IPaymentRepository
{
    public PostgresPaymentRepository(POSDbContext dbContext) : base(dbContext)
    {
    }

    private IQueryable<PaymentEntity> CreateQuery()
    {
        return DbContext.Payments
            .TagWithCallSite();
    }

    protected override IQueryable<PaymentEntity> GetEntityByIdQuery(Guid id)
    {
        return CreateQuery()
            .Where(x => x.Id == id);
    }

    protected override IQueryable<PaymentEntity> IterateEntitiesQuery()
    {
        return CreateQuery()
            .OrderBy(x => x.CreatedAt);
    }

    protected override PaymentAggregate CreateAggregateFromEntity(PaymentEntity entity)
    {
        var state = entity.ToState();
        var aggregate = new PaymentAggregate(state);
        return aggregate;
    }
}
