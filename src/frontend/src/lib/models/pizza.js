export class Pizza {
  /**
   * @param {string} id
   * @param {string} name
   * @param {string} description
   * @param {number} price
   * @param {string} currency
   * @param {string[]} [ingredients=[]]
   */
  constructor(id, name, description, price, currency, imageUrl = null, ingredients = []) {
    this.id = id;
    this.name = name;
    this.description = description;
    this.price = price;
    this.currency = currency;
    this.imageUrl =
      imageUrl || `https://dummyimage.com/150/000/fff.png&text=${encodeURIComponent(name)}`;
    this.ingredients = ingredients;
  }
}
