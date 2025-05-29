import { writable } from 'svelte/store';
import { CartItem } from '../models/cart-item';
import { Pizza } from '$lib/models/pizza';

class CartService {
  constructor() {
    /**
     * @type import('svelte/store').Writable<CartItem[]>
     */
    this.cartStore = writable([]);
  }

  /**
   *
   * @param {(param: CartItem[]) => void} callback
   * @returns
   */
  subscribe(callback) {
    return this.cartStore.subscribe(callback);
  }

    /**
     *
     * @param {Pizza} pizza
     * @param {number} quantity
     */
  addToCart(pizza, quantity = 1) {
    this.cartStore.update(items => {
      const existingItemIndex = items.findIndex(item => item.pizza.id === pizza.id);

      if (existingItemIndex >= 0) {
        const updatedItems = [...items];
        updatedItems[existingItemIndex].quantity += quantity;
        return updatedItems;
      } else {
        return [...items, new CartItem(pizza, quantity)];
      }
    });
  }

  /**
   *
   * @param {*} pizzaId
   */
  removeFromCart(pizzaId) {
    this.cartStore.update(items => items.filter(item => item.pizza.id !== pizzaId));
  }

  /**
   *
   * @param {string} pizzaId
   * @param {number} quantity
   * @returns
   */
  updateQuantity(pizzaId, quantity) {
    if (quantity <= 0) {
      this.removeFromCart(pizzaId);
      return;
    }

    this.cartStore.update(items => {
      return items.map(item => {
        if (item.pizza.id === pizzaId) {
          return new CartItem(item.pizza, quantity);
        }
        return item;
      });
    });
  }

  clearCart() {
    this.cartStore.set([]);
  }

  getTotalAmount() {
    let total = 0;
    this.cartStore.subscribe(items => {
      total = items.reduce((sum, item) => sum + item.totalPrice, 0);
    })();
    return total;
  }
}

export const cartService = new CartService();
