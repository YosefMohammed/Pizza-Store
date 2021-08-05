using System.Collections.Generic;

namespace PizzaStore.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public decimal TotalCost { get; set; }
    
        public List<PizzaWithToppings> Pizzas { get; set; } = new List<PizzaWithToppings>();        
    }
}