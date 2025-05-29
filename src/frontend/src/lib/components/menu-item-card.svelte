<script>
  import Button from './button.svelte';
  import { cartService } from '$lib/services/cart-service';

  /**
   * @type import('$lib/services/pizza-ordering-service-api').MenuItemDto
   */
  export let item;

  function addToCart() {
    cartService.addToCart(item);
  }
</script>

<div class="menu-card">
  <img src={'https://dummyimage.com/150/000/fff.png&text=unknown'} alt={item.name} />
  <div class="content">
    <h3>{item.name}</h3>
    <p class="description">{item.description}</p>
    <p class="ingredients">
      <small>{item.ingredients.join(', ')}</small>
    </p>
    <div class="footer">
      <span class="price">${item.price.price.gross.toFixed(2)} {item.price.price.currency}</span>
      <Button label="Add to Cart" onClick={addToCart} />
    </div>
  </div>
</div>

<style>
  .menu-card {
    border: 1px solid #ddd;
    border-radius: 8px;
    overflow: hidden;
    transition: transform 0.2s, box-shadow 0.2s;
    background: white;
  }

  .menu-card:hover {
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
