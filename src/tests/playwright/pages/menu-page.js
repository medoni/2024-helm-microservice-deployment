export class MenuPage {
  /**
   * @param {import("@playwright/test").Page} page
   */
  constructor(page) {
    this.page = page;
  }

  async clickAddToCart(
    cardTitle,
    cardSelector,
    timeout
  ) {
    cardSelector = cardSelector || '.menu-card';
    timeout = timeout || 5000;

    const specificCardSelector = `${cardSelector}:has(h3:text("${cardTitle}")) button`;
    await this.page.click(specificCardSelector, { timeout });
  }
}
