using DbServices;
using System;

namespace Bulletin_Bored
{
    class Program
    {

        static void Main(string[] args)
        {

            var choice = WelcomePage();

            if (choice == "Create Account")
            {
                Console.Write("Choose User Name: ");
                var userName = Console.ReadLine();

                Console.Write("Choose Password: ");
                var password = Console.ReadLine();

                DbUpdate.CreateUser(userName, password);
            }
            else if (choice == "Sign In")
            {

            }

            Console.ReadLine();
        }

        static string ShowMenu(string prompt, string[] options)
        {
            Console.WriteLine(prompt);

            int selected = 0;

            // Hide the cursor that will blink after calling ReadKey.
            Console.CursorVisible = false;

            ConsoleKey? key = null;
            while (key != ConsoleKey.Enter)
            {
                // If this is not the first iteration, move the cursor to the first line of the menu.
                if (key != null)
                {
                    Console.CursorLeft = 0;
                    Console.CursorTop = Console.CursorTop - options.Length;
                }

                // Print all the options, highlighting the selected one.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == selected)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    Console.WriteLine("- " + option);
                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    selected = Math.Min(selected + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    selected = Math.Max(selected - 1, 0);
                }
            }

            // Reset the cursor and perform the selected action.
            Console.CursorVisible = true;
            Console.Clear();
            return options[selected];
        }

        static string WelcomePage()
        {
            var options = new string[] { "Sign In", "Create Account" };
            return ShowMenu("\n\t\tWelcome to Bulletin Bored - for when you've got nothing better to do!\n\n", options);
        }
    }
}
