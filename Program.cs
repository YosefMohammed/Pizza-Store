using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PizzaStore.Models;
using PizzaStore.Repositories;
using Spectre.Console;

namespace PizzaStore
{
    class Program
    {
        static async Task Main()
        {
            var pizzaRepository = new PizzaRepository();
            var toppingRepository = new ToppingRepository();
            var orderRepository = new OrderRepository();

            await pizzaRepository.GetPizzaFromApi();
            await toppingRepository.GetToppingFromApi();
            await orderRepository.GetOrdersFromApi();
            
            
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

                        if (AnsiConsole.Confirm("Do you want to add another pizza? y for [green]Yes[/] n for [red]No[/]"))
                            continue;
                        else
                            break;
                    }
                    await orderRepository.PostOrderToApi(order);
                    // orderRepository.AddOrder(order);
                }

                else if (answer == 2)
                {
                    //Viewing order list
                    await orderRepository.GetOrdersFromApi();
                    foreach (var o in orderRepository.OrderList)
                        OrderRepository.DisplayOrderTable(o, pizzaRepository.PizzaList.ToList(), toppingRepository.ToppingList.ToList());
                    
                }

                else if (answer == 0)
                {
                    //Good bye
                    AnsiConsole.WriteLine("Good Bye!");
                    break;
                }
                
            }
        }

        
    }
}