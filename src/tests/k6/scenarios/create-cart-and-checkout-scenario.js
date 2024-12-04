import http from 'k6/http';
import { check, sleep } from 'k6';

import { uuidv4 } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';
import { randomIntBetween } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';
import { randomItem } from 'https://jslib.k6.io/k6-utils/1.2.0/index.js';

export function create_cart_and_checkout_scenario(
    testContext
  ) 
  {
    const activeMenu = getMenu(testContext);
    const cart = createCart(testContext);
    sleep(1 * testContext.slowMo);
  
    fillCart(testContext, activeMenu, cart.id);
    sleep(1 * testContext.slowMo);
  
    checkoutCart(testContext, cart.id, {
      checkoutAt: new Date()
    });
  }
  
  export function getMenu(testContext) 
  {
    let response = http.get(`${testContext.baseUrl}/v1/Menu/active`);
    check(response, {
      'menu fetched successfully': (res) => res.status === 200,
    }) || testContext.counters.error.add(1);
  
    return JSON.parse(response.body);
  }
  
  export function createCart(testContext) 
  {
    let data = {
      id: uuidv4(),
      requestedAt: new Date()
    };
    let response = http.post(`${testContext.baseUrl}/v1/Cart`, JSON.stringify(data), { headers: { 'Content-Type': 'application/json' } });
    check(response, {
      'cart created successfully': (res) => res.status === 204,
    }) || testContext.counters.error.add(1);
  
    return data;
  }
  
  export function fillCart(testContext, menu, cartId) 
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
      sleep(1 * testContext.slowMo);
    }
  }
  
  export function addItemToCart(testContext, cartId, cartItem) 
  {
    let data = cartItem;
    let response = http.post(`${testContext.baseUrl}/v1/Cart/${cartId}/items`, JSON.stringify(data), { headers: { 'Content-Type': 'application/json' } });
    check(response, {
      'cart item successfully added': (res) => res.status === 204,
    }) || testContext.counters.error.add(1);
  }
  
  export function checkoutCart(testContext, cartId, checkoutData) 
  {
    let data = checkoutData;
    let response = http.post(`${testContext.baseUrl}/v1/Cart/${cartId}/checkout`, JSON.stringify(data), { headers: { 'Content-Type': 'application/json' } });
    check(response, {
      'cart successfully checked out': (res) => res.status === 200,
    }) || testContext.counters.error.add(1);
  
    return JSON.parse(response.body);
  }