import { Counter } from 'k6/metrics';
import { create_cart_and_checkout_scenario } from './scenarios/create-cart-and-checkout-scenario.js'
let errorCounter = new Counter('errors');

export const options = {
  scenarios: 
  {
    warmup: {
      executor: 'shared-iterations',
      maxDuration: '30s',
      iterations: '30',
      vus: 10,
      startTime: '0s',
      exec: 'run_warmup',
    },

    create_cart_and_checkout: 
    {
      executor: 'shared-iterations',
      maxDuration: '30s',
      iterations: '100',
      vus: 10,
      exec: 'run_create_cart_and_checkout_scenario',
      startTime: '5s'
    }
  },

  thresholds: {
    'http_req_duration{scenario:warmup}': [
      {
        'threshold': 'p(50)<200',
        'abortOnFail': true
      }
    ],
    'http_req_failed{scenario:warmup}': [
      'rate<0.01'
    ],

    'http_req_duration{scenario:create_cart_and_checkout}': [
      {
        'threshold': 'p(50)<500',
        'abortOnFail': false
      }
    ],
    'http_req_failed{scenario:create_cart_and_checkout}': [
      'rate<0.1'
    ],
  }
};

const BASE_URL = __ENV.BASE_URL || 'https://cczvf5kg92.execute-api.eu-central-1.amazonaws.com/api';
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
