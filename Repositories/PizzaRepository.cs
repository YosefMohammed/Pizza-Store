using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using PizzaStore.Models;
using Spectre.Console;

namespace PizzaStore.Repositories
{
    public class PizzaRepository
    {
        
        public IEnumerable<Pizza> PizzaList {get; set;}
        
        public Pizza GetPizzaById(int id)
        {
            return PizzaList.Where(p => p.Id == id).FirstOrDefault();
        }
        
        public static void DisplayPizzaTable(IEnumerable<Pizza> pizzas)
        {
            // var repository = new PizzaRepository();
            var pizzaTable = new Table();
            pizzaTable.AddColumn(new TableColumn("Pizza Number"));
            pizzaTable.AddColumn(new TableColumn("Name"));
            pizzaTable.AddColumn(new TableColumn ("Ingredients"));
            pizzaTable.AddColumn(new TableColumn("Price"));
            foreach(var pizza in pizzas) 
            {
                var rows = new List<Markup>
                {
                    new Markup(pizza.Id.ToString()),
                    new Markup(pizza.Name),
                    new Markup(pizza.Ingredients.ToString()),
                    new Markup(pizza.Price.ToString()),
                };
                pizzaTable.AddRow(rows);
            }
            AnsiConsole.Render(pizzaTable);
        }

        public async Task GetPizzaFromApi()
        {
            var httpClient = new HttpClient();
            var httpResponse = await httpClient.GetAsync("https://localhost:5001/Pizza");
            var pizza = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var res = JsonSerializer.Deserialize<List<Pizza>>(pizza, options);
            
            PizzaList = res;
        }
    }
}