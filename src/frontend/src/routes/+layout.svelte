<script>
  import '../app.css';
</script>

<div class="app">
  <header>
    <nav>
      <a href="/">Home</a>
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
  }

  nav a:hover {
    text-decoration: underline;
  }

  main {
    flex: 1;
  }

  footer {
    padding: 1rem;
    background-color: #f8f8f8;
    text-align: center;
    font-size: 0.8rem;
  }
</style>
