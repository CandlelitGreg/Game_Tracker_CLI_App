using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GameTrackerEx01
{
    public sealed class GenreMap : ClassMap<Genre>
    {
        public GenreMap()
        {
            Map(m => m.genreID);
            Map(m => m.genreName);
            Map(m => m.attachedFranchiseIDs).Convert(args => string.Join(";", args.Value.attachedFranchiseIDs));
            Map(m => m.attachedGameIDs).Convert(args => string.Join(";", args.Value.attachedGameIDs));
            Map(m => m.avgGenreRating);
            Map(m => m.avgGenreLength);
            Map(m => m.avgGenrePlaytime);
            Map(m => m.avgGenreExcitement);
            Map(m => m.numGamesPlayed);
            Map(m => m.numGamesUnplayed);
            Map(m => m.totalHoursPlayed);
        }
    }

    public sealed class RetrieveGenreMap : ClassMap<Genre>
    {
        public RetrieveGenreMap()
        {
            Map(m => m.genreID);
            Map(m => m.genreName);
            Map(m => m.attachedFranchiseIDs).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("attachedFranchiseIDs")) ? Array.Empty<int>() : args.Row.GetField("attachedFranchiseIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.attachedGameIDs).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("attachedGameIDs")) ? Array.Empty<int>() : args.Row.GetField("attachedGameIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.avgGenreRating);
            Map(m => m.avgGenreLength);
            Map(m => m.avgGenrePlaytime);
            Map(m => m.avgGenreExcitement);
            Map(m => m.numGamesPlayed);
            Map(m => m.numGamesUnplayed);
            Map(m => m.totalHoursPlayed);
        }
    }

    public class Genre {
        public int genreID {get;set;}
        public string genreName {get; set;}
        public int[] attachedFranchiseIDs {get;set;} = [];
        public int[] attachedGameIDs {get;set;} = [];
        public float avgGenreRating {get;set;}
        public float avgGenreLength {get;set;}
        public float avgGenrePlaytime {get;set;}
        public float avgGenreExcitement {get;set;}
        public float avgGenreCompletion {get;set;}
        public int numGamesPlayed {get;set;}
        public int numGamesUnplayed {get;set;}
        public float totalHoursPlayed {get;set;}

        public void AttachFranchiseToGenre(int franchiseID)
        {
            attachedFranchiseIDs = attachedFranchiseIDs.Append(franchiseID).ToArray();
            SaveGenreChanges();
        }
        public void RemoveFranchiseFromGenre(int franchiseID)
        {
            attachedFranchiseIDs = attachedFranchiseIDs.Where(ID => ID != franchiseID).ToArray();
            SaveGenreChanges();
        }
        public void AttachGameToGenre(int gameID)
        {
            attachedGameIDs = attachedGameIDs.Append(gameID).ToArray();
            SaveGenreChanges();
        }
        public void RemoveGameFromGenre(int gameID)
        {
            attachedGameIDs = attachedGameIDs.Where(ID => ID != gameID).ToArray();
            SaveGenreChanges();
        }
        public void OpenGenre()
        {
            Console.WriteLine($"How would you like to interact with the {genreName} genre?\n" +
                              $"1. View genre details\n" +
                              $"2. Edit genre details\n" +
                              $"0. Go back");
            int userInput = Format.GetSingleResponse(2, "What would you like to do with the genre?");
            switch (userInput)
            {
                case 1:
                    ViewGenreDetails();
                    break;
                case 2:
                    EditGenreDetails();
                    break;
                case 0:
                    return;
            }
            if (Format.GetClosedAnswer($"Would you like to interact with the {genreName} genre again? (y/n)"))
            {
                OpenGenre();
            }
        }
        public void ViewGenreDetails()
        {
            Console.WriteLine($"What information would you like to view from the {genreName} genre?\n" +
                              $"1. View genre games\n" +
                              $"2. View genre franchises\n" +
                              $"3. View genre stats\n" +
                              $"0. Go back");
            int userInput = Format.GetSingleResponse(3, "What would you like to do with the genre?");
            switch (userInput)
            {
                case 1:
                    ViewAssociatedGames();
                    break;
                case 2:
                    ViewAssociatedFranchises();
                    break;
                case 3:
                    ViewGenreAverages();
                    break;
                case 0:
                    return;
            }
            if (Format.GetClosedAnswer($"Would you like to view more details from the {genreName} genre? (y/n)"))
            {
                ViewGenreDetails();
            }
        }
        public void ViewGenreAverages()
        {    
            Console.WriteLine($"Genre Name:                     {genreName}\n" +
                              $"Average Genre Rating:           {avgGenreRating}\n" +
                              $"Average Genre Length:           {avgGenreLength}\n" +
                              $"Average Genre Playtime:         {avgGenrePlaytime}\n" +
                              $"Average Genre Excitement:       {avgGenreExcitement}\n" +
                              $"Average Genre Completion:       {avgGenreCompletion}\n" +
                              $"Number of games played:         {numGamesPlayed}\n" +
                              $"Number of games unplayed:       {numGamesUnplayed}\n" +
                              $"Total # of hours played:        {totalHoursPlayed}\n");
        }
        public void ViewAssociatedFranchises()
        {    
            Console.WriteLine($"Genre Name:           {genreName}\n" +
                              $"Franchises associated with Genre:\n");
            for (int i = 0; i < attachedFranchiseIDs.Length; i++)
            {
                int franchiseIndex = Menu.FindFranchise(attachedFranchiseIDs[i]);
                if (franchiseIndex != -1)
                {
                    Franchise franchise = Menu.existingFranchises[franchiseIndex];
                    Console.WriteLine($"{i + 1}: {franchise.franchiseName}");
                }
            }
            //TODO: Add functionality to view details of each franchise associated with the genre
        }
        public void ViewAssociatedGames()
        {
            Console.WriteLine($"Genre Name:           {genreName}\n" +
                              $"Games associated with Genre:\n");
            for (int i = 0; i < attachedGameIDs.Length; i++)
            {
                VideoGame game = Menu.FindGameByID(attachedGameIDs[i]);
                if (game != null)
                {
                    Console.WriteLine($"{i + 1}: {game.gameName}");
                }
            }
            //TODO: Add functionality to view details of each game associated with the genre
        }
        public void EditGenreDetails()
        {
            Console.WriteLine($"What information would you like to edit from the {genreName} genre?\n" +
                              $"1. Edit genre games\n" +
                              $"2. Edit genre franchises\n" +
                              $"0. Go back");
            int userInput = Format.GetSingleResponse(2, "What would you like to edit from the genre?");
            switch (userInput)
            {
                case 1:
                    EditGenreGames();
                    break;
                case 2:
                    EditGenreFranchises();
                    break;
                case 0:
                    return;
            }
            if (Format.GetClosedAnswer($"Would you like to edit more details from the {genreName} genre? (y/n)"))
            {
                EditGenreDetails();
            }
        }
        public void EditGenreGames()
        {
            Console.WriteLine($"These are the current games attached to the {genreName} genre:");
            for (int i = 0; i < attachedGameIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.FindGameByID(attachedGameIDs[i]).gameName}");
            }
            Console.WriteLine($"\nWhat would you like to do with the {genreName} games?");
            Console.WriteLine("1. Add games to genre");
            Console.WriteLine("2. Remove games from genre");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What would you like to do with the games attached to the {genreName} genre?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    AddGenreGames();
                    break;
                case 2:
                    RemoveGenreGames();
                    break;
            }
            SaveGenreChanges();
            if (Format.GetClosedAnswer($"Would you like to continue editing the games attched to the {genreName} genre? (y/n)"))
            {
                EditGenreGames();
            }
        }
        public void AddGenreGames()
        {
            //TODO: Change adding game to be a selction add
            string gameName = Format.AskForInput($"the name of the game you would like to add to the {genreName} genre");
            VideoGame searchedGame = Menu.FindGameByTitle(gameName);
            if (searchedGame != null)
            {
                searchedGame.gameGenreIDs = searchedGame.gameGenreIDs.Append(genreID).ToArray();
                attachedGameIDs = attachedGameIDs.Append(searchedGame.gameID).ToArray();
                AddGameInfoToAverages(searchedGame.gameID);
                searchedGame.SaveGameUpdates();
            } 
            else if (Format.GetClosedAnswer($"{gameName} could not be found. Do you want to add it to your records? (y/n)"))
            {
                Console.WriteLine($"This functionality is not yet implemented. Please add {gameName} from the home screen.");
                return;
            }
            if (Format.GetClosedAnswer($"Would you like to add a different game to the {genreName} genre? (y/n)"))
            {
                AddGenreGames();
            }
            SaveGenreChanges();
        }
        public void RemoveGenreGames()
        {
            //Retrieve selection of all attached genres to remove
            Console.WriteLine($"Please select all games you would like to remove from the {genreName} genre");
            for (int i = 0; i < attachedGameIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.FindGameByID(attachedGameIDs[i]).gameName}");
            }
            Console.WriteLine($"\n{attachedGameIDs.Length + 1}. Go back");
            int[] removedGames = Format.GetManyMenuResponses(attachedGameIDs.Length + 1);
            Array.Sort(removedGames);
            if (removedGames.Contains(attachedGameIDs.Length + 1))
            {
                return;
            }

            //Generate and present confirmation message for removal of genres
            string removalConfirmation = $"Are you sure you would like to remove all the following games? (y/n):";
            for (int i = 0; i < removedGames.Length; i++)
            {
                removalConfirmation += $"\n -- {Menu.FindGameByID(attachedGameIDs[removedGames[i]-1]).gameName} --";
            }
            if (!Format.GetClosedAnswer(removalConfirmation))
            {
                if (Format.GetClosedAnswer($"Would you like to remove a different selection of games from the {genreName} genre? (y/n)"))
                {
                    RemoveGenreGames();
                    return;
                }
                return;
            }

            //Remove Game from genre and genre from games
            for (int i = removedGames.Length - 1; i >= 0; i--)
            {
                //Remove game from genre
                int currentGameID = attachedGameIDs[removedGames[i]-1];
                Menu.FindGameByID(currentGameID).gameGenreIDs = Menu.FindGameByID(currentGameID).gameGenreIDs.Where(ID => ID != genreID).ToArray();
                Menu.FindGameByID(currentGameID).SaveGameUpdates();
                RemoveFromGenreStats(currentGameID);
                RemoveGameFromGenre(currentGameID);
            }            
        }
        public void EditGenreFranchises()
        {
            Console.WriteLine($"These are the current franchises attached to the {genreName} genre:");
            for (int i = 0; i < attachedFranchiseIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.existingFranchises[Menu.FindFranchise(attachedFranchiseIDs[i])].franchiseName}");
            }
            Console.WriteLine($"\nWhat would you like to do with the {genreName} franchises?");
            Console.WriteLine("1. Add franchise to genre");
            Console.WriteLine("2. Remove franchise from genre");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What would you like to do with the franchises attached to the {genreName} genre?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    AddGenreFranchise();
                    break;
                case 2:
                    RemoveGenreFranchise();
                    break;
            }
            SaveGenreChanges();
            if (Format.GetClosedAnswer($"Would you like to continue editing the franchises attched to the {genreName} genre? (y/n)"))
            {
                EditGenreGames();
            }
        }
        public void RecalculateGenreStats()
        {
            avgGenreRating = 0;
            avgGenreLength = 0;
            avgGenrePlaytime = 0;
            avgGenreExcitement = 0;
            avgGenreCompletion = 0;
            numGamesPlayed = 0;
            numGamesUnplayed = 0;
            totalHoursPlayed = 0;

            for (int i = 0; i < attachedGameIDs.Length; i++)
            {
                VideoGame nextGame = Menu.FindGameByID(attachedGameIDs[i]);
                if (nextGame == null)
                {
                    RemoveGameFromGenre(attachedGameIDs[i]);
                    SaveGenreChanges();
                    RecalculateGenreStats();
                    SaveGenreChanges();
                    return;
                }
                //Check length
                // Console.WriteLine($"Checking average game length of {nextGame.gameName} is {nextGame.avgGameLength}hrs for {genreName} genre");
                avgGenreLength = ((avgGenreLength * i) + nextGame.avgGameLength) / (i + 1);
                //Check played game
                if (nextGame.played)
                {
                    PlayedGame pGame = null;
                    if (nextGame.completed) pGame = Menu.FindDetailedGameByID<CompletedGame>(nextGame.gameID);
                    if (nextGame.playing) pGame = Menu.FindDetailedGameByID<CurrentGame>(nextGame.gameID);
                    if (!nextGame.playing && !nextGame.completed) pGame = Menu.FindDetailedGameByID<DroppedGame>(nextGame.gameID);

                    numGamesPlayed++;

                    //Check rating
                    avgGenreRating = ((avgGenreRating * (numGamesPlayed - 1)) + pGame.rating) / numGamesPlayed;

                    //Check completion
                    //If completed completion bonus = 1 else 0
                    //If completion bonus and avgGenreCompletion == 0, avgGenreCompletion still = 0 else calc average
                    int completionBonus = nextGame.completed ? 1 : 0;
                    avgGenreCompletion = avgGenreCompletion == 0 && completionBonus == 0 ? 0 : ((avgGenreCompletion * (numGamesPlayed - 1)) + completionBonus) / numGamesPlayed;

                    //Check playtime total
                    //Check playtime avg
                    totalHoursPlayed += pGame.hoursPlayed;
                    avgGenrePlaytime = totalHoursPlayed / numGamesPlayed;

                    //Check initial excitement
                    avgGenreExcitement = ((avgGenreExcitement * i) + pGame.initialExcitementLevel) / (i + 1);
                }
                else
                {
                //Check unplayed game
                    UnplayedGame uGame = null;
                    uGame = nextGame.purchased ? Menu.FindDetailedGameByID<BackloggedGame>(nextGame.gameID) : Menu.FindDetailedGameByID<UnpurchasedGame>(nextGame.gameID);

                    numGamesUnplayed++;

                    //Check current excitement
                    avgGenreExcitement = ((avgGenreExcitement * i) + uGame.excitementLevel) / (i + 1);

                } 
            }
        }
        public void UpdateStatsForGame(int gameID)
        {
            RemoveFromGenreStats(gameID);
            AddGameInfoToAverages(gameID);
        }
        public void AddGameInfoToAverages(int gameID)
        {
            VideoGame nextGame = Menu.FindGameByID(gameID);
            if (nextGame == null)
            {
                RemoveGameFromGenre(gameID);
                SaveGenreChanges();
                RecalculateGenreStats();
                SaveGenreChanges();
                return;
            }
            //Check length
            avgGenreLength = ((avgGenreLength * (attachedGameIDs.Length - 1)) + nextGame.avgGameLength) / attachedGameIDs.Length;
            //Check played game
            if (nextGame.played)
            {
                PlayedGame pGame = null;
                if (nextGame.completed) pGame = Menu.FindDetailedGameByID<CompletedGame>(nextGame.gameID);
                if (nextGame.playing) pGame = Menu.FindDetailedGameByID<CurrentGame>(nextGame.gameID);
                if (!nextGame.playing && !nextGame.completed) pGame = Menu.FindDetailedGameByID<DroppedGame>(nextGame.gameID);

                numGamesPlayed++;

                //Check rating
                avgGenreRating = ((avgGenreRating * (numGamesPlayed - 1)) + pGame.rating) / numGamesPlayed;

                //Check completion
                //If completed completion bonus = 1 else 0
                //If completion bonus and avgGenreCompletion == 0, avgGenreCompletion still = 0 else calc average
                int completionBonus = nextGame.completed ? 1 : 0;
                avgGenreCompletion = avgGenreCompletion == 0 && completionBonus == 0 ? 0 : ((avgGenreCompletion * (numGamesPlayed - 1)) + completionBonus) / numGamesPlayed;

                //Check playtime total
                //Check playtime avg
                totalHoursPlayed += pGame.hoursPlayed;
                avgGenrePlaytime = totalHoursPlayed / numGamesPlayed;

                //Check initial excitement
                avgGenreExcitement = ((avgGenreExcitement * (attachedGameIDs.Length - 1)) + pGame.initialExcitementLevel) / attachedGameIDs.Length;
            }
            else
            {
            //Check unplayed game
                UnplayedGame uGame = null;
                uGame = nextGame.purchased ? Menu.FindDetailedGameByID<BackloggedGame>(nextGame.gameID) : Menu.FindDetailedGameByID<UnpurchasedGame>(nextGame.gameID);

                numGamesUnplayed++;

                //Check current excitement
                avgGenreExcitement = ((avgGenreExcitement * (attachedGameIDs.Length - 1)) + uGame.excitementLevel) / attachedGameIDs.Length;

            } 
            SaveGenreChanges();
        }
        public void RemoveFromGenreStats(int gameID)
        {
            VideoGame nextGame = Menu.FindGameByID(gameID);
            if (nextGame == null)
            {
                RemoveGameFromGenre(gameID);
                SaveGenreChanges();
                RecalculateGenreStats();
                SaveGenreChanges();
                return;
            }
            if (attachedGameIDs.Length == 1)
            {
                avgGenreRating = 0;
                avgGenreLength = 0;
                avgGenrePlaytime = 0;
                avgGenreExcitement = 0;
                avgGenreCompletion = 0;
                numGamesPlayed = 0;
                numGamesUnplayed = 0;
                totalHoursPlayed = 0;
                return;
            }
            //Check length
            avgGenreLength = ((avgGenreLength * attachedGameIDs.Length) - nextGame.avgGameLength) / (attachedGameIDs.Length - 1);
            //Check played game
            if (nextGame.played)
            {
                PlayedGame pGame = null;
                if (nextGame.completed) pGame = Menu.FindDetailedGameByID<CompletedGame>(nextGame.gameID);
                if (nextGame.playing) pGame = Menu.FindDetailedGameByID<CurrentGame>(nextGame.gameID);
                if (!nextGame.playing && !nextGame.completed) pGame = Menu.FindDetailedGameByID<DroppedGame>(nextGame.gameID);

                if (numGamesPlayed == 1)
                {
                    avgGenreRating = 0;
                    avgGenreLength = 0;
                    avgGenrePlaytime = 0;
                    avgGenreCompletion = 0;
                    numGamesPlayed = 0;
                    totalHoursPlayed = 0;
                    avgGenreExcitement = ((avgGenreExcitement * attachedGameIDs.Length) - pGame.initialExcitementLevel) / (attachedGameIDs.Length - 1);
                    return;
                }


                //Check rating
                avgGenreRating = ((avgGenreRating * numGamesPlayed) - pGame.rating) / (numGamesPlayed - 1);

                //Check completion
                //If completed completion bonus = 1 else 0
                //If completion bonus and avgGenreCompletion == 0, avgGenreCompletion still = 0 else calc average
                int completionBonus = nextGame.completed ? 1 : 0;
                avgGenreCompletion = avgGenreCompletion == 0 && completionBonus == 0 ? 0 : ((avgGenreCompletion * numGamesPlayed) - completionBonus) / (numGamesPlayed - 1);

                //Check playtime total
                //Check playtime avg
                totalHoursPlayed -= pGame.hoursPlayed;
                avgGenrePlaytime = totalHoursPlayed / (numGamesPlayed - 1);

                //Check initial excitement
                avgGenreExcitement = ((avgGenreExcitement * attachedGameIDs.Length) - pGame.initialExcitementLevel) / (attachedGameIDs.Length - 1);

                numGamesPlayed--;
            }
            else
            {
            //Check unplayed game
                UnplayedGame uGame = null;
                uGame = nextGame.purchased ? Menu.FindDetailedGameByID<BackloggedGame>(nextGame.gameID) : Menu.FindDetailedGameByID<UnpurchasedGame>(nextGame.gameID);

                //Check current excitement
                avgGenreExcitement = ((avgGenreExcitement * attachedGameIDs.Length) - uGame.excitementLevel) / (attachedGameIDs.Length - 1);

                numGamesUnplayed--;

            } 
            SaveGenreChanges();
        }
        public void AddGenreFranchise()
        {
            if (Menu.existingFranchises.Count - attachedFranchiseIDs.Length < 1)
            {
                //Back out if there are no franchises to attach
                Console.WriteLine($"All existing franchises are currently attached to the {genreName} genre");
                return;
            }

            //Track and index each franchise not attached to genre
            List<Franchise> unattachedFranchises = Menu.existingFranchises.ToList();
            int[] franchiseIndex = [];
            for (int i = 0; i < Menu.existingFranchises.Count; i++)
            {
                if (attachedFranchiseIDs.Contains(Menu.existingFranchises[i].franchiseID))
                {
                    unattachedFranchises.Remove(Menu.existingFranchises[i]);
                } else
                {
                    franchiseIndex = franchiseIndex.Append(i).ToArray();
                }
            }

            //Display all non attached genres
            Console.WriteLine($"Please select all franchises you would like to attach to the {genreName} genre");
            for (int i = 0; i < unattachedFranchises.Count; i++)
            {
                Console.WriteLine($"{i+1}. {unattachedFranchises[i].franchiseName}");
            }
            int[] newMatchingFranchises = Format.GetManyMenuResponses(unattachedFranchises.Count);
            for (int i = 0; i < newMatchingFranchises.Length; i++)
            {
                attachedFranchiseIDs = attachedFranchiseIDs.Append(unattachedFranchises[newMatchingFranchises[i]-1].franchiseID).ToArray();
                Menu.existingFranchises[franchiseIndex[newMatchingFranchises[i]-1]].franchiseGenreIDs = Menu.existingFranchises[franchiseIndex[newMatchingFranchises[i]-1]].franchiseGenreIDs.Append(genreID).ToArray();
            }
        }
        public void RemoveGenreFranchise()
        {
            //Retrieve selection of all attached genres to remove
            Console.WriteLine($"Please select all franchises you would like to remove from the {genreName} genre");
            for (int i = 0; i < attachedFranchiseIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.existingFranchises[Menu.FindFranchise(attachedFranchiseIDs[i])].franchiseName}");
            }
            Console.WriteLine($"\n{attachedFranchiseIDs.Length + 1}. Go back");
            int[] removedFranchises = Format.GetManyMenuResponses(attachedFranchiseIDs.Length + 1);
            Array.Sort(removedFranchises);
            if (removedFranchises.Contains(attachedFranchiseIDs.Length + 1))
            {
                return;
            }

            //Generate and present confirmation message for removal of genres
            string removalConfirmation = $"Are you sure you would like to remove all the following franchises? (y/n):";
            for (int i = 0; i < removedFranchises.Length; i++)
            {
                removalConfirmation += $"\n -- {Menu.existingFranchises[Menu.FindFranchise(attachedFranchiseIDs[removedFranchises[i]-1])].franchiseName} --";
            }
            if (!Format.GetClosedAnswer(removalConfirmation))
            {
                if (Format.GetClosedAnswer($"Would you like to remove a different selection of franchises from the {genreName} genre? (y/n)"))
                {
                    RemoveGenreFranchise();
                    return;
                }
                return;
            }

            //Remove Game from genres and genres from game
            for (int i = removedFranchises.Length - 1; i >= 0; i--)
            {
                int currentFranchiseID = Menu.FindFranchise(attachedFranchiseIDs[removedFranchises[i]-1]);
                Menu.existingFranchises[currentFranchiseID].franchiseGenreIDs = Menu.existingFranchises[currentFranchiseID].franchiseGenreIDs.Where(ID => ID != genreID).ToArray();
                attachedFranchiseIDs = attachedFranchiseIDs.Where(ID => ID != attachedFranchiseIDs[removedFranchises[i] - 1]).ToArray();
                Menu.existingFranchises[Menu.FindFranchise(currentFranchiseID)].SaveFranchiseChanges();
                SaveGenreChanges();
            }
        }
        public void SaveGenreChanges()
        {
            CSVHandler.UpdateInfoFile<Genre>(Menu.existingGenres, Menu.mainFiles.genreFile);
        }
    }
}