<script>
  import { cartService } from '$lib/services/cart-service';
  import { orderService } from '$lib/services/order-service';
  import CartItemCard from '$lib/components/cart-item.svelte';
  import Button from '$lib/components/button.svelte';
  import { goto } from '$app/navigation';
  import { CartItem } from '$lib/models/cart-item';

  /**
   * @type import('$lib/services/pizza-ordering-service-api').CartItemDto[]
   */
  let cartItems = [];

  const unsubscribe = cartService.subscribe(items => {
    cartItems = items;
  });

  function getTotalAmount() {
    return cartItems.reduce((sum, item) => sum + item.quantity * item.unitPrice.price.gross, 0);
  }

  function placeOrder() {
    if (cartItems.length === 0) return;

    throw 'Not implemented';

    // const newOrder = orderService.createOrder(cartItems, getTotalAmount());
    // cartService.clearCart();
    // goto(`/orders/${newOrder.id}`);
  }

  function continueShopping() {
    goto('/menu');
  }
</script>

<svelte:head>
  <title>Cart | Pizza Shop</title>
</svelte:head>

<div class="cart-container">
  <h1>Your Cart</h1>

  {#if cartItems.length === 0}
    <div class="empty-cart">
      <p>Your cart is empty</p>
      <Button label="Browse Menu" onClick={continueShopping} />
    </div>
  {:else}
    <div class="cart-items">
      {#each cartItems as item (item.id)}
        <CartItemCard {item} />
      {/each}
    </div>

    <div class="cart-summary">
      <div class="total">
        <span>Total Amount:</span>
        <span class="total-amount">${getTotalAmount().toFixed(2)}</span>
      </div>

      <div class="actions">
        <Button label="Continue Shopping" primary={false} onClick={continueShopping} />
        <Button label="Place Order" onClick={placeOrder} />
      </div>
    </div>
  {/if}
</div>

<style>
  .cart-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
  }

  h1 {
    color: #ff3e00;
    text-align: center;
    margin-bottom: 30px;
  }

  .empty-cart {
    text-align: center;
    padding: 40px;
    background: #f9f9f9;
    border-radius: 8px;
  }

  .empty-cart p {
    margin-bottom: 20px;
    font-size: 1.2rem;
    color: #666;
  }

  .cart-items {
    margin-bottom: 30px;
  }

  .cart-summary {
    background: #f9f9f9;
    padding: 20px;
    border-radius: 8px;
  }

  .total {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 20px;
    font-size: 1.2rem;
  }

  .total-amount {
    font-weight: bold;
    color: #ff3e00;
    font-size: 1.5rem;
  }

  .actions {
    display: flex;
    justify-content: space-between;
  }
</style>
