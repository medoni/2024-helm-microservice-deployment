import { expect } from '@playwright/test';

export class App {
  /**
   * @param {import("@playwright/test").Page} page
   */
  constructor(page) {
    this.page = page;
  }

  async getCartCount() {
    const elem = this.page.locator('header .cart-link .cart-badge');
    const innerText = (await elem.innerText()) || '';

    return +(innerText.trim());
  }

  async expectCartItemCount(expectedCount) {
    await expect(async () => {
      expect(await this.getCartCount()).toBe(expectedCount);
    }).toPass({
      intervals: [300, 1_000],
      timeout: 3_000
    })
  }

  async clickCartMenu() {
    this.page.click('header .cart-link:has-text("Cart")');
  }
}
