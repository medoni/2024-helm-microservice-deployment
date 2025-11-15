import { configService } from './config-service';
import { generateUUID } from '../utils/uuid';

/**
 * Service to communicate with the Pizza Ordering API
 */
class PizzaOrderingServiceApi {
  constructor() {
    // Base URL for API calls - dynamically loaded from config
    this.baseUrl = configService.getConfig('pizzaApiUrl');
    /**
     * @type {Record<string, string>}
     */
    this.headers = {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    };
  }

  /**
   * Set the authentication token for API requests
   * @param {string} token - JWT or other auth token
   */
  setAuthToken(token) {
    if (token) {
      this.headers['Authorization'] = `Bearer ${token}`;
    } else {
      delete this.headers['Authorization'];
    }
  }

  /**
   * @param {string} url
   * @param {object} options
   * General fetch handler with error management
   */
  async fetchWithErrorHandling(url, options) {
    try {
      const response = await fetch(url, options);

      // Parse JSON response if present
      const contentType = response.headers.get('content-type');
      const data =
        contentType && contentType.includes('application/json')
          ? await response.json()
          : await response.text();

      // Handle API errors
      if (!response.ok) {
        const error = {
          status: response.status,
          statusText: response.statusText,
          data,
        };
        throw error;
      }

      return data;
    } catch (error) {
      console.error('API request failed:', error);
      throw error;
    }
  }

  /**
   * @returns {Promise<MenuDto>}
   */
  async getActiveMenu() {
    return this.fetchWithErrorHandling(`${this.baseUrl}/v1/Menu/active`, {
      method: 'GET',
      headers: this.headers,
    });
  }

  /**
   * @returns {Promise<string>}
   */
  async createCart() {
    const createDto = {
      id: generateUUID(),
      requestedAt: new Date(),
    };

    await this.fetchWithErrorHandling(`${this.baseUrl}/v1/Cart/`, {
      method: 'POST',
      headers: this.headers,
      body: JSON.stringify(createDto),
    });

    return createDto.id;
  }

  /**
   *
   * @param {string} cartId
   * @returns {Promise<CartDto?>}
   */
  async getCartById(cartId) {
    const cart = await this.fetchWithErrorHandling(`${this.baseUrl}/v1/Cart/${cartId}`, {
      method: 'GET',
      headers: this.headers,
    });

    return cart;
  }

  /**
   *
   * @param {string} cartId
   * @returns {Promise<CartItemDto[]>}
   */
  async getCartItemsById(cartId) {
    let items = [];
    let paginationToken = null;

    for (;;) {
      const resultset = await this.fetchWithErrorHandling(
        `${this.baseUrl}/v1/Cart/${cartId}/items?token=${paginationToken || ''}`,
        {
          method: 'GET',
          headers: this.headers,
        }
      );

      items = [...resultset.data];
      paginationToken = resultset.nextPageToken;
      if (!paginationToken) break;
    }

    return items;
  }

  /**
   *
   * @param {string} cartId
   * @param {string} menuItemId
   * @param {number} newQuantity
   * @returns {Promise<CartItemDto>}
   */
  async patchCartItem(cartId, menuItemId, newQuantity) {
    const patchDto = {
      menuItemId: menuItemId,
      requestedAt: new Date(),
      quantity: newQuantity,
    };

    return this.fetchWithErrorHandling(`${this.baseUrl}/v1/Cart/${cartId}/items`, {
      method: 'PATCH',
      headers: this.headers,
      body: JSON.stringify(patchDto),
    });
  }

  /**
   *
   * @param {string} cartId
   * @returns {Promise<CartCheckedOutDto>}
   */
  async cartCheckout(cartId) {
    const checkoutDto = { checkoutAt: new Date() };
    const result = this.fetchWithErrorHandling(`${this.baseUrl}/v1/Cart/${cartId}/checkout`, {
      method: 'POST',
      headers: this.headers,
      body: JSON.stringify(checkoutDto),
    });

    return result;
  }

  /**
   *
   * @param {string} orderId
   * @returns {Promise<OrderDto>}
   */
  async getOrderById(orderId) {
    const result = this.fetchWithErrorHandling(`${this.baseUrl}/v1/Order/${orderId}`, {
      method: 'GET',
      headers: this.headers,
    });

    return result;
  }
}

// Create and export a singleton instance
export const pizzaOrderingApi = new PizzaOrderingServiceApi();

/**
 * @typedef AddItemDto
 * @property {string} menuItemId - UUID of the menu item
 * @property {string} requestedAt - Date and time when the item was requested
 * @property {number} quantity - Quantity of the item
 */

/**
 * @typedef CartCheckOutDto
 * @property {string} checkoutAt - Date and time of checkout
 */

/**
 * @typedef CartCheckedOutDto
 * @property {string} cartId - UUID of the cart
 * @property {string} orderId - UUID of the created order
 * @property {string} checkedOutAt - Date and time when the cart was checked out
 */

/**
 * @typedef CartCheckoutInfoDto
 * @property {string} checkedOutAt - Date and time when the cart was checked out
 * @property {string} orderId - UUID of the created order
 */

