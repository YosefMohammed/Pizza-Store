using System.Collections.Generic;

namespace PizzaStore.Models
{
    public class Topping
    {
        public int ToppingId { get; set; }
        public string ToppingName { get; set; }
        public decimal ToppingPrice { get; set; }
    }
}