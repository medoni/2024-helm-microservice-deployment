<script>
  import Button from '$lib/components/button.svelte';
  const title = 'Willkommen bei meiner Webanwendung';
  const subtitle = 'Einfache SvelteKit-Anwendung mit PNPM';

  function handleClick() {
    alert('Hallo! Willkommen bei unserer App.');
  }
</script>

<main>
  <section class="welcome-section">
    <h1>{title}</h1>
    <p>{subtitle}</p>
    <Button label="Klick mich" onClick={handleClick} />
  </section>
</main>

<style>
  main {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    min-height: 100vh;
    text-align: center;
    padding: 1rem;
  }

  .welcome-section {
    max-width: 600px;
  }

  h1 {
    color: #ff3e00;
    font-size: 2.5rem;
    margin-bottom: 1rem;
  }

  p {
    margin-bottom: 2rem;
    font-size: 1.2rem;
  }

  button {
    background-color: #ff3e00;
    color: white;
    border: none;
    padding: 0.75rem 1.5rem;
    border-radius: 4px;
    cursor: pointer;
    font-size: 1rem;
    transition: background-color 0.2s;
  }

  button:hover {
    background-color: #ff6340;
  }
</style>
