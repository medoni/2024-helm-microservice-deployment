<script>
  import { CartItem } from '$lib/models/cart-item';
  import { cartService } from '$lib/services/cart-service';

  /**
   * @type CartItem
   */
  export let item;

  function increment() {
    cartService.updateQuantity(item.pizza.id, item.quantity + 1);
  }

  function decrement() {
    cartService.updateQuantity(item.pizza.id, item.quantity - 1);
  }

  function remove() {
    cartService.removeFromCart(item.pizza.id);
  }
</script>

<div class="cart-item">
  <img src={item.pizza.imageUrl} alt={item.pizza.name} />
  <div class="details">
    <h3>{item.pizza.name}</h3>
    <p>${item.pizza.price.toFixed(2)} each</p>
  </div>
  <div class="quantity">
    <button on:click={decrement}>-</button>
    <span>{item.quantity}</span>
    <button on:click={increment}>+</button>
  </div>
  <div class="total">
    ${item.totalPrice.toFixed(2)}
  </div>
  <button class="remove" on:click={remove}>×</button>
</div>

<style>
  .cart-item {
    display: flex;
    align-items: center;
    padding: 10px;
    border-bottom: 1px solid #eee;
    gap: 10px;
  }

  img {
    width: 60px;
    height: 60px;
    object-fit: cover;
    border-radius: 4px;
  }

  .details {
    flex: 1;
  }

  h3 {
    margin: 0;
    font-size: 1rem;
  }

  .quantity {
    display: flex;
    align-items: center;
  }

  .quantity button {
    background: #f0f0f0;
    border: none;
    width: 25px;
    height: 25px;
    border-radius: 4px;
    cursor: pointer;
  }

  .quantity span {
    margin: 0 10px;
    min-width: 20px;
    text-align: center;
  }

  .total {
    font-weight: bold;
    min-width: 80px;
    text-align: right;
  }

  .remove {
    background: none;
    border: none;
    color: #ff3e00;
    font-size: 1.5rem;
    cursor: pointer;
    padding: 0 5px;
  }
</style>
