export class Order {
    /**
     *
     * @param {string} id
     * @param {any[]} items
     * @param {number} totalAmount
     * @param {string} status
     * @param {Date} date
     */
    constructor(id, items, totalAmount, status = 'pending', date = new Date()) {
      this.id = id;
      this.items = items;
      this.totalAmount = totalAmount;
      this.status = status;
      this.date = date;
    }
  }
