using System.Collections.Generic;

namespace PizzaStore.Models
{
    public class Order
    {
        public int Id { get; set; }
        public List<PizzaWithTopping> Pizzas { get; set; } = new List<PizzaWithTopping>();
        public List<Topping> Toppings { get; set; } = new List<Topping>();
        public decimal TotalCost { get; set; }
    }
}