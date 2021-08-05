namespace PizzaStore.Models
{
    public class Pizza
    {
        public int PizzaId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Ingredients { get; set; }
    }
}