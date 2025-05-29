
import { get, writable } from 'svelte/store';
import { pizzaOrderingApi as api } from './pizza-ordering-service-api';

const ORDER_STORAGE_KEY = "pizza-ordering-order-ids"

class OrderService {
  constructor() {
    /**
     * @type {import('svelte/store').Writable<string[]>}
     */
    this.orderIds = writable([]);
    this.loadOrderIds();
  }

  async loadOrderIds() {
    const rawOrderIds = localStorage.getItem(ORDER_STORAGE_KEY) || '[]';
    const orderIds = JSON.parse(rawOrderIds);

    this.orderIds.update(_ => orderIds);
  }

  async storeOrderIds() {
    const orderIds = get(this.orderIds);
    localStorage.setItem(ORDER_STORAGE_KEY, JSON.stringify(orderIds));
  }

  /**
   *
   * @param {(param: string[]) => void} callback
   * @returns
   */
  subscribe(callback) {
    return this.orderIds.subscribe(callback);
  }

  /**
   *
   * @param {string} cartId
   * @returns {Promise<import('./pizza-ordering-service-api').CartCheckedOutDto>}
   */
  async createOrder(cartId) {
    const checkoutDto = await api.cartCheckout(cartId);
    const orderId = checkoutDto.orderId;
    this.orderIds.update(ids => [orderId, ...ids]);
    this.storeOrderIds();
    return checkoutDto;
  }

  /**
   *
   * @param {string} orderId
   * @returns {Promise<import('./pizza-ordering-service-api').OrderDto>}
   */
  async getOrderById(orderId) {
    const orderDto = await api.getOrderById(orderId);
    return orderDto;
  }

  /**
   * @returns {Promise<import('./pizza-ordering-service-api').OrderDto[]>}
   */
  async loadAllOrders() {
    const orderIds = get(this.orderIds);

    const promisses = orderIds.map(id => this.getOrderById(id));
    const result = await Promise.all(promisses);

    return result;
  }
}

export const orderService = new OrderService();
