using System;
using System.Collections.Generic;
using CsvHelper;

namespace GameTrackerEx01
{
    class Program
    {
        
        
        public static List<VideoGame> videoGameList = [];
        
        static void Main()
        {
            CSVHandler.CreateBackups();
            Menu.GetStats();
            Console.WriteLine("\n\nWelcome to your Video Game Tracker");
            if (Format.GetClosedAnswer("Would you like to add a new game to the tracker? (y/n)\n"))
            {
                VideoGame newGame = new VideoGame();
                newGame.AddGame();
                videoGameList.Add(newGame);

            }
            for (int i = 0; i < videoGameList.Count; i++)
            {
                Console.WriteLine(videoGameList[i].DisplayGameDetails());
            }
            for (int i = 0; i < Menu.existingGenres.Count; i++)
            {
                Console.WriteLine(Menu.existingGenres[i].genreName);
            }
            
        }

        public static void HomePage()
        {

        }

        public static void AddGame()
        {

        }

        public static void ViewFranchises()
        {

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

