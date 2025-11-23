import { writable, get } from 'svelte/store';
import { pizzaOrderingApi as api } from './pizza-ordering-service-api';
import { orderService } from './order-service';

const CART_ID_STORAGE_KEY = 'pizza-ordering-cart-id';

class CartService {
  constructor() {
    this.cartId = '';

    /**
     * @type import('svelte/store').Writable<import('./pizza-ordering-service-api').CartItemDto[]>
     */
    this.cartStore = writable([]);
  }

  /**
   *
   * @param {(param: import('./pizza-ordering-service-api').CartItemDto[]) => void} callback
   * @returns
   */
  subscribe(callback) {
    return this.cartStore.subscribe(callback);
  }

  async ensureCartIsCreated() {
    if (this.cartId) return;
    if (await this.tryLoadExistingCart()) return;

    const createdId = await api.createCart();
    this.cartId = createdId;
    localStorage.setItem(CART_ID_STORAGE_KEY, this.cartId);
    this.cartStore.set([]);
  }

  async tryLoadExistingCart() {
    const cartId = localStorage.getItem(CART_ID_STORAGE_KEY) || '';
    if (cartId) {
      try {
        await this.loadCartById(cartId);
        this.cartId = cartId;

        return true;
      } catch (ex) {
        console.log(ex);
      }
    }

    return false;
  }

  ignoreLoadCartById = 0;
  /**
   *
   * @param {string} id
   */
  async loadCartById(id) {
    if (this.ignoreLoadCartById) return;

    ++this.ignoreLoadCartById;
    try {
      const items = await api.getCartItemsById(id);

      this.cartId = id;
      localStorage.setItem(CART_ID_STORAGE_KEY, id);
      this.cartStore.set(items);
    } finally {
      --this.ignoreLoadCartById;
    }
  }

  /**
   *
   * @returns {Promise<import('./pizza-ordering-service-api').CartDto?>}
   */
  async loadFullCart() {
    if (!(await this.tryLoadExistingCart())) return null;

    const cart = await api.getCartById(this.cartId);

    return cart;
  }

  /**
   *
   * @param {import('./pizza-ordering-service-api').MenuItemDto} item
   * @param {number} quantity
   */
  async addToCart(item, quantity = 1) {
    await this.ensureCartIsCreated();

    const existingItems = get(this.cartStore),
      existingItem = existingItems.find((x) => x.menuItemId === item.id),
      newQuantity = existingItem ? existingItem.quantity + quantity : quantity;

    var cartItem = await api.patchCartItem(this.cartId, item.id, newQuantity);

    this.cartStore.update((items) => {
      const existingItemIndex = items.findIndex((x) => x.menuItemId === cartItem.menuItemId);

      if (existingItemIndex >= 0) {
        const updatedItems = [...items];
        updatedItems[existingItemIndex].quantity += quantity;
        return updatedItems;
      } else {
        return [...items, cartItem];
      }
    });
  }

  /**
   *
   * @param {string} menuItemId
   */
  async removeFromCart(menuItemId) {
    await this.ensureCartIsCreated();

    await api.patchCartItem(this.cartId, menuItemId, 0);

    this.cartStore.update((items) => items.filter((item) => item.menuItemId !== menuItemId));
  }

  /**
   *
   * @param {string} menuItemId
   * @param {number} quantity
   * @returns
   */
  async updateQuantity(menuItemId, quantity) {
    await this.ensureCartIsCreated();

    if (quantity <= 0) {
      await this.removeFromCart(menuItemId);
      return;
    }

    var cartItem = await api.patchCartItem(this.cartId, menuItemId, quantity);

    this.cartStore.update((items) => {
      return items.map((item) => {
        if (item.menuItemId === menuItemId) {
          return cartItem;
        }
        return item;
      });
    });
  }

  async clearCart() {
    this.cartId = '';
    localStorage.removeItem(CART_ID_STORAGE_KEY);
    this.cartStore.update(() => []);
  }

  /**
   * @returns {Promise<import('./pizza-ordering-service-api').CartCheckedOutDto>}
   */
  async checkoutCart() {
    const checkedOutDto = await orderService.createOrder(this.cartId);
    this.clearCart();

    return checkedOutDto;
  }

  getTotalAmount() {
    let total = 0;
    this.cartStore.subscribe((items) => {
      total = items.reduce((sum, item) => sum + item.quantity * item.unitPrice.price.gross, 0);
    })();
    return total;
  }
}

export const cartService = new CartService();
