using System;
using System.Collections.Generic;
using CsvHelper;

namespace GameTrackerEx01
{
    class Program
    {
        
        static void Main()
        {
            // CSVHandler.CreateRefresh();
            Menu.GetStats();
            HomePage();
            // if (Format.GetClosedAnswer("Would you like to add a new game to the tracker? (y/n)\n"))
            // {
            //     VideoGame newGame = new VideoGame();
            //     newGame.AddGame();
            //     videoGameList.Add(newGame);

            // }
            // for (int i = 0; i < Menu.existingVideoGames.Count; i++)
            // {
            //     Console.WriteLine(Menu.existingVideoGames[i].DisplayGameDetails());
            // }
            // for (int i = 0; i < Menu.existingGenres.Count; i++)
            // {
            //     Console.WriteLine(Menu.existingGenres[i].genreName);
            // }
            
        }

        public static void HomePage()
        {
            Console.WriteLine("\n\nWelcome to your Video Game Tracker Home Page!\nPlease select an option from the menu below:\n");
            Console.WriteLine("1. Add game to tracker");
            Console.WriteLine("2. View games in tracker");
            Console.WriteLine("3. View franchises in tracker");
            Console.WriteLine("4. View genres in tracker");
            Console.WriteLine("5. Get game recommendation");
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(5, "What action would you like to do?");
            switch (userInput)
            {
                case 1:
                    AddGame();
                    break;
                case 2:
                    ViewGames();
                    break;
                case 3:
                    ViewFranchises();
                    break;
                case 4:
                    ViewGenres();
                    break;
                case 5:
                    //TODO: Display list of recommended games based on previous games stats
                    break;
                case 0:
                    return;
            }
            if (Format.GetClosedAnswer($"Would you like to return to the home page? (y/n)"))
            {
                HomePage();
            }
            return;
        }

        public static void ViewGames()
        {
            Console.WriteLine("Here are all the games in your tracker:\n");
            for (int i = 0; i < Menu.existingVideoGames.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Menu.existingVideoGames[i].gameName}");
            }
            Console.WriteLine("\nPlease select a game to view details for, or type 0 to go back.");
            int userInput = Format.GetSingleResponse(Menu.existingVideoGames.Count, "Which game would you like to view details for?");
            if (userInput == 0) return;
            Menu.existingVideoGames[userInput-1].OpenGameDetails();
            if (Format.GetClosedAnswer($"Would you like to interact with another game? (y/n)"))
            {
                ViewGames();
            }
        }

        public static void ViewFranchises()
        {
            Console.WriteLine("Here are all the franchises in your tracker:\n");
            for (int i = 0; i < Menu.existingFranchises.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Menu.existingFranchises[i].franchiseName}");
            }
            Console.WriteLine("\nPlease select a franchise to engage with, or type 0 to go back");
            int userInput = Format.GetSingleResponse(Menu.existingFranchises.Count, "Which franchise would you like to view details for?");
            if (userInput == 0) return;
            Menu.existingFranchises[userInput-1].OpenFranchise();
            if (Format.GetClosedAnswer($"Would you like to interact with another franchise? (y/n)"))
            {
                ViewFranchises();
            }
        }

        public static void ViewGenres()
        {
            Console.WriteLine("Here are all the genres in your tracker:\n");
            for (int i = 0; i < Menu.existingGenres.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Menu.existingGenres[i].genreName}");
            }
            Console.WriteLine("\nPlease select a genre to view details for, or type 0 to go back");
            int userInput = Format.GetSingleResponse(Menu.existingGenres.Count, "Which genre would you like to view details for?");
            if (userInput == 0) return;
            Menu.existingGenres[userInput-1].OpenGenre();
            if (Format.GetClosedAnswer($"Would you like to interact with another genre? (y/n)"))
            {
                ViewGenres();
            }
        }
        public static void AddGame()
        {
            VideoGame newGame = new VideoGame();
            newGame.AddGame();
        }

        public static void SearchGames()
        {
            
        }

        public static void GetGeneralGameRecommendation()
        {

        }

        public static void GetSpecificGameRecommendation()
        {

        }

        



        //SaveDataToCSVs as if DB, normaised via .cs file layout
        //Add Primary key to initial csv file, and foreign key to each other file
        //Have any commas in data be replaced by "^&^" value
        //Have output of list be organisable via playtime, excitement level, admiration, purchase, deckPlayable, etc.
    }
    
}

