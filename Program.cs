using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.Serialization;
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
            CSVHandler.UpdateAllFiles();
            
        }

        public static void HomePage()
        {
            Console.WriteLine("\n\nWelcome to your Video Game Tracker Home Page!\nPlease select an option from the menu below:\n");
            Console.WriteLine("1. Add game to tracker");
            Console.WriteLine("2. View games in tracker");
            Console.WriteLine("3. View franchises in tracker");
            Console.WriteLine("4. View genres in tracker");
            Console.WriteLine("5. Get game recommendation");
            Console.WriteLine("6. Change Settings");
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(6, "What action would you like to do?");
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
                    OpenGameRecommender();
                    break;
                case 6:
                    Menu.mainFiles.OpenSettings();
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
            //Get 2 from each top 3 genres, and 2 from each of 3 most recently played games

            List<Genre> genreList = new List<Genre>(Menu.existingGenres);
            genreList.Sort((x,y) => x.avgGenreRating.CompareTo(y.avgGenreRating));
            Genre mostRatedGenre = genreList[genreList.Count - 1];
            genreList.Sort((x,y) => x.totalHoursPlayed.CompareTo(y.totalHoursPlayed));
            Genre mostPlayedGenre = genreList[genreList.Count - 1];
            genreList.Sort((x,y) => x.avgGenreCompletion.CompareTo(y.avgGenreCompletion));
            Genre mostCompletedGenre = genreList[genreList.Count - 1];
            List<Genre> recommendedGenres = [mostRatedGenre, mostPlayedGenre, mostCompletedGenre];

            //Get top rated/excited games from each genre that are wanting to replay, ready to play
            List<VideoGame> recommendedGames = [];
            
            for (int n = 0; n < recommendedGenres.Count; n++)
            {
                int topRating = 0;
                List<VideoGame> bestGamesInGenre = [];
                for (int i = 0; i < recommendedGenres[n].attachedGameIDs.Length; i++)
                {
                    VideoGame currentGame = Menu.FindGameByID(recommendedGenres[n].attachedGameIDs[i]);
                    if (currentGame.ReadyToRecommend(true, true, false, false, true, true) && !(currentGame.completed && !Menu.FindDetailedGameByID<CompletedGame>(currentGame.gameID).wantToPlayAgain))
                    {
                        if (currentGame.RetrieveGameRatingOrExcitement() > topRating)
                        {
                            topRating = currentGame.RetrieveGameRatingOrExcitement();
                            bestGamesInGenre = [currentGame];
                        } 
                        else if (currentGame.RetrieveGameRatingOrExcitement() == topRating)
                        {
                            bestGamesInGenre.Add(currentGame);
                        }
                        
                    }
                }
                for (int i = 0; i < bestGamesInGenre.Count; i++)
                {
                    recommendedGames.Add(bestGamesInGenre[i]);
                }
            }

            List<VideoGame> displayedGames = [];
            if (recommendedGames.Count > 10)
            {
                Random random = new Random();
                bool readyToDisplay = false;
                while (displayedGames.Count < 10 && !readyToDisplay)
                {
                    int randomIndex = random.Next(0,recommendedGames.Count);
                    if (!displayedGames.Contains(recommendedGames[randomIndex]))
                    {
                        displayedGames.Add(recommendedGames[randomIndex]);
                        recommendedGames.RemoveAll(game => game.gameID == recommendedGames[randomIndex].gameID);
                    }
                    if (recommendedGames.Count < 1)
                    {
                        readyToDisplay = true;
                    }
                }
            } else
            {
                for (int i = 0; i < recommendedGames.Count; i++)
                {
                    if (!displayedGames.Contains(recommendedGames[i]))
                    {
                        displayedGames.Add(recommendedGames[i]);
                    }
                }
            }
            for (int i = 0; i < displayedGames.Count; i++)
            {
                Console.WriteLine($"\n{i+1}. {displayedGames[i].gameName}");
            }
            Console.WriteLine("\nInput the number for the game you would like to interact with, or press 0 to go back");
            int userInput = Format.GetSingleResponse(displayedGames.Count, "Please input the number for the game you would like to interact with, or press 0 to go back");
            switch (userInput)
            {
                case 0:
                    return;
                default:
                    Menu.FindGameByID(displayedGames[userInput-1].gameID).OpenGameDetails();
                    break;
            }
            GetGeneralGameRecommendation();
        }

        public static void GetSpecificGameRecommendation()
        {
            bool filterWaitingForPrequel = true;
            bool filterUnreleased = true;
            bool filterReplays = false;
            bool filterPlayed = false;
            bool filterPlaying = false;
            bool filterDropped = false;
            bool filterNotPurchased = false;
            bool filterDeckPlayable = false;
            bool avgLengthMatters = false;
            float avgLengthBase = 0;
            float avgLengthDeviation = 0;
            bool requiresCertainExcitement = false;
            int requiredExcitementLevelOrRating = 0;
            bool onlySessionGames = false;
            bool ignoreSessionGames = false;
            int[] filteredGenreIDs = [];

            Console.WriteLine($"Welcome to the targeted game searcher\nLet us help you find out what to play!");
            //Do you want to play a session game?
            if (Format.GetClosedAnswer($"Would you like to play a session game? (y/n)"))
            {
                if (Format.GetClosedAnswer($"Do you want your search to only show session games? (y/n)"))
                {
                    onlySessionGames = true;
                }
            } else
            {
                ignoreSessionGames = true;
            }

            //Do you want a specific genre?
            if (Format.GetClosedAnswer($"Is there a specific genre you would like to search for? (y/n)"))
            {
                //If so, what genres are you keen for?
                if (Menu.existingGenres.Count > 0)
                {
                    Console.WriteLine($"Please select the appropriate genres for your search");
                    for (int i = 0; i < Menu.existingGenres.Count; i++)
                    {
                        Console.WriteLine($"{i+1}. {Menu.existingGenres[i].genreName}");
                    }
                    int[] matchingGenres = Format.GetManyMenuResponses(Menu.existingGenres.Count);
                    for (int i = 0; i < matchingGenres.Length; i++)
                    {
                        //Use these genres to filter the games down initially
                        filteredGenreIDs = filteredGenreIDs.Append(Menu.existingGenres[matchingGenres[i]-1].genreID).ToArray();
                    }
                }            
            }

            //How long a game are you looking for
            if (Format.GetClosedAnswer("Are you looking for a game with a specific average length? (y/n)"))
            {
                Console.WriteLine($"Please input the average play time you are looking for");
                avgLengthBase = Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the average play time for your search");
                avgLengthDeviation = avgLengthBase * 0.3f;
                avgLengthMatters = true;
            }
            //Do you want to include games you have played?
            if (Format.GetClosedAnswer("Would you like to include games you have previously played? (y/n)"))
            {
                //Do you want to include replays?
                if (!Format.GetClosedAnswer("Do you want to include games you have marked as wanting to replay? (y/n)"))
                {
                    filterReplays = true;
                }
                //Do you want to include dropped games?
                if (!Format.GetClosedAnswer("Do you want to include games that you have previously not completed? (y/n)"))
                {
                    filterDropped = true;
                }
                if (!Format.GetClosedAnswer("Do you want to include games that you are currently playing? (y/n)"))
                {
                    filterPlaying = true;
                }
            } else
            {
                filterPlayed = true;
            }

            //Do you want to include unpurchased games?
            if (!Format.GetClosedAnswer("Do you want to include released games you do not yet own?"))
            {
                filterNotPurchased = true;
            }            

            //Do you want to include games that won't play on the steam deck?
            if (!Format.GetClosedAnswer("Do you want to include games that cannot run on the Steam deck? (y/n)"))
            {
                filterDeckPlayable = true;
            }

            //Do you want a random game, or a game that you have been keen for?
            if (Format.GetClosedAnswer("Do you want to only show games you displayed significant excitement for? (y/n)"))
            {
                requiresCertainExcitement = true;
                requiredExcitementLevelOrRating = 8;
            }
            
            (int similarGameID, int numRelGenres)[] similarGames = [];
            if (filteredGenreIDs.Length > 0)
            {
                for (int i = 0; i < filteredGenreIDs.Length; i++)
                {
                    //Cycle through each game attached to genre
                    for (int n = 0; n < Menu.existingGenres[Menu.FindGenre(filteredGenreIDs[i])].attachedGameIDs.Length; n++)
                    {
                    
                        //For each gameID, track how many times it shows up in the genre arrays
                        if (similarGames.Any(game => game.similarGameID == Menu.existingGenres[Menu.FindGenre(filteredGenreIDs[i])].attachedGameIDs[n]))
                        {
                            similarGames = similarGames.Select(game => game.similarGameID == Menu.existingGenres[Menu.FindGenre(filteredGenreIDs[i])].attachedGameIDs[n] ? (game.similarGameID, game.numRelGenres+1) : game).ToArray();
                            Console.WriteLine($"Recurring game detected");
                        } else
                        {
                            similarGames = similarGames.Append((Menu.existingGenres[Menu.FindGenre(filteredGenreIDs[i])].attachedGameIDs[n], 1)).ToArray();
                        }
                    }

                }
                //Sort final array by number of related genres
                similarGames = similarGames.OrderByDescending(game => game.numRelGenres).ToArray();
            } else
            {
                for (int i = 0; i < Menu.existingVideoGames.Count; i++)
                {
                    similarGames = similarGames.Append((Menu.existingVideoGames[i].gameID,0)).ToArray();
                }
            }
            (int similarGameID, int numRelGenres)[] similarUnplayedReadyGames = [];
            //Create list with remaining filters
            // Console.WriteLine($"filterWatingPrequel = {filterWaitingForPrequel} ;\n filterUnreleased = {filterUnreleased} ;\n filterReplays = {filterReplays} ;\n filterPlayed = {filterPlayed} ;\n filterPlaying = {filterPlaying} ;\n filterDropped = {filterDropped} ;\n filterNotPurchased = {filterNotPurchased} ;\n filterDeckPlayable = {filterDeckPlayable} ;\n avgLengthMatters = {avgLengthMatters} ;\n avgLengthBase = {avgLengthBase} ;\n avgLengthDeviation = {avgLengthDeviation} ;\n requiresCertainExcitement = {requiresCertainExcitement} ;\n requiredExcitementLevelOrRating = {requiredExcitementLevelOrRating} ;\n onlySessionGames = {onlySessionGames} ;\n ignoreSessionGames = {ignoreSessionGames}");
            similarUnplayedReadyGames = similarGames.Where(game => Menu.FindGameByID(game.similarGameID).ReadyToRecommend(filterWaitingForPrequel, filterUnreleased, filterReplays, filterPlayed, filterPlaying, filterDropped, filterNotPurchased, filterDeckPlayable, avgLengthMatters, avgLengthBase, avgLengthDeviation, requiresCertainExcitement, requiredExcitementLevelOrRating, onlySessionGames, ignoreSessionGames) == true).ToArray();

            if (similarUnplayedReadyGames.Length < 1)
            {
                if (Format.GetClosedAnswer($"Unfortunately, there were no games matching your search filters\nWould you like to try again? (y/n)"))
                {
                    GetSpecificGameRecommendation();
                }
                return;
            }
            Console.WriteLine($"\n\nPlease find the results for your search:\n\n");
            for (int i = 0; i < similarUnplayedReadyGames.Length; i++)
            {
                Console.WriteLine($"\n{i+1}. {Menu.FindGameByID(similarUnplayedReadyGames[i].similarGameID).gameName}");
            } 
            Console.WriteLine("\nInput the number for the game you would like to interact with, or press 0 to go back");
            int userInput = Format.GetSingleResponse(similarUnplayedReadyGames.Length, "Please input the number for the game you would like to interact with, or press 0 to go back");
            switch (userInput)
            {
                case 0:
                    return;
                default:
                    Menu.FindGameByID(similarUnplayedReadyGames[userInput-1].similarGameID).OpenGameDetails();
                    break;
            }
            GetSpecificGameRecommendation();
        }

        public static void OpenGameRecommender()
        {
            Console.WriteLine("\n\nWelcome to your Video Game Recommender!\nPlease select an option from the menu below:\n");
            Console.WriteLine("1. Get automatic recommendation");
            Console.WriteLine("2. Get curated recommendation");
            Console.WriteLine("0. Exit");
            int userInput = Format.GetSingleResponse(2, "What action would you like to do?");
            switch (userInput)
            {
                case 1:
                    GetGeneralGameRecommendation();
                    break;
                case 2:
                    GetSpecificGameRecommendation();
                    break;
                case 0:
                    return;
            }
            OpenGameRecommender();
        }



        //SaveDataToCSVs as if DB, normaised via .cs file layout
        //Add Primary key to initial csv file, and foreign key to each other file
        //Have any commas in data be replaced by "^&^" value
        //Have output of list be organisable via playtime, excitement level, admiration, purchase, deckPlayable, etc.
    }
    
}

