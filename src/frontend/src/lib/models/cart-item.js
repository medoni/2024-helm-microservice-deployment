import { Pizza } from "./pizza";

export class CartItem {
    /**
     *
     * @param {Pizza} pizza
     * @param {number} quantity
     */
    constructor(pizza, quantity = 1) {
      this.pizza = pizza;
      this.quantity = quantity;
    }

    get totalPrice() {
      return this.pizza.price * this.quantity;
    }
  }
