import { Pizza } from '../models/pizza';

class MenuService {
    constructor() {
      this.pizzas = [
        new Pizza('1', 'Margherita', 'Classic tomato and mozzarella', 8.99, 'EUR', null, ['Tomato Sauce', 'Mozzarella', 'Basil']),
        new Pizza('2', 'Pepperoni', 'Pepperoni and cheese', 9.99, 'EUR', null, ['Tomato Sauce', 'Mozzarella', 'Pepperoni']),
        new Pizza('3', 'Vegetarian', 'Fresh vegetables and cheese', 10.99, 'EUR', null, ['Tomato Sauce', 'Mozzarella', 'Bell Peppers', 'Mushrooms', 'Onions']),
        new Pizza('4', 'Hawaiian', 'Ham and pineapple', 11.99, 'EUR', null, ['Tomato Sauce', 'Mozzarella', 'Ham', 'Pineapple']),
        new Pizza('5', 'BBQ Chicken', 'BBQ sauce and chicken', 12.99, 'EUR', null, ['BBQ Sauce', 'Mozzarella', 'Chicken', 'Red Onion']),
      ];
    }

    getAllPizzas() {
      return [...this.pizzas];
    }

    /**
     *
     * @param {string} id
     * @returns
     */
    getPizzaById(id) {
      return this.pizzas.find(pizza => pizza.id === id);
    }
  }

  export const menuService = new MenuService();
