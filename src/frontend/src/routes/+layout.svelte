<script>
  import '../app.css';
  import { cartService } from '$lib/services/cart-service';
  import { onMount } from 'svelte';

  let cartItemsCount = 0;

  onMount(async () => {
    cartService.tryLoadExistingCart();
  });

  const unsubscribe = cartService.subscribe(items => {
    cartItemsCount = items.reduce((count, item) => count + item.quantity, 0);
  });
</script>

<div class="app">
  <header>
    <nav>
      <a href="/">Home</a>
        <a href="/menu">Menu</a>
        <a href="/cart" class="cart-link">
          Cart
          {#if cartItemsCount > 0}
            <span class="cart-badge">{cartItemsCount}</span>
          {/if}
        </a>
        <a href="/orders">My Orders</a>
    </nav>
  </header>

  <main>
    <slot />
  </main>

  <footer>
    <p>© {new Date().getFullYear()} - Meine Webanwendung</p>
  </footer>
</div>

<style>
  .app {
    display: flex;
    flex-direction: column;
    min-height: 100vh;
  }

  header {
    background-color: #f8f8f8;
    padding: 1rem;
    box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  }

  nav a {
    color: #ff3e00;
    text-decoration: none;
    font-weight: bold;
    margin-right: 5rem;
  }

  nav a:hover {
    text-decoration: underline;
  }

  main {
    flex: 1;
  }

  .cart-link {
    position: relative;
  }

  .cart-badge {
    position: absolute;
    top: -8px;
    right: -22px;
    background: #ff3e00;
    color: white;
    border-radius: 50%;
    width: 20px;
    height: 20px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 0.8rem;
  }

  footer {
    padding: 1rem;
    background-color: #f8f8f8;
    text-align: center;
    font-size: 0.8rem;
  }
</style>
