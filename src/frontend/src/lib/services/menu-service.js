import { pizzaOrderingApi as api } from './pizza-ordering-service-api'

class MenuService {
  constructor() {
    /**
     * @type {import('./pizza-ordering-service-api').MenuItemDto[]}
     */
    this.menuItems = [];

    /**
     * @type {import('./pizza-ordering-service-api').MenuSectionDto[]}
     */
    this.menuSections = [];
  }

  async ensureItemsAreLoaded() {
    if (this.menuItems && this.menuItems.length) return;

    const menuDto = await api.getActiveMenu();
    this.menuSections = menuDto.sections;

    this.menuItems = this.menuSections.map(section => section.items).reduce((res, items) => res.concat(items), [])
  }

  /**
   * @returns {Promise<import('./pizza-ordering-service-api').MenuSectionDto[]>}
   */
  async getAllSections() {
    await this.ensureItemsAreLoaded();

    return [...this.menuSections];
  }

  /**
   * @returns {Promise<import('./pizza-ordering-service-api').MenuItemDto[]>}
   */
  async getAllMenuItems() {
    await this.ensureItemsAreLoaded();

    return [...this.menuItems];
  }

  /**
   *
   * @param {string} id
   * @returns {Promise<import('./pizza-ordering-service-api').MenuItemDto?>}
   */
  async getMenuItemById(id) {
    await this.ensureItemsAreLoaded();

    return this.menuItems.find(item => item.id === id) ?? null;
  }
}

export const menuService = new MenuService();
