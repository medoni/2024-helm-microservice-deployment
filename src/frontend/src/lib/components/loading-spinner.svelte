<script>
  export let size = '40px';
  export let color = '#ff3e00';
  export let message = 'Loading...';
</script>

<div class="spinner-container">
  <div
    class="spinner"
    style="width: {size}; height: {size}; border-color: {color} transparent {color} transparent;"
  ></div>
  {#if message}
    <p class="message">{message}</p>
  {/if}
</div>

<style>
  .spinner-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
    padding: 20px 0;
  }

  .spinner {
    border: 4px solid;
    border-radius: 50%;
    animation: spin 1.5s linear infinite;
  }

  .message {
    margin-top: 10px;
    font-size: 1rem;
    color: #666;
  }

  @keyframes spin {
    0% {
      transform: rotate(0deg);
    }
    100% {
      transform: rotate(360deg);
    }
  }
</style>
