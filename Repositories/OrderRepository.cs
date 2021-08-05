using System.Collections.Generic;
using PizzaStore.Models;
using System.Linq;
using System;
using Spectre.Console;

namespace PizzaStore.Repository
{
    public class OrderRepository : IDisposable
    {
        private const string jsonFilePath = @"./Data/orders.json";
        public OrderRepository()
        {
            OrderList = JsonManager.ReadJsonFile<List<Order>>(jsonFilePath);
        }
        public List<Order> OrderList {get;set;}

        public Order GetOrderById(int id)
        {
            return OrderList.Where(o => o.OrderId == id).FirstOrDefault();
        }

        public void AddOrder(Order order)
        {
            order.OrderId = OrderList.Count + 1;
            OrderList.Add(order);
        }

        public void Dispose()
        {
            System.Console.WriteLine("Orders dispose");
            JsonManager.SaveJsonFile(OrderList, jsonFilePath);
        }

        public static void DisplayOrderTable(Order order) 
        {
             var pizzaRepository = new PizzaRepository();
            var toppingRepository = new ToppingRepository();
            Console.WriteLine($"Order number {order.OrderId} Items => {order.Pizzas.Count} | Total => {order.TotalCost}");
            var orderTable = new Table();
            orderTable.AddColumn(new TableColumn("Pizza Number"));
            orderTable.AddColumn(new TableColumn("Name"));
            orderTable.AddColumn(new TableColumn("Price"));
            foreach(var id in order.Pizzas) 
            {
                var pizza = pizzaRepository.PizzaList.FirstOrDefault(p => p.PizzaId == id.PizzaId);
                var rows = new List<Markup>
                {
                    new Markup(pizza.PizzaId.ToString()),
                    new Markup(pizza.Name),
                    new Markup(pizza.Price.ToString()),
                };
                orderTable.AddRow(rows);
            }
            AnsiConsole.Render(orderTable);
        }
    }
}