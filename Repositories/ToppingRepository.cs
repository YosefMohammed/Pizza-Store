using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PizzaStore.Models;
using Spectre.Console;

namespace PizzaStore.Repositories
{
    public class ToppingRepository
    {
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
        
        public async Task GetToppingFromApi()
        {
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync("https://localhost:5001/Topping");
            var topping = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var res = JsonSerializer.Deserialize<List<Topping>>(topping, options);
            
            ToppingList = res;
        }
    }
}