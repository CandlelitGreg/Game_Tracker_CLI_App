using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CsvHelper.Configuration;
using System.Linq;
using System.Collections;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using System.Net;

namespace GameTrackerEx01
{

    public sealed class VideoGameMap : ClassMap<VideoGame>
    {
        public VideoGameMap()
        {
            Map(m => m.gameID);
            Map(m => m.gameName);
            Map(m => m.avgGameLength);
            Map(m => m.gameGenreIDs).Convert(args => string.Join(";", args.Value.gameGenreIDs));
            Map(m => m.deckPlayable);
            Map(m => m.sessionGame);
            Map(m => m.worthCompletionist);
            Map(m => m.franchiseID);
            Map(m => m.played);
            Map(m => m.purchased);
            Map(m => m.completed);
            Map(m => m.playing);
            Map(m => m.DLCIDs).Convert(args => string.Join(";", args.Value.DLCIDs));
        }
    }



    public class VideoGame
    {
        public int gameID {get; set;} = -1;
        public string gameName {get; set;}
        public float avgGameLength {get; set;}
        public int[] gameGenreIDs {get; set;} = [];
        public bool deckPlayable {get; set;}
        public bool sessionGame {get; set;}
        public bool worthCompletionist {get; set;}
        public int franchiseID {get; set;} = -1;
        public bool played {get; set;}
        public bool purchased {get; set;}
        public bool completed {get; set;}
        public bool playing {get; set;}

        public int[] DLCIDs {get;set;} = [];


