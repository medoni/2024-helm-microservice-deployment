<script>
  import Button from '$lib/components/button.svelte';
  import { goto } from '$app/navigation';

  function goToMenu() {
    goto('/menu');
  }
</script>

<svelte:head>
  <title>Pizza Shop | Home</title>
</svelte:head>

<main>
  <section class="hero">
    <div class="hero-content">
      <h1>Fresh & Tasty Pizza</h1>
      <p>Order delicious pizzas online with fast delivery</p>
      <Button label="View Menu" onClick={goToMenu} />
    </div>
  </section>

  <section class="features">
    <div class="feature">
      <div class="icon">🍕</div>
      <h2>Quality Ingredients</h2>
      <p>We use only the freshest ingredients for our pizzas</p>
    </div>

    <div class="feature">
      <div class="icon">🚚</div>
      <h2>Fast Delivery</h2>
      <p>Enjoy your pizza while it's hot with our quick delivery</p>
    </div>

    <div class="feature">
      <div class="icon">💰</div>
      <h2>Best Prices</h2>
      <p>Delicious pizzas at affordable prices</p>
    </div>
  </section>
</main>

<style>
  main {
    width: 100%;
  }

  .hero {
    background:
      linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.7)),
      url('https://images.unsplash.com/photo-1513104890138-7c749659a591?ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80');
    background-size: cover;
    background-position: center;
    color: white;
    padding: 120px 20px;
    text-align: center;
  }

  .hero-content {
    max-width: 800px;
    margin: 0 auto;
  }

  .hero h1 {
    font-size: 3rem;
    margin-bottom: 1rem;
    color: white;
  }

  .hero p {
    font-size: 1.2rem;
    margin-bottom: 2rem;
  }

  .features {
    display: flex;
    flex-wrap: wrap;
    justify-content: center;
    padding: 60px 20px;
    background: #f9f9f9;
    max-width: 1200px;
    margin: 0 auto;
  }

  .feature {
    flex: 1;
    min-width: 250px;
    max-width: 350px;
    margin: 20px;
    padding: 30px;
    text-align: center;
    background: white;
    border-radius: 8px;
    box-shadow: 0 5px 15px rgba(0, 0, 0, 0.05);
    transition: transform 0.3s;
  }

  .feature:hover {
    transform: translateY(-10px);
  }

  .icon {
    font-size: 3rem;
    margin-bottom: 20px;
  }

  .feature h2 {
    margin-bottom: 15px;
    color: #333;
  }
</style>
