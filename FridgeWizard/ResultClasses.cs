using System;
using System.Collections.Generic;
using System.Text;

namespace FridgeWizard
{

    public class Rootobject
    {
        public string title { get; set; }
        public float version { get; set; }
        public string href { get; set; }
        public Result[] results { get; set; }
    }

    public class Result
    {
        public string title { get; set; }
        public string href { get; set; }
        public string ingredients { get; set; }
        public string thumbnail { get; set; }

        public void PrintRecipe(ConsoleColor titleColor)
        {
            Console.ForegroundColor = titleColor;
            string titleTrimmed = title.Trim(' ');
            titleTrimmed = titleTrimmed.Replace("\n", "").Replace("\r", "").Replace("\t", "");
            Console.WriteLine(titleTrimmed);
            Console.ResetColor();
            Console.WriteLine("\t" + ingredients + "\n");
        }
    }

    public class UserQuery
    {
        public string userIngredients { get; set; }
        public string userTerm { get; set; }

        //This prints the instructions and creates the query object later used to create the api url string
        public void MakeQuery()
        {
            // Grab ingredients
            Console.WriteLine("Provide all the stuff you got in a comma-separated list, and we'll see if we can't make magic happen.");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("for example: onions, tomato, eggs, butter, spinach");
            Console.ResetColor();

            string userIng = Console.ReadLine();
            userIng = userIng.Replace(' ', '+'); // Remove spaces

            /* May not need
            userIng = userIng.Insert(0, "%2B"); // Add + to beginning of string, + denoted required ingredient
            userIng = userIng.Replace(",", "%2C%2B"); // Add other + */

            userIngredients = userIng;

            // Grab search term
            Console.WriteLine("Want to try for a specific keyword? You skip this by pressing enter.");
            string userTer = Console.ReadLine();
            userTer = userTer.Replace(' ', '+');
            userTerm = userTer;

        }
    }

}
