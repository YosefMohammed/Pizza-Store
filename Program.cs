using System.Collections.Generic;
using PizzaStore.Models;
using PizzaStore.Repositories;
using Spectre.Console;

namespace PizzaStore
{
    class Program
    {
        static void Main()
        {
            var pizzaRepository = new PizzaRepository();
            var toppingRepository = new ToppingRepository();
            var orderRepository = new OrderRepository();

            AnsiConsole.WriteLine("Welcome to pizza store!");
            while (true)
            {
                var answer = AnsiConsole.Prompt(
                    new TextPrompt<int>("Do you want to make a new order or check your old orders? 1 for new order, 2 for old orders, 0 to exit")
                        .Validate(input =>
                        {
                            return input switch
                            {
                                < 0 => ValidationResult.Error("[red]Invalid input[/]"),
                                > 2 => ValidationResult.Error("[red]Invalid input[/]"),
                                _ => ValidationResult.Success(),
                            };
                        }));

                var order = new Order();

                if (answer == 1)
                {
                    //Making an order
                    while (true)
                    {
                        PizzaRepository.DisplayPizzaTable(pizzaRepository.PizzaList);

                        var choosingPizza = AnsiConsole.Prompt(
                            new TextPrompt<int>("Please choose your pizza")
                                .Validate(pizzaId =>
                                    pizzaRepository.GetPizzaById(pizzaId) == null ? 
                                        ValidationResult.Error("[red]Invalid input[/]") : ValidationResult.Success()));

                        var getPizza = pizzaRepository.GetPizzaById(choosingPizza);

                        var pizzaWithToppings = new PizzaWithTopping()
                        {
                            Id = getPizza.Id,
                            ToppingList = new List<int>()
                        };
                        
                        order.TotalCost += getPizza.Price;

                        while (true)
                        {
                            //Adding topping
                            var addTopping = AnsiConsole.Confirm("Do you want to add any topping on your pizza? y for [green]Yes[/], n for [red]No[/]");

                            if (addTopping)
                            {
                                ToppingRepository.DisplayToppingTable(toppingRepository.ToppingList);

                                var chooseTopping = AnsiConsole.Prompt(
                                    new TextPrompt<int>("Please Choose your extra topping.")
                                        .Validate(toppingId =>
                                            toppingRepository.GetToppingById(toppingId) == null ? 
                                                ValidationResult.Error("[red]Invalid input[/]") : ValidationResult.Success()));

                                var getTopping = toppingRepository.GetToppingById(chooseTopping);

                                pizzaWithToppings.ToppingList.Add(getTopping.Id);
                                order.TotalCost += getTopping.Price;
                                order.Toppings.Add(getTopping);
                            }
                            else
                            {
                                order.Pizzas.Add(pizzaWithToppings);
                                break;
                            }
                        }

                        AnsiConsole.WriteLine($"Pizza count => {order.Pizzas.Count}");

                        // AnsiConsole.Prompt(new TextPrompt<String>("[grey]Press any key to go to main menu[/]").AllowEmpty()); 
                        if (AnsiConsole.Confirm("Do you want to add another pizza? y for [green]Yes[/] n for [red]No[/]"))
                            continue;
                        else
                            break;
                    }

                    orderRepository.AddOrder(order);
                }

                else if (answer == 2)
                {
                    //Viewing order list
                    foreach (var o in orderRepository.OrderList)
                        OrderRepository.DisplayOrderTable(o);
                }

                else if (answer == 0)
                {
                    //Good bye
                    AnsiConsole.WriteLine("Good Bye!");
                    orderRepository.Dispose();
                    break;
                }
            }
        }
    }
}