using DbServices;
using System;

namespace Bulletin_Bored
{
    class Program
    {
        private static string currentUser;
        private static int currentUserID;
        private static bool isSignedIn;
        private static string welcomeText;
        static void Main(string[] args)
        {

            var choice = WelcomePage();



            WelcomePageChoiceSwitch(choice);
            currentUserID = DbUpdate.GetUserId(currentUser);

            while (isSignedIn)
            {
                var task = MainMenu();
                switch (task)
                {
                    case "Most Recent Posts":
                        ShowRecentPosts();
                        break;
                    case "Most Popular Posts":
                        ShowMostPopular();
                        break;
                    case "Posts by Category":
                        break;
                    case "Search":
                        break;
                    case "Create a Post":
                        CreateNewPost();
                        break;
                    case "Quit":
                        Environment.Exit(0);
                        break;
                }
            }
        }

        static void WelcomePageChoiceSwitch(string choice)
        {
            string task = string.Empty;

            if (choice == "Create Account")
            {
                Console.Write("Choose User Name: ");
                var userName = Console.ReadLine();

                Console.Write("Choose Password: ");
                var password = Console.ReadLine();

                DbUpdate.CreateUser(userName, password);
                Console.Clear();
                Console.WriteLine("New User created\n");
                isSignedIn = SignIn();
            }
            else if (choice == "Sign In")
            {
                isSignedIn = SignIn();
            }
        }

        static bool SignIn()
        {
            string task = string.Empty;
            bool userCheck = false;
            while (!userCheck)
            {
                Console.Write("Enter User Name: ");
                var userName = Console.ReadLine();

                Console.Write("Enter Password: ");
                var password = Console.ReadLine();

                userCheck = DbUpdate.CheckUser(userName, password);

                if (userCheck == true)
                {
                    Console.Clear();
                    currentUser = userName;
                    welcomeText = $"Welcome! You are now logged in as {currentUser}\n";

                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("User name or Password is incorrect, Please try again\n");
                }
            }

            return true;

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

        static string MainMenu()
        {
            Console.WriteLine("\t\t\n" + welcomeText + "\n");
            var options = new string[] { "Most Recent Posts"
                                        ,"Most Popular Posts"
                                        ,"Posts by Category"
                                        ,"Search"
                                        ,"Create a Post", "Quit" };

            return ShowMenu($"\tMain Menu\n", options);


        }

        static void CreateNewPost()
        {
            Console.WriteLine("Enter Text(Max 300 Char)");
            var text = Console.ReadLine();
            DbUpdate.CreatePost(text, currentUserID);
        }

        static void ShowRecentPosts()
        {

            var posts = DbUpdate.GetLatestPosts();
            var postDetails = new string[posts.Length];
            for (int i = 0; i < posts.Length; i++)
            {
                postDetails[i] = string.Format($"Post by {posts[i].User.UserName} at {posts[i].Date} ({posts[i].Like}) likes");
            }

            string choice = ShowMenu("Recent Post:\n", postDetails);

            int index = 0;
            for (int i = 0; i < postDetails.Length; i++)
            {
                if (postDetails[i] == choice)
                {
                    index = i;
                }
            }

            Console.WriteLine(choice + ":\n\n" + posts[index].Text);

            Console.WriteLine("\nHit Space to Like");

            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                DbUpdate.LikePost(posts[index].Id);
            }

            Console.WriteLine("\nHit AnyKey to return");
            Console.ReadKey();
            Console.Clear();

        }

        static void ShowMostPopular()
        {

            var posts = DbUpdate.GetMostPopular();
            var postDetails = new string[posts.Length];
            for (int i = 0; i < posts.Length; i++)
            {
                postDetails[i] = string.Format($"Post by {posts[i].User.UserName} at {posts[i].Date} ({posts[i].Like}) likes");
            }

            string choice = ShowMenu("Recent Post:\n", postDetails);

            int index = 0;
            for (int i = 0; i < postDetails.Length; i++)
            {
                if (postDetails[i] == choice)
                {
                    index = i;
                }
            }

            Console.WriteLine(choice + ":\n\n" + posts[index].Text);

            Console.WriteLine("\nHit Space to Like");

            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                DbUpdate.LikePost(posts[index].Id);
            }

            Console.WriteLine("\nHit AnyKey to return");
            Console.ReadKey();
            Console.Clear();

        }
    }
}
