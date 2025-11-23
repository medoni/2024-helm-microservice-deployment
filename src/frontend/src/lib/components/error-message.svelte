<script>
  export let message = 'Something went wrong.';
  export let showRetry = true;
  export let retryAction = () => window.location.reload();
</script>

<div class="error-container">
  <div class="error-icon">
    <svg
      width="50"
      height="50"
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      stroke-width="2"
      stroke-linecap="round"
      stroke-linejoin="round"
    >
      <circle cx="12" cy="12" r="10"></circle>
      <line x1="12" y1="8" x2="12" y2="12"></line>
      <line x1="12" y1="16" x2="12.01" y2="16"></line>
    </svg>
  </div>
  <p class="message">{message}</p>
  {#if showRetry}
    <button class="retry-button" on:click={retryAction}>Try Again</button>
  {/if}
</div>

<style>
  .error-container {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    width: 100%;
    padding: 30px 0;
    text-align: center;
  }

  .error-icon {
    color: #d32f2f;
    margin-bottom: 15px;
  }

  .message {
    font-size: 1.1rem;
    color: #d32f2f;
    margin-bottom: 15px;
  }

  .retry-button {
    background-color: #ff3e00;
    color: white;
    border: none;
    padding: 8px 16px;
    border-radius: 4px;
    cursor: pointer;
    font-weight: 500;
    transition: background-color 0.2s;
  }

  .retry-button:hover {
    background-color: #e43700;
  }
</style>
