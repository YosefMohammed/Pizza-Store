using System.Collections.Generic;
using System.Linq;
using PizzaStore.Models;
using Spectre.Console;

namespace PizzaStore.Repositories
{
    public class ToppingRepository
    {
        public ToppingRepository()
        {
            ToppingList = JsonManager.ReadJsonFile<IEnumerable<Topping>>(@"./Data/toppings.json");
        }

        public IEnumerable<Topping> ToppingList { get; set; }

        public Topping GetToppingById(int id)
        {
            return ToppingList.Where(t => t.Id == id).FirstOrDefault();
        }

        public static void DisplayToppingTable(IEnumerable<Topping> toppings)
        {
            var toppingTable = new Table();
            toppingTable.AddColumn(new TableColumn("Topping Number"));
            toppingTable.AddColumn(new TableColumn("Name"));
            toppingTable.AddColumn(new TableColumn("Price"));
            foreach (var topping in toppings)
            {
                var rows = new List<Markup>
                {
                    new Markup(topping.Id.ToString()),
                    new Markup(topping.Name),
                    new Markup(topping.Price.ToString()),
                };
                toppingTable.AddRow(rows);
            }

            AnsiConsole.Render(toppingTable);
        }
    }
}