<script>
  import { orderService } from '$lib/services/order-service';
  import OrderCard from '$lib/components/order-cart.svelte';
  import Button from '$lib/components/button.svelte';
  import { goto } from '$app/navigation';
  import { onMount } from 'svelte';
  import LoadingSpinner from '$lib/components/loading-spinner.svelte';
  import ErrorMessage from '$lib/components/error-message.svelte';

  /**
   * @type {import('$lib/services/pizza-ordering-service-api').OrderDto[]}
   */
  let orderItems = [];
  let loading = true;
  let error = '';

  onMount(async () => {
    try {
      loading = true;
      orderItems = await orderService.loadAllOrders();
    } catch (err) {
      error = 'Failed to load orders. Please try again later.';
      console.error(err);
    } finally {
      loading = false;
    }
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

  {#if loading}
    <LoadingSpinner message="Loading your orders..." />
  {:else if error}
    <ErrorMessage message={error} />
  {:else if orderItems.length === 0}
    <div class="empty-orders">
      <p>You haven't placed any orders yet</p>
      <Button label="Browse Menu" onClick={goToMenu} />
    </div>
  {:else}
    <div class="orders-list">
      {#each orderItems as order (order.id)}
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
