import { Counter } from 'k6/metrics';
import { create_cart_and_checkout_scenario } from './scenarios/create-cart-and-checkout-scenario.js'
let errorCounter = new Counter('errors');

const scenarios = {
  functional_check: {
    executor: 'shared-iterations',
    vus: 9,
    iterations: 24,
    exec: 'run_create_cart_and_checkout_scenario',
  }
};

export const options = {
  scenarios: {}
};

if (__ENV.SCENARIO) {
  options.scenarios[__ENV.SCENARIO] = scenarios[__ENV.SCENARIO];
} else {
  options.scenarios = scenarios;
}

const BASE_URL = __ENV.BASE_URL || 'https://m9s12tlge2.execute-api.eu-central-1.amazonaws.com/api';
const SLOWMO = parseFloat(__ENV.SLOWMO || '0.1');

export function run_warmup() {
  const testContext = {
    baseUrl: BASE_URL,
    slowMo: SLOWMO,
    counters: {
      error: errorCounter
    }
  };

  create_cart_and_checkout_scenario(testContext);
}

export function run_create_cart_and_checkout_scenario() {
  const testContext = {
    baseUrl: BASE_URL,
    slowMo: SLOWMO,
    counters: {
      error: errorCounter
    }
  };

  create_cart_and_checkout_scenario(testContext);
}
