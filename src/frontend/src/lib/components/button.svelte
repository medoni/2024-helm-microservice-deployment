<script>
  export let label = 'Klicken';
  export let primary = true;
  export let onClick = () => {};
</script>

<button class:primary on:click={onClick}>
  {label}
</button>

<style>
  button {
    padding: 0.75rem 1.5rem;
    border: none;
    border-radius: 4px;
    cursor: pointer;
    font-size: 1rem;
    transition:
      background-color 0.2s,
      transform 0.1s;
  }

  button:hover {
    transform: translateY(-1px);
  }

  button:active {
    transform: translateY(1px);
  }

  .primary {
    background-color: #ff3e00;
    color: white;
  }

  .primary:hover {
    background-color: #ff6340;
  }

  button:not(.primary) {
    background-color: #f1f1f1;
    color: #333;
  }

  button:not(.primary):hover {
    background-color: #e1e1e1;
  }
</style>
