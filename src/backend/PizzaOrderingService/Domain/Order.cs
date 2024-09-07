namespace PizzaOrderingService.Domain;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public int PizzaId { get; set; }
    public Pizza Pizza { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }

    [Obsolete("Deserialization only")]
    public Order()
    {
        CustomerName = null!;
        Pizza = null!;
    }

    public Order(
        int id,
        string customerName,
        int pizzaId,
        Pizza pizza,
        int quantity,
        decimal totalPrice
    )
    {
        Id = id;
        CustomerName = customerName ?? throw new ArgumentNullException(nameof(customerName));
        PizzaId = pizzaId;
        Pizza = pizza ?? throw new ArgumentNullException(nameof(pizza));
        Quantity = quantity;
        TotalPrice = totalPrice;
    }
}
