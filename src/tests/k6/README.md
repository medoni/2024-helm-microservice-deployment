# K6 Performance Tests

## Running Tests

### Smoke Test
```bash
k6 run smoke-create-cart-and-checkout.js
```

### Load Test with Spikes
```bash
k6 run load-high-order-rate-with-spikes.js
```

## Configuration

### Custom Base URL
```bash
k6 run --env BASE_URL=http://localhost:5000 smoke-create-cart-and-checkout.js
```

### Run Specific Scenario
```bash
k6 run --env SCENARIO=functional_check smoke-create-cart-and-checkout.js
```

### Adjust Request Delay
```bash
k6 run --env SLOWMO=0.5 smoke-create-cart-and-checkout.js
```
   