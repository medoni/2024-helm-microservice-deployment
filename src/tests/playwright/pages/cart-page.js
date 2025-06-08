import { expect } from '@playwright/test';

export class CartPage {
    /**
     * @param {import("@playwright/test").Page} page
     */
    constructor(page) {
      this.page = page;
    }

    async expectToBeVisible() {
      await expect(this.page.locator('.cart-container h1:text("Your Cart")')).toBeVisible();
    }

    /**
     * @param {object[]} items
     * @param {string} items.title
     * @param {number?} items.qty
     */
    async expectCartItems(items) {
      for (let i=0; i<items.length; ++i) {
        await this.expectCartItem(items[i]);
      }
    }

    /**
     * @param {object} item
     * @param {string} item.title
     * @param {number?} item.qty
     */
    async expectCartItem(item) {
      const cartItemLoc = this.page.locator(`.cart-item:has(h3:text("${item.title}"))`);
      const qtyLoc = cartItemLoc.locator('.quantity>span');

      await expect(cartItemLoc).toBeVisible();

      if (item.qty !== undefined) {
        await expect(qtyLoc).toBeVisible();
        const text = (await qtyLoc.innerText()) || '';
        expect(+text).toBe(item.qty);
      }
    }

    /**
     * @param {string} cartItemTitle
     */
    async increaseCount(cartItemTitle) {
      const cartItemLoc = this.page.locator(`.cart-item:has(h3:text("${cartItemTitle}"))`);
      const cartItemIncreaseLoc = cartItemLoc.locator('.quantity button:text("+")');

      await cartItemIncreaseLoc.click();
    }

    async clickPlaceOrder() {
      const buttonLoc = this.page.locator('.actions button:text("Place Order")');
      await buttonLoc.click();
    }
  }
