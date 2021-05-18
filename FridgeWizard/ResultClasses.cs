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
            Console.WriteLine(title);
            Console.ResetColor();
            Console.WriteLine("\t" + ingredients + "\n");
        }
    }

    public class UserQuery
    {
        public string userIngredients { get; set; }
        public string userTerm { get; set; }

        public UserQuery(string userIng, string userTer)
        {
            userIngredients = userIng;
            userTerm = userTer;
        }
    }

}
