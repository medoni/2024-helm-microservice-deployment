<script>
  export let order;

  // Format date
  const formattedDate = new Date(order.date).toLocaleDateString('en-US', {
    year: 'numeric',
    month: 'short',
    day: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  });

  // Status color mapping
  const statusColors = {
    'pending': '#FFA500',
    'preparing': '#3498db',
    'ready': '#2ecc71',
    'delivered': '#27ae60',
    'cancelled': '#e74c3c'
  };
</script>

<a href={`/orders/${order.id}`} class="order-card">
  <div class="order-header">
    <h3>Order #{order.id}</h3>
    <span class="date">{formattedDate}</span>
  </div>

  <div class="order-status">
    <span class="status-indicator" style="background-color: {statusColors[order.status] || '#999'}"></span>
    <span class="status-text">{order.status.charAt(0).toUpperCase() + order.status.slice(1)}</span>
  </div>

  <div class="order-summary">
    <span>{order.items.length} item{order.items.length !== 1 ? 's' : ''}</span>
    <span class="total">${order.totalAmount.toFixed(2)}</span>
  </div>
</a>

<style>
  .order-card {
    display: block;
    padding: 15px;
    border: 1px solid #ddd;
    border-radius: 8px;
    margin-bottom: 15px;
    text-decoration: none;
    color: inherit;
    transition: transform 0.2s, box-shadow 0.2s;
    background: white;
  }

  .order-card:hover {
    transform: translateY(-3px);
    box-shadow: 0 4px 10px rgba(0,0,0,0.1);
  }

  .order-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 10px;
  }

  h3 {
    margin: 0;
    color: #333;
  }

  .date {
    font-size: 0.8rem;
    color: #666;
  }

  .order-status {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
  }

  .status-indicator {
    display: inline-block;
    width: 10px;
    height: 10px;
    border-radius: 50%;
    margin-right: 8px;
  }

  .order-summary {
    display: flex;
    justify-content: space-between;
  }

  .total {
    font-weight: bold;
    color: #ff3e00;
  }
</style>
