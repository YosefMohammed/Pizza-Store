using System.Collections.Generic;
using System.Linq;
using PizzaStore.Models;
using Spectre.Console;

namespace PizzaStore.Repository
{
    public class PizzaRepository
    {
        public PizzaRepository()
        {
            PizzaList = JsonManager.ReadJsonFile<IEnumerable<Pizza>>(@"./Data/pizza.json");
        }
        
        public IEnumerable<Pizza> PizzaList {get;set;}

        public Pizza GetPizzaById(int id)
        {
            return PizzaList.Where(p => p.PizzaId == id).FirstOrDefault();
        }

        public static void DisplayPizzaTable(IEnumerable<Pizza> pizzas) 
        {
            var pizzaTable = new Table();
            pizzaTable.AddColumn(new TableColumn("Pizza Number"));
            pizzaTable.AddColumn(new TableColumn("Name"));
            pizzaTable.AddColumn(new TableColumn ("Ingredients"));
            pizzaTable.AddColumn(new TableColumn("Price"));
            foreach(var pizza in pizzas) 
            {
                var rows = new List<Markup>
                {
                    new Markup(pizza.PizzaId.ToString()),
                    new Markup(pizza.Name),
                    new Markup(pizza.Ingredients.ToString()),
                    new Markup(pizza.Price.ToString()),
                };
                pizzaTable.AddRow(rows);
            }
            AnsiConsole.Render(pizzaTable);
        }
    }
}