
import { writable } from 'svelte/store';
import { Order } from '../models/order';
import { CartItem } from '$lib/models/cart-item';

class OrderService {
  constructor() {
    /**
     * @type import('svelte/store').Writable<Order[]>
     */
    this.orders = writable([]);

    /**
     * @type number
     */
    this.nextOrderId = 1;
  }

  /**
   *
   * @param {(param: any[]) => void} callback
   * @returns
   */
  subscribe(callback) {
    return this.orders.subscribe(callback);
  }

  /**
   *
   * @param {CartItem[]} cartItems
   * @param {number} totalAmount
   * @returns
   */
  createOrder(cartItems, totalAmount) {
    const orderId = this.nextOrderId++;
    const newOrder = new Order('' + orderId, [...cartItems], totalAmount);

    this.orders.update(orders => {
      return [newOrder, ...orders];
    });

    return newOrder;
  }

  /**
   *
   * @param {string} id
   * @returns {Order?}
   */
  getOrderById(id) {
    let foundOrder = null;
    this.orders.subscribe(orders => {
      foundOrder = orders.find(order => order.id === id);
    })();
    return foundOrder;
  }

  /**
   *
   * @param {string} id
   * @param {string} status
   */
  updateOrderStatus(id, status) {
    this.orders.update(orders => {
      return orders.map(order => {
        if (order.id === id) {
          return { ...order, status };
        }
        return order;
      });
    });
  }
}

export const orderService = new OrderService();
