using DbAdapter;
using DbServices;
using System;
using System.Collections.Generic;

namespace Bulletin_Bored
{
    class Program
    {
        private static string currentUser;
        private static User User;
        private static bool isSignedIn;
        private static string welcomeText;
        private static bool admin;
        static void Main(string[] args)
        {

            var choice = WelcomePage();



            WelcomePageChoiceSwitch(choice);
            User = DbUpdate.GetCurrentUser(currentUser);
            admin = User.Administrator;

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
                        SearchByPhrase();
                        break;
                    case "Create a Post":
                        CreateNewPost();
                        break;
                    case "Delete Post":
                        DeletePosts();
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

                Console.WriteLine("If Admin Enter Sepecial Password, otherwise leave empty");
                var adminPassword = Console.ReadLine();

                DbUpdate.CreateUser(userName, password, adminPassword);
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

        static string[] ShowMultiMenu(string prompt, string[] options)
        {
            Console.WriteLine(prompt);

            var selected = new List<int>();
            int focused = 0;

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

                // Print all the options, highlighting the focused one and the selected ones.
                for (int i = 0; i < options.Length; i++)
                {
                    var option = options[i];
                    if (i == focused)
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else if (selected.Contains(i))
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    if (selected.Contains(i)) Console.Write("+");
                    else Console.Write("-");
                    Console.WriteLine(" " + option);

                    Console.ResetColor();
                }

                // Read another key and adjust the selected value before looping to repeat all of this.
                key = Console.ReadKey().Key;
                if (key == ConsoleKey.DownArrow)
                {
                    focused = Math.Min(focused + 1, options.Length - 1);
                }
                else if (key == ConsoleKey.UpArrow)
                {
                    focused = Math.Max(focused - 1, 0);
                }
                else if (key == ConsoleKey.Spacebar)
                {
                    if (selected.Contains(focused))
                    {
                        selected.Remove(focused);
                    }
                    else
                    {
                        selected.Add(focused);
                    }
                }
            }

            // Reset the cursor and return the selected options.
            Console.CursorVisible = true;

            // For consistency and predictability, sort selected indexes so that returned strings are in order shown in menu.
            selected.Sort();
            var selectedStrings = new List<string>();
            foreach (int i in selected)
            {
                selectedStrings.Add(options[i]);
            }
            return selectedStrings.ToArray();
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
                                        ,"Delete Post"
                                        ,"Create a Post", "Quit" };

            return ShowMenu($"\tMain Menu\n", options);


        }

        static void CreateNewPost()
        {
            Console.WriteLine("Enter Text(Max 300 Char)");
            var text = Console.ReadLine();
            var types = ShowMultiMenu("\nCatergories:", DbUpdate.GetAllCatergories());

            DbUpdate.CreatePost(text, User.Id, types);
            Console.Clear();
        }

        static void ShowRecentPosts()
        {

            var posts = DbUpdate.GetLatestPosts();


            var postDetails = new string[posts.Length];


            for (int i = 0; i < posts.Length; i++)
            {
                string categories = "";

                foreach (var type in posts[i].postCategory)
                {
                    categories += type.Category.Type + ", ";

                }

                if (categories.Length != 0)
                    categories = categories.Remove(categories.Length - 2);


                postDetails[i] = string.Format($"Post by {posts[i].User.UserName} at {posts[i].Date} ({posts[i].Like}) likes ({categories})");
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
                string categories = "";

                foreach (var type in posts[i].postCategory)
                {
                    categories += type.Category.Type + ", ";

                }

                if (categories.Length != 0)
                    categories = categories.Remove(categories.Length - 2);


                postDetails[i] = string.Format($"Post by {posts[i].User.UserName} at {posts[i].Date} ({posts[i].Like}) likes ({categories})");
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

        static void SearchByPhrase()
        {
            Console.Write("Enter Prase to search: ");
            var phrase = Console.ReadLine();


            var posts = DbUpdate.SearchPostByPhrase(phrase);

            if (posts.Length != 0)
            {
                var postDetails = new string[posts.Length];


                for (int i = 0; i < posts.Length; i++)
                {
                    string categories = "";

                    foreach (var type in posts[i].postCategory)
                    {
                        categories += type.Category.Type + ", ";

                    }

                    if (categories.Length != 0)
                        categories = categories.Remove(categories.Length - 2);


                    postDetails[i] = string.Format($"Post by {posts[i].User.UserName} at {posts[i].Date.ToShortDateString()} {posts[i].Date.ToShortTimeString()} ({posts[i].Like}) likes ({categories})");
                }

                string choice = ShowMenu($"\nPosts with {{{phrase}}} :\n", postDetails);

                int index = 0;
                for (int i = 0; i < postDetails.Length; i++)
                {
                    if (postDetails[i] == choice)
                    {
                        index = i;
                    }
                }

                Console.WriteLine(choice + ":\n\n" + posts[index].Text);

                Console.WriteLine("\nPerss any key to continue");
                Console.ReadKey();
            }

            else
            {
                Console.WriteLine("\nNo Result\n\nPress any Key to Continue");
                Console.ReadKey();
                Console.Clear();
            }





        }

        static void DeletePosts()
        {
            Post[] posts;
            if (admin)
            {
                posts = DbUpdate.GetLatestPosts();
            }
            else
            {
                posts = DbUpdate.GetPostByUser(User.Id);
            }

            if (posts.Length != 0)
            {
                var postDetails = new string[posts.Length];


                for (int i = 0; i < posts.Length; i++)
                {
                    string categories = "";

                    foreach (var type in posts[i].postCategory)
                    {
                        categories += type.Category.Type + ", ";

                    }

                    if (categories.Length != 0)
                        categories = categories.Remove(categories.Length - 2);


                    postDetails[i] = string.Format($"Post by {posts[i].User.UserName} at {posts[i].Date} ({posts[i].Like}) likes ({categories})");
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

                Console.WriteLine("\nHit Space to delete");

                if (Console.ReadKey().Key == ConsoleKey.Spacebar)
                {
                    DbUpdate.DeletePost(posts[index]);
                }

                Console.WriteLine("\nHit AnyKey to return");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                Console.WriteLine("\nNo Posts by You current user Found!\nPress Any key to Continue");
                Console.ReadKey();
                Console.Clear();

            }


        }


    }
}
