<script>
  import Button from './button.svelte';
  import { cartService } from '$lib/services/cart-service';
  import { Pizza } from '$lib/models/pizza';

  /**
   * @type Pizza
   */
  export let pizza;

  function addToCart() {
    cartService.addToCart(pizza);
  }
</script>

<div class="pizza-card">
  <img src={pizza.imageUrl} alt={pizza.name} />
  <div class="content">
    <h3>{pizza.name}</h3>
    <p class="description">{pizza.description}</p>
    <p class="ingredients">
      <small>{pizza.ingredients.join(', ')}</small>
    </p>
    <div class="footer">
      <span class="price">${pizza.price.toFixed(2)}</span>
      <Button label="Add to Cart" onClick={addToCart} />
    </div>
  </div>
</div>

<style>
  .pizza-card {
    border: 1px solid #ddd;
    border-radius: 8px;
    overflow: hidden;
    transition: transform 0.2s, box-shadow 0.2s;
    background: white;
  }

  .pizza-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 5px 15px rgba(0,0,0,0.1);
  }

  img {
    width: 100%;
    height: 150px;
    object-fit: cover;
  }

  .content {
    padding: 15px;
  }

  h3 {
    margin: 0 0 10px 0;
    color: #333;
  }

  .description {
    margin-bottom: 5px;
  }

  .ingredients {
    color: #666;
    margin-bottom: 15px;
  }

  .footer {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  .price {
    font-weight: bold;
    font-size: 1.2rem;
    color: #ff3e00;
  }
</style>
