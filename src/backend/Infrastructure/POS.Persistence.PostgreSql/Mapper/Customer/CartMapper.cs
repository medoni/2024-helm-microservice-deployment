using POS.Domains.Customer.Abstractions.Carts;
using POS.Domains.Customer.Domain.Carts;
using POS.Persistence.PostgreSql.Data.Customer;

namespace POS.Persistence.PostgreSql.Mapper.Customer;
internal static class CartMapper
{
    public static CartState ToState(this CartEntity entity)
    {
        return new CartState(
            entity.Id,
            entity.CreatedAt,
            entity.MenuId,
            entity.Currency,
            entity.Items.ToItems()
        )
        {
            State = entity.State,
            LastChangedAt = entity.LastChangedAt,
            CheckoutInfo = entity.CheckoutInfo?.ToDomain()
        };
    }

    public static CartCheckoutInfo ToDomain(this CartCheckoutInfoEntity entity)
    {
        return new CartCheckoutInfo(entity.CheckedOutAt, entity.OrderId);
    }
}
