import http from 'k6/http';
import { check, sleep } from 'k6';
import { Counter } from 'k6/metrics';

import { uuidv4 } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';
import { randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { randomItem } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

export let errorCounter = new Counter('errors');



export const options = {
  stages: [
    { duration: '30s', target: 100 }, // Gradual ramp-up
    { duration: '1m', target: 500 }, // High load during peak hours
    { duration: '2m', target: 1000 }, // Simulating spike traffic
    { duration: '2m', target: 500 }, // Decrease after spike
    { duration: '2m', target: 100 }, // Cool down
  ],
  // stages: [
  //   { duration: '2m', target: 1 }
  // ]
};

const BASE_URL = __ENV.BASE_URL || 'http://pizza-service.dev-local.pos.mycluster.localhost/api/v1'; // 'http://localhost:5000/v1' ||

export default function test() {
  const testContext = {
    baseUrl: BASE_URL,
    counters: {
      error: errorCounter
    }
  };

  create_and_checkout_cart_scenario(testContext);
}

function create_and_checkout_cart_scenario(
  testContext
) 
{
  const activeMenu = getMenu(testContext);
  const cart = createCart(testContext);
  sleep(1);

  fillCart(testContext, activeMenu, cart.id);
  sleep(1);

  checkoutCart(testContext, cart.id, {
    checkoutAt: new Date()
  });
}

function getMenu(testContext) 
{
  let response = http.get(`${testContext.baseUrl}/Menu/active`);
  check(response, {
    'menu fetched successfully': (res) => res.status === 200,
  }) || testContext.counters.error.add(1);

  return JSON.parse(response.body);
}

function createCart(testContext) 
{
  let data = {
    id: uuidv4(),
    requestedAt: new Date()
  };
  let response = http.post(`${testContext.baseUrl}/Cart`, JSON.stringify(data), { headers: { 'Content-Type': 'application/json' } });
  check(response, {
    'cart created successfully': (res) => res.status === 204,
  }) || testContext.counters.error.add(1);

  return data;
}

function fillCart(testContext, menu, cartId) 
{
  const menuItems = menu.sections.flatMap(x => x.items);
  
  const cartItemsToCreate = randomIntBetween(1, 10);
  for(let i=0; i<cartItemsToCreate; ++i) {
    var menuItem = randomItem(menuItems);
    addItemToCart(
      testContext,
      cartId,
      {
        menuItemId: menuItem.id,
        requestedAt: new Date(),
        quantity: randomItem(1, 5)
      }
    )
    sleep(1);
  }
}

function addItemToCart(testContext, cartId, cartItem) 
{
  let data = cartItem;
  let response = http.post(`${testContext.baseUrl}/Cart/${cartId}/items`, JSON.stringify(data), { headers: { 'Content-Type': 'application/json' } });
  check(response, {
    'cart item successfully added': (res) => res.status === 204,
  }) || testContext.counters.error.add(1);
}

function checkoutCart(testContext, cartId, checkoutData) 
{
  let data = checkoutData;
  let response = http.post(`${testContext.baseUrl}/Cart/${cartId}/checkout`, JSON.stringify(data), { headers: { 'Content-Type': 'application/json' } });
  check(response, {
    'cart successfully checked out': (res) => res.status === 200,
  }) || testContext.counters.error.add(1);

  return JSON.parse(response.body);
}