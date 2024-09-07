namespace PizzaOrderingService.Domain;

public class Pizza
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Ingredients { get; set; }
    public decimal Price { get; set; }

    [Obsolete("Deserialization only")]
    public Pizza()
    {
        Name = null!;
        Ingredients = null!;
    }

    public Pizza(
        Guid id,
        string name,
        string ingredients,
        decimal price
    )
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Ingredients = ingredients ?? throw new ArgumentNullException(nameof(ingredients));
        Price = price;
    }
}
