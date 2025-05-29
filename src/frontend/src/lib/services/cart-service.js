import { writable } from 'svelte/store';
import { CartItem } from '../models/cart-item';
import { pizzaOrderingApi as api } from './pizza-ordering-service-api';

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

    const createdId = await api.createCart();
    this.cartId = createdId;
  }

  /**
   *
   * @param {import('./pizza-ordering-service-api').MenuItemDto} item
   * @param {number} quantity
   */
  async addToCart(item, quantity = 1) {
    await this.ensureCartIsCreated();

    var cartItem = await api.patchCartItem(
      this.cartId,
      item.id,
      quantity
    )

    this.cartStore.update(items => {
      const existingItemIndex = items.findIndex(x => x.id === cartItem.id);

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

    await api.patchCartItem(
      this.cartId,
      menuItemId,
      0
    )

    this.cartStore.update(items => items.filter(item => item.menuItemId !== menuItemId));
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

    var cartItem = await api.patchCartItem(
      this.cartId,
      menuItemId,
      quantity
    )

    this.cartStore.update(items => {
      return items.map(item => {
        if (item.menuItemId === menuItemId) {
          return cartItem;
        }
        return item;
      });
    });
  }

  async clearCart() {
    this.cartId = '';
    this.cartStore.set([]);
    await this.ensureCartIsCreated();
  }

  getTotalAmount() {
    let total = 0;
    this.cartStore.subscribe(items => {
      total = items.reduce((sum, item) => sum + (item.quantity * item.unitPrice.price.gross), 0);
    })();
    return total;
  }
}

export const cartService = new CartService();
