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
      executor: 'ramping-vus',
      stages: 
      [
        { duration: '30s', target: 50 }, // Gradual ramp-up
        { duration: '1m', target: 100 }, // High load during peak hours
        { duration: '2m', target: 300 }, // Simulating spike traffic
        { duration: '2m', target: 150 }, // Decrease after spike
        { duration: '2m', target: 50 },  // Cool down
      ],
      exec: 'run_create_cart_and_checkout_scenario',
      startTime: '10s'
    }
  },

  thresholds: {
    'http_req_duration{scenario:warmup}': [
      {
        'threshold': 'p(50)<200',
      }
    ],
    'http_req_failed{scenario:warmup}': [
      'rate<0.01'
    ],

    'http_req_duration{scenario:create_cart_and_checkout}': [
      {
        'threshold': 'p(50)<500'
      }
    ],
    'http_req_failed{scenario:create_cart_and_checkout}': [
      'rate<0.1'
    ],
  }
};

const BASE_URL = __ENV.BASE_URL || 'http://pizza-service.dev-local.pos.mycluster.localhost/api';
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
