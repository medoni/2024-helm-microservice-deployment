<script>
  import { orderService } from '$lib/services/order-service';
  import OrderCard from '$lib/components/order-cart.svelte';
  import Button from '$lib/components/button.svelte';
  import { goto } from '$app/navigation';

  /**
   * @type any[]
   */
  let orders = [];

  const unsubscribe = orderService.subscribe(allOrders => {
    orders = allOrders;
  });

  function goToMenu() {
    goto('/menu');
  }
</script>

<svelte:head>
  <title>My Orders | Pizza Shop</title>
</svelte:head>

<div class="orders-container">
  <h1>My Orders</h1>

  {#if orders.length === 0}
    <div class="empty-orders">
      <p>You haven't placed any orders yet</p>
      <Button label="Browse Menu" onClick={goToMenu} />
    </div>
  {:else}
    <div class="orders-list">
      {#each orders as order (order.id)}
        <OrderCard {order} />
      {/each}
    </div>
  {/if}
</div>

<style>
  .orders-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
  }

  h1 {
    color: #ff3e00;
    text-align: center;
    margin-bottom: 30px;
  }

  .empty-orders {
    text-align: center;
    padding: 40px;
    background: #f9f9f9;
    border-radius: 8px;
  }

  .empty-orders p {
    margin-bottom: 20px;
    font-size: 1.2rem;
    color: #666;
  }
</style>
