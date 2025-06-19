import { expect } from '@playwright/test';

export class WelcomePage {
  /**
   * @param {import("@playwright/test").Page} page
   */
  constructor(page) {
    this.page = page;
  }

  async expectToBeVisible() {
    await expect(async () => {
      expect(await this.page.title()).toBe('Pizza Shop | Home');
    }).toPass({
      timeout: [300, 1_000, 1_500]
    })
  }

  async clickViewMenu() {
    await this.page.click('button:has-text("View Menu")');
  }
}
