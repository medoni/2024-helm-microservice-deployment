import { expect } from '@playwright/test';

export class OrderPage {
  /**
   * @param {import("@playwright/test").Page} page
   */
  constructor(page) {
    this.page = page;

    this.orderDetailContainerLoc = page.locator('.order-detail-container');
  }

  async expectToBeVisible() {
    await expect(this.orderDetailContainerLoc).toBeVisible();
  }

  /**
   * @param {object[]} items
   * @param {string} items.title
   * @param {number?} items.qty
   */
  async expectOrderItems(items) {
    for (let i=0; i<items.length; ++i) {
      await this.expectOrderItem(items[i]);
    }
  }

  /**
   * @param {object} item
   * @param {string} item.title
   * @param {number?} item.qty
   */
  async expectOrderItem(item) {
    const orderItemLoc = this.page.locator(`.order-items tr:has(td:text("${item.title}"))`);
    const qtyLoc = orderItemLoc.locator(':nth-child(3)');

    await expect(orderItemLoc).toBeVisible();

    if (item.qty !== undefined) {
      await expect(qtyLoc).toBeVisible();
      const text = (await qtyLoc.innerText()) || '';
      expect(+text).toBe(item.qty);
    }
  }

  /**
   * @param {object} expectedSummary
   * @param {function({qty: number, currency: string})} expectedSummary.total
   */
  async expectOrderSummary(expectedSummary) {
    const orderSummaryLoc = this.page.locator('.order-summary');
    const totalLoc = orderSummaryLoc.locator('.total-amount');

    const totalText = await totalLoc.innerText();
    const totalResult = this.parseAmountWithCurrency(totalText);

    expectedSummary.total(totalResult)
  }

  /**
   * @param {string} text
   */
  parseAmountWithCurrency(text) {
    const match = text.match(/^([\d.]+)\s+([A-Z]+)$/);
    if (!match) throw new Error("Invalid format");

    return {
      qty: parseFloat(match[1]),
      currency: match[2]
    };
  }
}