        public void AddGame()
        {
            //Assign the gameID
            gameID = Menu.GetAndUpdateNextGameID();
            //Get the game's title
            Console.WriteLine("Please input the video game title below");
            gameName = Format.CheckForCommas(Console.ReadLine(), "video game title");
            VideoGame existenceCheck = Menu.FindGameByTitle(gameName);
            while (existenceCheck != null)
            {
                if (Format.GetClosedAnswer($"{gameName} already exists, do you want to read it's information? (y/n)"))
                {
                    existenceCheck.DisplayGameDetails();
                    return;
                } else if (Format.GetClosedAnswer($"Would you like to add a different game? (y/n)"))
                {
                    gameName = Format.CheckForCommas(Console.ReadLine(), "video game title");
                    existenceCheck = Menu.FindGameByTitle(gameName);
                }
            }

            //Check franchise status
            if (Format.GetClosedAnswer($"Is {gameName} part of a larger franchise? (y/n)"))
            {
                AddToFranchise();
            }

            GetGameInfo();
        }
        public void AddToFranchise()
        {
            Console.WriteLine($"Please select the franchise {gameName} is a part of:");
            for (int i = 0; i < Menu.existingFranchises.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {Menu.existingFranchises[i].franchiseName}");
            }
            Console.WriteLine($"\n0. If {gameName} is part of an unlisted franchise please press 0\n");
            int franchiseInput = Format.GetSingleResponse(Menu.existingFranchises.Count + 1, $"Please select the franchise {gameName} is a part of:");
            if (franchiseInput == 0)
            {
                franchiseID = Menu.AddNewFranchise();
            } 
            else 
            {
                franchiseID = Menu.existingFranchises[franchiseInput-1].franchiseID;
            }

            //Add game to franchise
            if (franchiseID != -1)
            {
                Menu.AddGameToFranchise(gameID, franchiseID);
            }
        }
        public void GetGameInfo()
        {
            if (gameID == -1)
            {
                gameID = Menu.GetAndUpdateNextGameID();
            }
            

            //Get deckPlayable stat
            if (Format.GetClosedAnswer($"Is {gameName} playable on the SteamDeck? (y/n)"))
            {
                deckPlayable = true;
            }

            //Get fitting existing game genres
            if (Menu.existingGenres.Count > 0)
            {
                Console.WriteLine($"Please select the appropriate genres for {gameName}");
                for (int i = 0; i < Menu.existingGenres.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {Menu.existingGenres[i].genreName}");
                }
                Console.WriteLine($"\n{Menu.existingGenres.Count + 1}. Create a new genre for {gameName}");
                int[] matchingGenres = Format.GetManyMenuResponses(Menu.existingGenres.Count + 1);
                for (int i = 0; i < matchingGenres.Length; i++)
                {
                    if (matchingGenres[i] == Menu.existingGenres.Count + 1)
                    {
                        //Get fitting new game genres
                        if (Format.GetClosedAnswer($"Would you like to create a new genre to attach to {gameName}? (y/n)"))
                        {
                            int currentGenreCount = Menu.existingGenres.Count;
                            Menu.AddNewGenre();
                            if (Menu.existingGenres.Count > currentGenreCount)
                            {
                                for (int q = currentGenreCount; q < Menu.existingGenres.Count; q++)
                                {
                                    gameGenreIDs = gameGenreIDs.Append(Menu.existingGenres[q].genreID).ToArray();
                                    Menu.existingGenres[q].AttachGameToGenre(gameID);
                                }
                            }
                        }
                    } 
                    else 
                    {
                        gameGenreIDs = gameGenreIDs.Append(Menu.existingGenres[matchingGenres[i]-1].genreID).ToArray();
                        Menu.existingGenres[matchingGenres[i]-1].AttachGameToGenre(gameID);
                    }
                }
            }

            //TODO: Add the option to add a new genre after picking existing genres

            

            //Get average completion time
            Console.WriteLine($"Please input the average play time to complete {gameName}");
            avgGameLength = Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the average play time to complete {gameName}");

            //Get Session game status
            if (Format.GetClosedAnswer($"Would you consider {gameName} a session game? (y/n)"))
            {
                sessionGame = true;
            }

            //Get Completionist status
            if (Format.GetClosedAnswer($"Would you consider achieving 100% completion on {gameName}? (y/n)"))
            {
                worthCompletionist = true;
            }


            //Get game played status
            played = Format.GetClosedAnswer($"Have you played {gameName} before? (y/n)");
            if (played) {
                PlayedGame playedVersion = new PlayedGame();
                playedVersion.GrabGameInfo(this);
                playedVersion.GetPlayInfo();
            } else {
                UnplayedGame unplayedVersion = new UnplayedGame();
                unplayedVersion.GrabGameInfo(this);
                unplayedVersion.GetUnplayedInfo();
            }

            //Add game stats to genre averages
            for (int i = 0; i < gameGenreIDs.Length; i++)
            {
                Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].AddGameInfoToAverages(gameID);
            }
            

        }
        public void AddGameFromFranchise(string gameTitle, int existingFranchiseID)
        {
            //Add the game sequel or prequel to the game automatically
            gameID = Menu.GetAndUpdateNextGameID();
            gameName = gameTitle;
            franchiseID = existingFranchiseID;
            Menu.AddGameToFranchise(gameID, franchiseID);
            GetGameInfo();
        }
        public void UpdateGameLength()
        {
            float newGameLength = 0;
            Console.WriteLine($"Current average playtime required to complete {gameName} is {avgGameLength}");
            Console.WriteLine($"Please input new average playtime:");
            newGameLength = Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the average play time to complete {gameName}");
            if (Format.GetClosedAnswer($"Are you sure you want to update the current average playtime required to complete {gameName} from {avgGameLength} to {newGameLength}? (y/n)"))
            {
                avgGameLength = newGameLength;
                SaveGameUpdates();
            }
        }

