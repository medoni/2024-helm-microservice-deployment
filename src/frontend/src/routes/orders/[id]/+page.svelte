<script>
  import { orderService } from '$lib/services/order-service';
  import Button from '$lib/components/button.svelte';
  import { goto } from '$app/navigation';
  import { onMount } from 'svelte';
  import LoadingSpinner from '$lib/components/loading-spinner.svelte';
  import ErrorMessage from '$lib/components/error-message.svelte';
  import { page } from '$app/state'

  const orderId = page.params.id;
  /** @type {import('$lib/services/pizza-ordering-service-api').OrderDto?} */
  let order = null;
  let loading = true;
  let error = '';

  onMount(async () => {
    try {
      loading = true;
      order = await orderService.getOrderById(orderId);
    } catch (err) {
      error = 'Failed to load order. Please try again later.';
      console.error(err);
    } finally {
      loading = false;
    }
  });

  // Format date
  /**
   *
   * @param {string | Date} date
   */
  const dateFormatter = date => new Date(date).toLocaleDateString();

  function goBack() {
    goto('/orders');
  }
</script>

<svelte:head>
  <title>Order #{orderId} | Pizza Shop</title>
</svelte:head>

<div class="order-detail-container">
  <button class="back-button" on:click={goBack}>← Back to Orders</button>

  {#if loading}
    <LoadingSpinner message="Loading you order..." />
  {:else if error}
    <ErrorMessage message={error} />
  {:else}
    {#if !order}
      <div class="not-found">
        <h2>Order not found</h2>
        <p>The order you're looking for doesn't exist.</p>
        <Button label="View All Orders" onClick={goBack} />
      </div>
    {:else}
      <div class="order-header">
        <h1>Order #{order.id}</h1>
        <div class="order-meta">
          <p class="date">Placed on: {dateFormatter(order.createdAt)}</p>
          <p class="status">Status: <span class="status-badge">{order.state}</span></p>
        </div>
      </div>

      <div class="order-items">
        <h2>Order Items</h2>
        <table>
          <thead>
            <tr>
              <th>Item</th>
              <th>Price</th>
              <th>Quantity</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody>
            {#each order.items as item}
              <tr>
                <td>{item.name}</td>
                <td>{item.unitPrice.price.gross.toFixed(2)} {item.unitPrice.price.currency}</td>
                <td>{item.quantity}</td>
                <td>{item.totalPrice.gross.toFixed(2)} {item.totalPrice.currency}</td>
              </tr>
            {/each}
          </tbody>
        </table>
      </div>

      <div class="order-summary">
        <div class="total">
          <span>Total Amount:</span>
          <span class="total-amount">{order.priceSummary.totalPrice.gross.toFixed(2)} {order.priceSummary.totalPrice.currency}</span>
        </div>
      </div>
    {/if}
  {/if}


</div>

<style>
  .order-detail-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
  }

  .back-button {
    background: none;
    border: none;
    color: #666;
    cursor: pointer;
    margin-bottom: 20px;
    padding: 0;
  }

  .back-button:hover {
    color: #ff3e00;
  }

  .not-found {
    text-align: center;
    padding: 40px;
    background: #f9f9f9;
    border-radius: 8px;
  }

  h1 {
    color: #ff3e00;
    margin-bottom: 10px;
  }

  .order-meta {
    margin-bottom: 30px;
    color: #666;
  }

  .date, .status {
    margin: 5px 0;
  }

  .status-badge {
    display: inline-block;
    padding: 3px 8px;
    border-radius: 4px;
    text-transform: capitalize;
    font-weight: bold;
    background: #f0f0f0;
  }

  h2 {
    margin-bottom: 15px;
    border-bottom: 1px solid #eee;
    padding-bottom: 5px;
  }

  table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 30px;
  }

  th, td {
    padding: 10px;
    text-align: left;
    border-bottom: 1px solid #eee;
  }

  th {
    background: #f9f9f9;
  }

  .order-summary {
    background: #f9f9f9;
    padding: 20px;
    border-radius: 8px;
  }

  .total {
    display: flex;
    justify-content: space-between;
    align-items: center;
    font-size: 1.2rem;
  }

  .total-amount {
    font-weight: bold;
    color: #ff3e00;
    font-size: 1.5rem;
  }
</style>
