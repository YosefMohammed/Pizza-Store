using System;
using System.Collections.Generic;
using PizzaStore.Models;
using PizzaStore.Repository;
using Spectre.Console;

namespace PizzaStore
{
    class Program
    {
        static int ReadIntegerInput()
        {
            var res = Console.ReadLine();
            
            if(string.IsNullOrEmpty(res))
            {
                return -1;
            }
            var valid = int.TryParse(res, out var answer);
            return valid ? answer : -1;
        }
        static void Main()
        {
            var pizzaRepository = new PizzaRepository();
            var toppingRepository = new ToppingRepository();
            var orderRepository = new OrderRepository();

            AnsiConsole.WriteLine("Welcome to Pizza Store");
            while (true)
            {
                AnsiConsole.WriteLine("Do you want to make a new order or check your old orders? 1 for new order, 2 for old orders, 0 to exit: ");

                var answer = ReadIntegerInput();
                if(answer == -1) 
                {
                    System.Console.WriteLine("Invalid Input");
                    continue;
                }
                var order = new Order();
                if (answer == 1)
                {
                    while(true) 
                    {
                        //make a new order
                        PizzaRepository.DisplayPizzaTable(pizzaRepository.PizzaList);
                        Console.WriteLine("Please choose your pizza");

                        var orderChoice = ReadIntegerInput();

                        var getPizza = pizzaRepository.GetPizzaById(orderChoice);
                        if(getPizza == null) 
                        {
                            System.Console.WriteLine("This pizza doesn't not exist");
                            continue;
                        }
                        var pizzaWithTopping = new PizzaWithToppings() {
                            PizzaId = getPizza.PizzaId,
                            Toppings = new List<int>()
                        };
                        order.TotalCost += getPizza.Price;

                        while (true)
                        {
                            Console.WriteLine("Do you like to add any toppings on your pizza? y for yes, n for no");

                            //add extra topping
                            var addToppings = Console.ReadLine();
                            if (addToppings.Trim().ToLower() == "y")
                            {
                            
                                //choosing topping 
                                Console.WriteLine("Please choose your extra topping.");
                                ToppingRepository.DisplayToppingTable(toppingRepository.ToppingList);
                                var toppingChoice = ReadIntegerInput();
                                
                                if(toppingChoice == -1)
                                {
                                    System.Console.WriteLine("Invalid input");
                                    continue;
                                }

                                var getTopping = toppingRepository.GetToppingById(toppingChoice);
                                
                                if(getTopping == null)
                                {
                                    System.Console.WriteLine("This topping doesn't exist");
                                    continue;
                                }
                                pizzaWithTopping.Toppings.Add(getTopping.ToppingId);
                                order.TotalCost += getTopping.ToppingPrice;
                            }
                            else
                            {
                                order.Pizzas.Add(pizzaWithTopping);
                                break;
                            }
                        }
                        System.Console.WriteLine($"Pizza count => {order.Pizzas.Count}");
                        Console.WriteLine("Do you like to add another pizza? y for yes, n for no");
                        var addAnotherPizza = Console.ReadLine();

                        if (addAnotherPizza.Trim().ToLower() == "n") 
                            break;
                    }
                    orderRepository.AddOrder(order);
                }
                else if (answer == 2)
                {
                    //view order list
                    foreach(var o in orderRepository.OrderList)
                        OrderRepository.DisplayOrderTable(o);
                }
                else if(answer == 0)
                {
                    System.Console.WriteLine("Goodbey!");
                    orderRepository.Dispose();
                    break;
                }
            }
        }
    }
}