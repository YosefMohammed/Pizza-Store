using System;
using System.Collections.Generic;
using System.Linq;
using PizzaStore.Models;
using Spectre.Console;

namespace PizzaStore.Repositories
{
    public class OrderRepository : IDisposable
    {
        private const string jsonFilePath = @"./Data/orders.json";

        public OrderRepository()
        {
            OrderList = JsonManager.ReadJsonFile<List<Order>>(jsonFilePath);
        }

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
            JsonManager.SaveJsonFile(OrderList, jsonFilePath);
        }

        public static void DisplayOrderTable(Order order)
        {
            var pizzaRepository = new PizzaRepository();
            var toppingRepository = new ToppingRepository();
            var pizzaWithToppings = new PizzaWithTopping();

            Console.WriteLine($"Order number {order.Id} Items => {order.Pizzas.Count} | Total => {order.TotalCost}");

            var orderTable = new Table();
            orderTable.AddColumn(new TableColumn("Pizza Number"));
            orderTable.AddColumn(new TableColumn("Name"));
            orderTable.AddColumn(new TableColumn("Price"));

            foreach (var id in order.Pizzas)
            {
                var pizza = pizzaRepository.PizzaList.FirstOrDefault(p => p.Id == id.Id);
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
                    var topping = toppingRepository.ToppingList.FirstOrDefault(t => t.Id == toppingId);

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
    }
}