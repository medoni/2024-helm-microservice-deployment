
<script>
  import { menuService } from '$lib/services/menu-service';
  import PizzaCard from '$lib/components/pizza-card.svelte';

  const pizzas = menuService.getAllPizzas();
</script>

<svelte:head>
  <title>Menu | Pizza Shop</title>
</svelte:head>

<div class="menu-container">
  <h1>Our Pizza Menu</h1>

  <div class="pizza-grid">
    {#each pizzas as pizza (pizza.id)}
      <PizzaCard {pizza} />
    {/each}
  </div>
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
