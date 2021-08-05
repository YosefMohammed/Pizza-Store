using System.Collections.Generic;

namespace PizzaStore.Models
{
    public class PizzaWithToppings
    {
        public int PizzaId { get; set; }
        public List<int> Toppings { get; set; } = new List<int>();
    }
}