        public void UpdatePlayStatus()
        {

        }
        public void EditTitle()
        {
            string newName = "";
            Console.WriteLine($"What would you like to change the title of {gameName} to?");
            {
                newName = Format.CheckForCommas(Console.ReadLine(), "video game title");
            }
            if (Format.GetClosedAnswer($"Are you sure you would like to change the title of this game from {gameName} to {newName}? (y/n)"))
            {
                gameName = newName;
                SaveGameUpdates();
            }
        }
        public void EditDeckStatus()
        {
            if (deckPlayable)
            {
                if (Format.GetClosedAnswer($"Are you sure you would like to update {gameName} to unplayable on the steam deck? (y/n)"))
                {
                    deckPlayable = false;
                }
            } else
            {
                if (Format.GetClosedAnswer($"Are you sure you would like to update {gameName} to playable on the steam deck? (y/n)"))
                {
                    deckPlayable = true;
                }
            }
            SaveGameUpdates();
        }
        public void AddGenres()
        {
            if (Menu.existingGenres.Count - gameGenreIDs.Length < 1)
            {
                //Add new genre if there are no unattached existing genres
                if (Format.GetClosedAnswer($"All existing genres are currently attached to {gameName}\nWould you like to create a new genre? (y/n)"))
                {
                    int currentGenreCount = Menu.existingGenres.Count;
                    Menu.AddNewGenre();
                    if (Menu.existingGenres.Count > currentGenreCount)
                    {
                        for (int i = currentGenreCount; i < Menu.existingGenres.Count; i++)
                        {
                            gameGenreIDs = gameGenreIDs.Append(Menu.existingGenres[i].genreID).ToArray();
                            Menu.existingGenres[i].AttachGameToGenre(gameID);
                            Menu.existingGenres[i].AddGameInfoToAverages(gameID);
                        }
                    }
                }
                return;
            }

            //Track and index each genre not attached to game
            List<Genre> unattachedGameGenres = Menu.existingGenres.ToList();
            int[] genreIndex = [];
            for (int i = 0; i < Menu.existingGenres.Count; i++)
            {
                if (gameGenreIDs.Contains(Menu.existingGenres[i].genreID))
                {
                    unattachedGameGenres.Remove(Menu.existingGenres[i]);
                } else
                {
                    genreIndex = genreIndex.Append(i).ToArray();
                }
            }

            //Display all non attached genres
            Console.WriteLine($"Please select all genres you would like to add to {gameName}");
            for (int i = 0; i < unattachedGameGenres.Count; i++)
            {
                Console.WriteLine($"{i+1}. {unattachedGameGenres[i].genreName}");
            }
            Console.WriteLine($"\n{unattachedGameGenres.Count + 1}. Create a new genre for {gameName}");
            int[] newMatchingGenres = Format.GetManyMenuResponses(unattachedGameGenres.Count + 1);
            for (int i = 0; i < newMatchingGenres.Length; i++)
            {
                if (newMatchingGenres[i] == unattachedGameGenres.Count + 1)
                {
                    //Get fitting new game genres
                    if (Format.GetClosedAnswer($"Would you like to create a new genre to attach to {gameName}? (y/n)"))
                    {
                        int currentGenreCount = Menu.existingGenres.Count;
                        Menu.AddNewGenre();
                        if (Menu.existingGenres.Count > currentGenreCount)
                        {
                            for (int q = currentGenreCount; q < Menu.existingGenres.Count; q++)
                            {
                                gameGenreIDs = gameGenreIDs.Append(Menu.existingGenres[q].genreID).ToArray();
                                Menu.existingGenres[q].AttachGameToGenre(gameID);
                                Menu.existingGenres[q].AddGameInfoToAverages(gameID);
                            }
                        }
                    }
                } 
                else 
                {
                    gameGenreIDs = gameGenreIDs.Append(unattachedGameGenres[newMatchingGenres[i]-1].genreID).ToArray();
                    Menu.existingGenres[genreIndex[newMatchingGenres[i]-1]].AttachGameToGenre(gameID);
                    Menu.existingGenres[genreIndex[newMatchingGenres[i]-1]].AddGameInfoToAverages(gameID);
                }
        
            }

        }
        public void RemoveGenres()
        {

            //Retrieve selection of all attached genres to remove
            Console.WriteLine($"Please select all genres you would like to remove from {gameName}");
            for (int i = 0; i < gameGenreIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].genreName}");
            }
            Console.WriteLine($"\n{gameGenreIDs.Length + 1}. Go back");
            int[] removedGenres = Format.GetManyMenuResponses(gameGenreIDs.Length + 1);
            Array.Sort(removedGenres);
            if (removedGenres.Contains(gameGenreIDs.Length + 1))
            {
                return;
            }

            //Generate and present confirmation message for removal of genres
            string removalConfirmation = $"Are you sure you would like to remove all the following genres? (y/n):";
            for (int i = 0; i < removedGenres.Length; i++)
            {
                removalConfirmation += $"\n -- {Menu.existingGenres[Menu.FindGenre(gameGenreIDs[removedGenres[i]-1])].genreName} --";
            }
            if (!Format.GetClosedAnswer(removalConfirmation))
            {
                if (Format.GetClosedAnswer($"Would you like to remove a different selection of genres from {gameName}? (y/n)"))
                {
                    RemoveGenres();
                    return;
                }
                return;
            }

            //Remove Game from genres and genres from game
            for (int i = removedGenres.Length - 1; i >= 0; i--)
            {
                Menu.existingGenres[Menu.FindGenre(gameGenreIDs[removedGenres[i]-1])].RemoveFromGenreStats(gameID);
                Menu.existingGenres[Menu.FindGenre(gameGenreIDs[removedGenres[i]-1])].RemoveGameFromGenre(gameID);
                gameGenreIDs = gameGenreIDs.Where(ID => ID != gameGenreIDs[removedGenres[i] - 1]).ToArray();
            }
        }
        public void RemoveFromSequel(int sequelID)
        {
            if (played)
            {
                switch (true)
                {
                    case var _ when completed:
                        CompletedGame cg = Menu.FindDetailedGameByID<CompletedGame>(gameID);
                        if (cg.nextEntryID == sequelID) cg.nextEntryID = -1;
                        break;
                    case var _ when playing:
                        CurrentGame pg = Menu.FindDetailedGameByID<CurrentGame>(gameID);
                        if (pg.nextEntryID == sequelID) pg.nextEntryID = -1;
                        break;
                    case var _ when !completed && !playing:
                        DroppedGame dg = Menu.FindDetailedGameByID<DroppedGame>(gameID);
                        if (dg.nextEntryID == sequelID) dg.nextEntryID = -1;
                        break;
                }
            }
            else
            {
                switch (true)
                {
                    case var _ when !purchased:
                        UnpurchasedGame ug = Menu.FindDetailedGameByID<UnpurchasedGame>(gameID);
                        if (ug.previousEntryID == sequelID) ug.previousEntryID = -1;
                        break;
                    case var _ when purchased:
                        BackloggedGame bg = Menu.FindDetailedGameByID<BackloggedGame>(gameID);
                        if (bg.previousEntryID == sequelID) bg.previousEntryID = -1;
                        break;
                }
            }
            SaveGameUpdates();
        }
        public void SetSequel(int sequelID)
        {
            if (played)
            {
                switch (true)
                {
                    case var _ when completed:
                        CompletedGame cg = Menu.FindDetailedGameByID<CompletedGame>(gameID);
                        if (cg.nextEntryID != -1 && cg.nextEntryID != sequelID && Format.GetClosedAnswer($"{Menu.FindGameByID(cg.nextEntryID).gameName} is currently saved as the sequel to {gameName}\nAre you sure you want to overwrite this and make {Menu.FindGameByID(sequelID).gameName} the sequel instead? (y/n)"))
                        {
                            Menu.FindGameByID(cg.nextEntryID).RemoveFromSequel(gameID);
                        }
                        cg.nextEntryID = sequelID;
                        Console.WriteLine($"Completed game: {gameName} is adding the sequel {Menu.FindGameByID(sequelID).gameName} to file");
                        break;
                    case var _ when playing:
                        CurrentGame pg = Menu.FindDetailedGameByID<CurrentGame>(gameID);
                        if (pg.nextEntryID != -1 && pg.nextEntryID != sequelID && Format.GetClosedAnswer($"{Menu.FindGameByID(pg.nextEntryID).gameName} is currently saved as the sequel to {gameName}\nAre you sure you want to overwrite this and make {Menu.FindGameByID(sequelID).gameName} the sequel instead? (y/n)"))
                        {
                            Menu.FindGameByID(pg.nextEntryID).RemoveFromSequel(gameID);
                        }
                        pg.nextEntryID = sequelID;
                        break;
                    case var _ when !completed && !playing:
                        DroppedGame dg = Menu.FindDetailedGameByID<DroppedGame>(gameID);
                        if (dg.nextEntryID != -1 && dg.nextEntryID != sequelID && Format.GetClosedAnswer($"{Menu.FindGameByID(dg.nextEntryID).gameName} is currently saved as the sequel to {gameName}\nAre you sure you want to overwrite this and make {Menu.FindGameByID(sequelID).gameName} the sequel instead? (y/n)"))
                        {
                            Menu.FindGameByID(dg.nextEntryID).RemoveFromSequel(gameID);
                        }
                        dg.nextEntryID = sequelID;
                        break;
                }
                SaveGameUpdates();
            }
            
        }
        public void EditGenres()
        {
            Console.WriteLine($"These are the current genres for {gameName}:");
            for (int i = 0; i < gameGenreIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].genreName}");
            }
            Console.WriteLine($"\nWhat would you like to do with {gameName}'s genres?");
            Console.WriteLine("1. Add a genre");
            Console.WriteLine("2. Remove a genre");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What would you like to do with {gameName}'s genres?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    AddGenres();
                    break;
                case 2:
                    RemoveGenres();
                    break;
            }
            SaveGameUpdates();
            if (Format.GetClosedAnswer($"Would you like to continue editing {gameName}'s genres? (y/n)"))
            {
                EditGenres();
            }
        }
        public void EditFurtherDetails()
        {
            switch (true)
            {
                case var _ when completed:
                    Menu.FindDetailedGameByID<CompletedGame>(gameID).EditChildDetails();
                    break;
                case var _ when playing:
                    Menu.FindDetailedGameByID<CurrentGame>(gameID).EditChildDetails();
                    break;
                case var _ when played && !playing && !completed:
                    Menu.FindDetailedGameByID<DroppedGame>(gameID).EditChildDetails();
                    break;
                case var _ when !played && !purchased:
                    Menu.FindDetailedGameByID<UnpurchasedGame>(gameID).EditChildDetails();
                    break;
                case var _ when !played && purchased:
                    Menu.FindDetailedGameByID<BackloggedGame>(gameID).EditChildDetails();
                    break;
            }
        }
        public void EditGameDetails()
        {
            Console.WriteLine($"What would you like to edit about {gameName}?\n");
            Console.WriteLine("1. Edit title");
            Console.WriteLine("2. Edit average game length");
            Console.WriteLine("3. Edit deck playable status");
            Console.WriteLine("4. Edit game genres");
            Console.WriteLine("5. See further editing options");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(5, $"What would you like to edit about {gameName}?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    EditTitle();
                    break;
                case 2:
                    UpdateGameLength();
                    break;
                case 3:
                    EditDeckStatus();
                    break;
                case 4:
                    EditGenres();
                    break;
                case 5:
                    EditFurtherDetails();
                    break;
            }
        }
        public void OpenGameDetails()
        {
            Console.WriteLine($"What would you like to do with {gameName}?\n");
            Console.WriteLine("1. View game details");
            Console.WriteLine("2. Edit game details");
            Console.WriteLine("3. View similar games");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(3, $"What would you like to do with {gameName}?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    Console.WriteLine(DisplayGameDetails());
                    break;
                case 2:
                    EditGameDetails();
                    break;
                case 3:
                    ViewSimilarGames();
                    break;
            }
            if (Format.GetClosedAnswer($"Do you want to keep interacting with {gameName}? (y/n)"))
            {
                OpenGameDetails();
            }
            return;
        }
        public void ViewSimilarGames()
        {
            //Collect attachedGameID arrays for each gameGenre
            (int similarGameID, int numRelGenres)[] similarGames = [];
            //Cycle through each genre
            for (int i = 0; i < gameGenreIDs.Length; i++)
            {
                //Cycle through each game attached to genre
                for (int n = 0; n < Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].attachedGameIDs.Length; n++)
                {
                   
                    //For each gameID, track how many times it shows up in the genre arrays
                    if (similarGames.Any(game => game.similarGameID == Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].attachedGameIDs[n]))
                    {
                        similarGames = similarGames.Select(game => game.similarGameID == Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].attachedGameIDs[n] ? (game.similarGameID, game.numRelGenres+1) : game).ToArray();
                        Console.WriteLine($"Recurring game detected");
                    } else
                    {
                        similarGames = similarGames.Append((Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].attachedGameIDs[n], 1)).ToArray();
                    }
                }

            }

            //Sort final array by number of related genres
            // similarGames.Sort(similarGames, (a,b) => a.numRelGenres.CompareTo(b.numRelGenres));
            similarGames = similarGames.OrderByDescending(game => game.numRelGenres).ToArray();
            for (int i = 0; i < similarGames.Length; i++)
            {
                Console.WriteLine($"{similarGames[i].similarGameID}, {similarGames[i].numRelGenres}");
            }
            (int similarGameID, int numRelGenres)[] similarUnplayedReadyGames = [];

            //Create list without any games played, or awaiting previous completion
            Console.WriteLine($"\n\nGames similar to {gameName}:\n\n");
            for (int i = 0; i < similarGames.Length; i++)
            {
                if (i < 5)
                {
                    Console.WriteLine($"\n{i+1}.\n{Menu.FindGameByID(similarGames[i].similarGameID).DisplayGameDetails()}\n\n");
                }
            }

            similarUnplayedReadyGames = similarGames.Where(game => Menu.FindGameByID(game.similarGameID).ReadyToRecommend(true, false, false, true) == true).ToArray();

            Console.WriteLine($"Unplayed and ready to play games similar to {gameName}:");
            for (int i = 0; i < similarUnplayedReadyGames.Length; i++)
            {
                Console.WriteLine($"\n{i+1}.\n{Menu.FindGameByID(similarUnplayedReadyGames[i].similarGameID).DisplayGameDetails()}");
            }
            //If any of the games have been completed, and retry is not desired, remove entry and find next suitable result

            //If selected as so, remove all results from same franchise, and find next suitable results

            //If selected as such, prioritise similar length games

            //If selected as such, remove unpurchased games, and replace with next suitable results

            //If requested as such, remove all played games from suggestions


        }
        public string DisplayGameDetails()
        {
            string details = "";
            details += $"Title:             {gameName}\n";
            details += $"Avg Game Length:   {avgGameLength}\n";
            if (gameGenreIDs.Length >= 1)
            {
                details += $"Game Genres:       {Menu.existingGenres[Menu.FindGenre(gameGenreIDs[0])].genreName}";
                for (int i = 1; i < gameGenreIDs.Length; i++)
                {
                    details += $" || {Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].genreName}";
                }
                details += "\n";
            } else
            {
                details += $"Game Genres:       None\n";
            }
            details += $"Playable on Deck:  {deckPlayable}\n";
            if (played)
            {
                if (!completed && !playing)
                {
                    details += $"Played Status:     Dropped\n";
                    details +=Menu.FindDetailedGameByID<DroppedGame>(gameID).DisplayDroppedGameInfo();
                }
            } else
            {
                if (purchased)
                {
                    details += $"Played Status:     Backlogged\n";
                    details += Menu.FindDetailedGameByID<BackloggedGame>(gameID).DisplayBackloggedGameInfo();
                } else
                {
                    details += $"Played Status:     Unpurchased\n";
                    details += Menu.FindDetailedGameByID<UnpurchasedGame>(gameID).DisplayUnpurchasedGameInfo();
                }
            }
            if (completed)
            {
                details += $"Played Status:     Completed\n";
                details += Menu.FindDetailedGameByID<CompletedGame>(gameID).DisplayCompletedGameInfo();
            }
            if (playing)
            {
                details += $"Played Status:     Currently Playing\n";
                details += Menu.FindDetailedGameByID<CurrentGame>(gameID).DisplayCurrentGameInfo();
            }
            return details;
        }
        public bool ReadyToRecommend(
                                bool filterWaitingForPrequel = false, 
                                bool filterUnreleased = false, 
                                bool filterReplays = false, 
                                bool filterPlayed = false,
                                bool filterPlaying = false,
                                bool filterDropped = false,
                                bool filterNotPurchased = false,
                                bool filterDeckPlayable = false,
                                bool avgLengthMatters = false,
                                float avgLengthBase = 0,
                                float avgLengthDeviation = 0,
                                bool requiresCertainExcitement = false,
                                int requiredExcitementLevelOrRating = 0,
                                bool onlySessionGames = false,
                                bool ignoreSessionGames = false)
        {
            bool recommendable = true;
            if (completed && !Menu.FindDetailedGameByID<CompletedGame>(gameID).wantToPlayAgain)
            {
                return false;
            }
            if (filterWaitingForPrequel)
            {
                switch (true)
                {
                    case var _ when purchased && !played:
                        if (franchiseID != -1)
                        {
                            BackloggedGame detailedVersion = Menu.FindDetailedGameByID<BackloggedGame>(gameID);
                            if (detailedVersion.previousEntryID != -1)
                            {
                                recommendable = Menu.FindGameByID(detailedVersion.previousEntryID).completed;
                            }
                        }
                        break;
                    case var _ when !purchased && !played:
                        if (franchiseID != -1)
                        {
                            UnpurchasedGame detailedVersion = Menu.FindDetailedGameByID<UnpurchasedGame>(gameID);
                            if (detailedVersion.previousEntryID != -1)
                            {
                                recommendable = Menu.FindGameByID(detailedVersion.previousEntryID).completed;
                            }
                        }
                        break;
                }
            }
            if (filterUnreleased && !played && !purchased)
            {
                recommendable = Menu.FindDetailedGameByID<UnpurchasedGame>(gameID).released;
                
            }
            if (filterReplays && completed)
            {
                return false;
            }
            if (filterPlayed && played)
            {
                return false;
            }
            if (filterPlaying && playing)
            {
                return false;
            }
            if (filterDropped && played && !playing && !completed)
            {
                return false;
            }
            if (filterNotPurchased && !purchased)
            {
                return false;
            }
            if (avgLengthMatters)
            {
                Console.WriteLine($"{gameName} is length of {avgGameLength}");
                if ((avgGameLength >= avgLengthBase && (avgGameLength - avgLengthDeviation) > avgLengthBase) || (avgGameLength < avgLengthBase && avgGameLength + avgLengthDeviation < avgLengthBase))
                {
                    return false;
                } 
            }
            if (requiresCertainExcitement)
            {
                switch (true)
                {
                    case var _ when purchased && !played:
                        BackloggedGame backloggedVersion = Menu.FindDetailedGameByID<BackloggedGame>(gameID);
                        if (backloggedVersion.excitementLevel < requiredExcitementLevelOrRating)
                        {
                            return false;
                        }
                        break;
                    case var _ when !purchased && !played:
                        UnpurchasedGame unpurchasedVersion = Menu.FindDetailedGameByID<UnpurchasedGame>(gameID);
                        if (unpurchasedVersion.excitementLevel < requiredExcitementLevelOrRating)
                        {
                            return false;
                        }
                        break;
                    case var _ when played && !completed && !playing:
                        DroppedGame droppedVersion = Menu.FindDetailedGameByID<DroppedGame>(gameID);
                        if (droppedVersion.rating < requiredExcitementLevelOrRating)
                        {
                            return false;
                        }
                        break;
                    case var _ when completed:
                        CompletedGame completedVersion = Menu.FindDetailedGameByID<CompletedGame>(gameID);
                        if (completedVersion.rating < requiredExcitementLevelOrRating)
                        {
                            return false;
                        }
                        break;
                    case var _ when playing:
                        CurrentGame playingVersion = Menu.FindDetailedGameByID<CurrentGame>(gameID);
                        if (playingVersion.rating < requiredExcitementLevelOrRating)
                        {
                            return false;
                        }
                        break;
                }
            }
            if (filterDeckPlayable && !deckPlayable)
            {
                return false;
            }
            if (onlySessionGames && !sessionGame)
            {
                return false;
            }
            if (ignoreSessionGames && sessionGame)
            {
                return false;
            }
            return recommendable;
        }
        public void UpdateAllGenreStats()
        {
            for (int i = 0; i < gameGenreIDs.Length; i++)
            {
                Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].UpdateStatsForGame(gameID);
            }
        }
        public void GrabGameInfo(VideoGame parentGame)
        {
            gameID = parentGame.gameID;
            gameName = parentGame.gameName;
            avgGameLength = parentGame.avgGameLength;
            gameGenreIDs = parentGame.gameGenreIDs;
            deckPlayable = parentGame.deckPlayable;
            sessionGame = parentGame.sessionGame;
            worthCompletionist = parentGame.worthCompletionist;
            franchiseID = parentGame.franchiseID;
            played = parentGame.played;
            DLCIDs = parentGame.DLCIDs;
        }
        public void SaveGameUpdates()
        {
            UpdateAllGenreStats();
            switch (true)
            {
                case var _ when completed:
                    Menu.FindDetailedGameByID<CompletedGame>(gameID).SaveGameInfo();
                    break;
                case var _ when playing:
                    Menu.FindDetailedGameByID<CurrentGame>(gameID).SaveGameInfo();
                    break;
                case var _ when played && !playing && !completed:
                    Menu.FindDetailedGameByID<DroppedGame>(gameID).SaveGameInfo();
                    break;
                case var _ when !played && !purchased:
                    Menu.FindDetailedGameByID<UnpurchasedGame>(gameID).SaveGameInfo();
                    break;
                case var _ when !played && purchased:
                    Menu.FindDetailedGameByID<BackloggedGame>(gameID).SaveGameInfo();
                    break;
            }
            Console.WriteLine("GameInfo saved");
        }
    }
}