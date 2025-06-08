import { test, expect } from '@playwright/test';
import { App } from '../pages/app.js';
import { WelcomePage } from '../pages/welcome-page.js';
import { MenuPage } from '../pages/menu-page.js';
import { CartPage } from '../pages/cart-page.js';
import { OrderPage } from '../pages/order-page.js';

test('Complete pizza order flow', async ({ page }) => {
  // Pages
  const app = new App(page);
  const welcomePage = new WelcomePage(page);
  const menuPage = new MenuPage(page);
  const cartPage = new CartPage(page);
  const orderPage = new OrderPage(page);

  // Step 1: Open app and navigate to menu
  await page.goto('/');
  await welcomePage.expectToBeVisible();
  await welcomePage.clickViewMenu();

  // Step 2: Add Pizza
  await menuPage.clickAddToCart('Pizza Four Seasons');
  await app.expectCartItemCount(1);

  // Step 3: Add Cola
  await menuPage.clickAddToCart('Coca Cola');
  await app.expectCartItemCount(2);

  // Step 4: Go to cart
  await app.clickCartMenu();
  await cartPage.expectToBeVisible();

  // Step 5: verify cart
  await cartPage.expectCartItems([
    { title: 'Pizza Four Seasons', qty: 1 },
    { title: 'Coca Cola', qty: 1 },
  ]);

  // Step 6: increase count of pizza
  await cartPage.increaseCount('Pizza Four Seasons');
  await app.expectCartItemCount(3);

  // Step 7: verify cart
  await cartPage.expectCartItems([
    { title: 'Pizza Four Seasons', qty: 2 },
    { title: 'Coca Cola', qty: 1 },
  ]);

  // Step 8: place order
  await cartPage.clickPlaceOrder();

  // Step 9: verify Order
  await orderPage.expectToBeVisible();
  await orderPage.expectOrderItems([
    { title: 'Pizza Four Seasons', qty: 2 },
    { title: 'Coca Cola', qty: 1 },
  ]);
  await orderPage.expectOrderSummary({
    total: v => expect(v.qty).toBeGreaterThan(10)
  });
});
