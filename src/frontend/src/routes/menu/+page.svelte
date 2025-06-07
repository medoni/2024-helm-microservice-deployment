<script>
  import { menuService } from '$lib/services/menu-service';
  import MenuCard from '$lib/components/menu-item-card.svelte';
  import LoadingSpinner from '$lib/components/loading-spinner.svelte';
  import ErrorMessage from '$lib/components/error-message.svelte';
  import { onMount } from 'svelte';

  /**
   * @type {import('$lib/services/pizza-ordering-service-api').MenuItemDto[]}
   */
  let menuItems = [];
  let loading = true;
  let error = '';

  onMount(async () => {
    try {
      loading = true;
      menuItems = await menuService.getAllMenuItems();
    } catch (err) {
      error = 'Failed to load pizzas. Please try again later.';
      console.error(err);
    } finally {
      loading = false;
    }
  });
</script>

<svelte:head>
  <title>Menu | Pizza Shop</title>
</svelte:head>

<div class="menu-container">
  <h1>Our Menu</h1>

  {#if loading}
    <LoadingSpinner message="Loading our delicious pizzas..." />
  {:else if error}
    <ErrorMessage message={error} />
  {:else}
    <div class="pizza-grid">
      {#each menuItems as item (item.id)}
        <MenuCard {item} />
      {/each}
    </div>
  {/if}
</div>

<style>
  .menu-container {
    max-width: 1200px;
    margin: 0 auto;
    padding: 20px;
  }

  h1 {
    color: #ff3e00;
    text-align: center;
    margin-bottom: 30px;
  }

  .pizza-grid {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
    gap: 20px;
  }
</style>
