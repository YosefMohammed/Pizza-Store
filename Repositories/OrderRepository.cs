using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PizzaStore.Models;
using Spectre.Console;

namespace PizzaStore.Repositories
{
    public class OrderRepository : IDisposable
    {
        private const string jsonFilePath = @"./Data/orders.json";
        public List<Order> OrderList { get; set; }

        public Order GetOrderById(int id)
        {
            return OrderList.Where(o => o.Id == id).FirstOrDefault();
        }

        public void AddOrder(Order order)
        {
            order.Id = OrderList.Count + 1;
            OrderList.Add(order);
        }

        public void Dispose()
        {
            Console.WriteLine("Orders dispose");
            JsonManager.SaveJsonFile(OrderList, jsonFilePath);
        }

        public static void DisplayOrderTable(Order order, List<Pizza> pizzas, List<Topping> toppings)
        {
            Console.WriteLine($"Order number {order.Id} Items => {order.Pizzas.Count} | Total => {order.TotalCost}");

            var orderTable = new Table();
            orderTable.AddColumn(new TableColumn("Pizza Number"));
            orderTable.AddColumn(new TableColumn("Name"));
            orderTable.AddColumn(new TableColumn("Price"));

            foreach (var id in order.Pizzas)
            {
                var pizza = pizzas.FirstOrDefault(p => p.Id == id.Id);
                var rows = new List<Markup>
                {
                    new Markup(pizza.Id.ToString()),
                    new Markup(pizza.Name),
                    new Markup(pizza.Price.ToString()),
                };
                orderTable.AddRow(rows);
            }

            var toppingTable = new Table();
            toppingTable.AddColumn(new TableColumn("Topping Number"));
            toppingTable.AddColumn(new TableColumn("Name"));
            toppingTable.AddColumn(new TableColumn("Price"));
            toppingTable.AddColumn(new TableColumn("Pizza Number"));

            foreach (var pizza in order.Pizzas)
            {
                var pizzaId = pizza.Id;

                foreach (var toppingId in pizza.ToppingList)
                {
                    var topping = toppings.FirstOrDefault(t => t.Id == toppingId);

                    var rows = new List<Markup>
                    {
                        new Markup(topping.Id.ToString()),
                        new Markup(topping.Name),
                        new Markup(topping.Price.ToString()),
                        new Markup(pizzaId.ToString()),
                    };
                    toppingTable.AddRow(rows);
                }
            }
            AnsiConsole.Render(orderTable);
            AnsiConsole.Render(toppingTable);
        }
        
        public async Task GetOrdersFromApi()
        {
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync("https://localhost:5001/Order");
            var order = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var res = JsonSerializer.Deserialize<List<Order>>(order, options);
            
             OrderList = res;
        }

        public async Task PostOrderToApi(Order order)
        {
            var httpClient = new HttpClient();
            var body = JsonSerializer.Serialize(order);
            var requestContent = new StringContent(body, Encoding.UTF8, "application/json");
            var httpResponse = await httpClient.PostAsync("https://localhost:5001/Order", requestContent);
        }
    }
}