/**
 * @typedef CartDto
 * @property {string} id - UUID of the cart
 * @property {string} createdAt - Date and time when the cart was created
 * @property {string} lastChangedAt - Date and time when the cart was last changed
 * @property {string} state - Current state of the cart (Created, CheckedOut)
 * @property {string} activeMenuId - UUID of the active menu
 * @property {number} totalItems - Total number of items in the cart
 * @property {GrossNetPriceDto} totalPrice - Total price of all items in the cart
 * @property {CartCheckoutInfoDto} checkoutInfo - Information about the checkout
 */

/**
 * @typedef CartItemDto
 * @property {string} id - UUID of the cart item
 * @property {string} addedAt - Date and time when the item was added to the cart
 * @property {string} menuItemId - Id of the corresponding menu item
 * @property {string} name - Name of the item
 * @property {string} description - Description of the item
 * @property {PriceInfoDto} unitPrice - Price information for a single unit
 * @property {number} quantity - Quantity of the item
 */

/**
 * @typedef CartItemDtoResultSetDto
 * @property {CartItemDto[]} data - Collection of cart items
 * @property {string} nextPageToken - Token to retrieve the next page of results
 * @property {string} previousPageToken - Token to retrieve the previous page of results
 * @property {Object.<string, string>} metadata - Additional metadata about the request or result
 */

/**
 * @typedef CreateCartDto
 * @property {string} id - UUID of the cart to create
 * @property {string} requestedAt - Date and time when the cart was requested
 */

/**
 * @typedef CreateMenuDto
 * @property {string} id - UUID of the menu to create
 * @property {string} currency - Currency code for the menu prices
 * @property {MenuSectionDto[]} sections - Sections of the menu
 */

/**
 * @typedef GrossNetPriceDto
 * @property {number} gross - Gross price amount
 * @property {number} net - Net price amount
 * @property {number} vat - VAT amount
 * @property {string} currency - Currency code
 */

/**
 * @typedef MenuDto
 * @property {string} id - UUID of the menu
 * @property {string} createdAt - Date and time when the menu was created
 * @property {string} lastChangedAt - Date and time when the menu was last changed
 * @property {MenuSectionDto[]} sections - Sections of the menu
 * @property {boolean} isActive - Whether the menu is currently active
 * @property {string} activatedAt - Date and time when the menu was activated
 */

/**
 * @typedef MenuDtoResultSetDto
 * @property {MenuDto[]} data - Collection of menus
 * @property {string} nextPageToken - Token to retrieve the next page of results
 * @property {string} previousPageToken - Token to retrieve the previous page of results
 * @property {Object.<string, string>} metadata - Additional metadata about the request or result
 */

/**
 * @typedef MenuItemDto
 * @property {string} id - UUID of the menu item
 * @property {string} name - Name of the menu item
 * @property {PriceInfoDto} price - Price information for the menu item
 * @property {string} description - Description of the menu item
 * @property {string[]} ingredients - List of ingredients
 */

/**
 * @typedef MenuSectionDto
 * @property {string} id - UUID of the menu section
 * @property {string} name - Name of the menu section
 * @property {MenuItemDto[]} items - Items in the menu section
 */

/**
 * @typedef OrderDto
 * @property {string} id - UUID of the order
 * @property {string} createdAt - Date and time when the order was created
 * @property {string} lastChangedAt - Date and time when the order was last changed
 * @property {string} state - Current state of the order
 * @property {OrderItemDto[]} items - Items in the order
 * @property {OrderPriceSummaryDto} priceSummary - Price summary of the order
 */

/**
 * @typedef OrderItemDto
 * @property {string} itemId - UUID of the order item
 * @property {string} cartItemId - UUID of the corresponding cart item
 * @property {string} name - Name of the item
 * @property {string} description - Description of the item
 * @property {PriceInfoDto} unitPrice - Price information for a single unit
 * @property {number} quantity - Quantity of the item
 * @property {GrossNetPriceDto} totalPrice - Total price for this item
 */

/**
 * @typedef OrderPriceSummaryDto
 * @property {GrossNetPriceDto} totalItemPrice - Total price of all items
 * @property {GrossNetPriceDto} totalPrice - Total price including delivery and discounts
 * @property {GrossNetPriceDto} deliveryCosts - Delivery costs
 * @property {GrossNetPriceDto} discount - Applied discount
 */

/**
 * @typedef PriceInfoDto
 * @property {GrossNetPriceDto} price - Price information
 * @property {number} regularyVatInPercent - Regular VAT percentage
 */

/**
 * @typedef ProblemDetailsDto
 * @property {string} type - URI reference that identifies the problem type
 * @property {string} title - Short, human-readable summary of the problem
 * @property {number} status - HTTP status code
 * @property {string} detail - Human-readable explanation of the problem
 * @property {string} instance - URI reference that identifies the specific occurrence of the problem
 */

/**
 * @typedef UpdateItemDto
 * @property {string} menuItemId - UUID of the menu item
 * @property {string} requestedAt - Date and time when the update was requested
 * @property {number} quantity - New quantity of the item
 */

/**
 * @typedef UpdateMenuDto
 * @property {string} id - UUID of the menu to update
 * @property {MenuSectionDto[]} sections - Updated sections of the menu
 */
