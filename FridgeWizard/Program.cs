using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace FridgeWizard
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        // Method to send out the request to the Recipe Puppy API to retrieve recipes
        // API takes i as comma-delimited list of ingredients, q is a search term, p is a specific page from the results
        private static async Task<Result[]> ProcessRequest(string userIngredients, string userTerm, int page)
        {
            client.DefaultRequestHeaders.Accept.Clear();

            Task<System.IO.Stream> stringTask;
            Rootobject result = new Rootobject();
            try { 
               stringTask = client.GetStreamAsync("http://www.recipepuppy.com/api/?i=" + userIngredients + "&q=" + userTerm + "&p=" + page);
               result = await JsonSerializer.DeserializeAsync<Rootobject>(await stringTask); 
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
            }

            return result.results;
        }

        

        private static void PrintRecipeMenu(Result[] recipes, int index, int page)
        {
            Console.WriteLine("Select a recipe that speaks to you with the Up and Down arrow keys, then press Enter to open the recipe in your default browser. Change pages with Left and Right.");
            Console.WriteLine("If it all looks bad, press R to retry, or Escape to exit entirely.\n");
        
            // First check that there were results found from query
            if(recipes == null)
            {
                Console.WriteLine("No more recipes found!");
            }
            else if(recipes.Length == 0)
            {
                Console.WriteLine("No recipes found!");
            }
            else
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("PAGE " + page);
                Console.WriteLine("-------------------------");
                for (int i = 0; i < recipes.Length; i++)
                {
                    if (i == index)
                        recipes[i].PrintRecipe(ConsoleColor.Blue);
                    else
                        recipes[i].PrintRecipe(ConsoleColor.Green);
                }
            }
        }

        static async Task Main(string[] args)
        {
            // Print out initial intro
            Console.WriteLine("Uh oh, not able to get out to the grocery store today?\nThink you can't make anything to eat?\nThink again!");

            //Get user input, we save it as an object here for later searches that increment pages
            var query = new UserQuery();
            query.MakeQuery();

            // Make api call with provided input
            var recipes = await ProcessRequest(query.userIngredients, query.userTerm, 1);

            ConsoleKeyInfo pressedKey;
            int index = 0, page = 1;

            Console.Clear();
            PrintRecipeMenu(recipes, index, page);

            // Controls for recipe selection menu
            do
            {
                pressedKey = Console.ReadKey();
                switch (pressedKey.Key)
                {
                    case ConsoleKey.DownArrow:
                        if(recipes != null && index != recipes.Length - 1)
                        {
                            Console.Clear();
                            index++;
                            PrintRecipeMenu(recipes, index, page);
                        }                        
                        break;
                    case ConsoleKey.UpArrow:
                        if (recipes != null && index != 0)
                        {
                            Console.Clear();
                            index--;
                            PrintRecipeMenu(recipes, index, page);
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if(page != 1)
                        {
                            page--;
                            index = 0;
                            recipes = await ProcessRequest(query.userIngredients, query.userTerm, page);
                            Console.Clear();
                            PrintRecipeMenu(recipes, index, page);
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        // TODO, ERROR: api occasionally 500's for no reason... so technically there are more pages after "problem" pages
                        // But for now we will pretend that there are no more pages to view :)
                        if (recipes != null && page > 0)
                        {
                            page++;
                            index = 0;
                            recipes = await ProcessRequest(query.userIngredients, query.userTerm, page);
                            Console.Clear();
                            PrintRecipeMenu(recipes, index, page);
                        }
                        break;
                    case ConsoleKey.Enter:
                        var psi = new ProcessStartInfo
                        {
                            FileName = recipes[index].href,
                            UseShellExecute = true
                        };
                        Process.Start(psi);
                        break;
                    case ConsoleKey.R:
                        Console.Clear();
                        query.MakeQuery();
                        recipes = await ProcessRequest(query.userIngredients, query.userTerm, 1);
                        Console.Clear();
                        index = 0;
                        page = 1;
                        PrintRecipeMenu(recipes, index, page);
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
            while (pressedKey.Key != ConsoleKey.Escape);
        }
    }
}
