using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GameTrackerEx01
{

    public sealed class FranchiseMap : ClassMap<Franchise>
    {
        public FranchiseMap()
        {
            Map(m => m.franchiseID);
            Map(m => m.franchiseName);
            Map(m => m.franchiseEntryIDs).Convert(args => string.Join(";", args.Value.franchiseEntryIDs));
            Map(m => m.franchiseGenreIDs).Convert(args => string.Join(";", args.Value.franchiseGenreIDs));
            Map(m => m.avgFranchiseRating);
        }
    }

    public sealed class RetrieveFranchiseMap : ClassMap<Franchise>
    {
        public RetrieveFranchiseMap()
        {
            Map(m => m.franchiseID);
            Map(m => m.franchiseName);
            Map(m => m.franchiseEntryIDs).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("franchiseEntryIDs")) ? Array.Empty<int>() : args.Row.GetField("franchiseEntryIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.franchiseGenreIDs).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("franchiseGenreIDs")) ? Array.Empty<int>() : args.Row.GetField("franchiseGenreIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.avgFranchiseRating);
        }
    }
    
    public class Franchise {
        public int franchiseID {get; set;}
        public string franchiseName {get; set;}
        public int[] franchiseEntryIDs {get; set;} = [];
        public int[] franchiseGenreIDs {get; set;} = [];
        public float avgFranchiseRating {get;set;}

        public void OpenFranchise()
        {
            Console.WriteLine($"What would you like to do with {franchiseName}?\n");
            Console.WriteLine("1. View franchise details");
            Console.WriteLine("2. Edit franchise details");
            Console.WriteLine("3. View similar franchises");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(3, $"What would you like to do with {franchiseName}?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    Console.WriteLine(ViewFranchiseDetails());
                    break;
                case 2:
                    EditFranchiseDetails();
                    break;
                case 3:
                    //Add functionality to view similar games
                    // ViewSimilarGames();
                    break;
            }
            if (Format.GetClosedAnswer($"Do you want to keep interacting with {franchiseName}? (y/n)"))
            {
                OpenFranchise();
            }
            return;
        }
        public string ViewFranchiseDetails()
        {
            string franchiseDetails = $"Franchise Name: {franchiseName}\n" +
                                        $"Average Franchise Rating: {avgFranchiseRating}\n" +
                                        $"\nGames in Franchise:\n";
            for (int i = 0; i < franchiseEntryIDs.Length; i++)
            {   
                franchiseDetails += $"{i + 1}. {Menu.FindGameByID(franchiseEntryIDs[i]).gameName}\n";
            }
            franchiseDetails += $"\nGenres associated with Franchise:\n";
            for (int i = 0; i < franchiseGenreIDs.Length; i++)
            {
                int genreIndex = Menu.FindGenre(franchiseGenreIDs[i]);
                if (genreIndex != -1)
                {
                    Genre genre = Menu.existingGenres[genreIndex];
                    franchiseDetails += $"{i + 1}: {genre.genreName}\n";
                }
            }
            return franchiseDetails;
        }
        public void EditFranchiseDetails()
        {
            Console.WriteLine($"What element do you want to edit from the franchise of {franchiseName}?\n");
            Console.WriteLine("1. Franchise Games");
            Console.WriteLine("2. Franchise Genres");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What would you like to do with {franchiseName}?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    EditAttachedGames();
                    break;
                case 2:
                    EditGenres();
                    break;
            }
            if (Format.GetClosedAnswer($"Do you want to keep editing {franchiseName}'s details? (y/n)"))
            {
                OpenFranchise();
            }
            return;
        }
        public void EditAttachedGames()
        {
            Console.WriteLine($"These are the current games for {franchiseName}:");
            for (int i = 0; i < franchiseEntryIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.FindGameByID(franchiseEntryIDs[i]).gameName}");
            }
            Console.WriteLine($"\nWhat would you like to do with {franchiseName}'s entries?");
            Console.WriteLine("1. Add an entry");
            Console.WriteLine("2. Remove an entry");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What would you like to do with {franchiseName}'s entries?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    AddGames();
                    break;
                case 2:
                    RemoveGames();
                    break;
            }
            SaveFranchiseChanges();
            if (Format.GetClosedAnswer($"Would you like to continue editing {franchiseName}'s entries? (y/n)"))
            {
                EditAttachedGames();
            }
        }
        public void AddGames()
        {
            string gameName = Format.AskForInput($"the name of the game you would like to add to the franchise of {franchiseName}");
            VideoGame searchedGame = Menu.FindGameByTitle(gameName);
            if (searchedGame != null)
            {
                if(searchedGame.franchiseID == franchiseID)
                {
                    if (Format.GetClosedAnswer($"{searchedGame} is already a part of the franchise, {franchiseName}.\nWould you like to add a different game? (y/n)"))
                    {
                        AddGames();
                    }
                }
                else if(searchedGame.franchiseID == -1)
                {
                    searchedGame.franchiseID = franchiseID;
                    franchiseEntryIDs = franchiseEntryIDs.Append(searchedGame.gameID).ToArray();
                    searchedGame.SaveGameUpdates();
                }
                else if (searchedGame.franchiseID != -1 && Format.GetClosedAnswer($"{searchedGame.gameName} is currently part of a different franchise.\nAre you sure you want to move {searchedGame.gameName} franchises from {Menu.existingFranchises[Menu.FindFranchise(searchedGame.franchiseID)].franchiseName} to {franchiseName}? (y/n)"))
                {
                    Menu.existingFranchises[Menu.FindFranchise(searchedGame.franchiseID)].RemoveSelectedGame(searchedGame.gameID);
                    searchedGame.franchiseID = franchiseID;
                    franchiseEntryIDs = franchiseEntryIDs.Append(searchedGame.gameID).ToArray();
                    searchedGame.SaveGameUpdates();
                } 
            } 
            else if (Format.GetClosedAnswer($"{gameName} could not be found. Do you want to add it to your records? (y/n)"))
            {
                searchedGame = new VideoGame();
                searchedGame.AddGameFromFranchise(gameName, franchiseID);
            }
            SaveFranchiseChanges();
        }
        public void RemoveGames()
        {
            //Retrieve selection of all attached genres to remove
            Console.WriteLine($"Please select all games you would like to remove from {franchiseName}");
            for (int i = 0; i < franchiseEntryIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.FindGameByID(franchiseEntryIDs[i]).gameName}");
            }
            Console.WriteLine($"\n{franchiseEntryIDs.Length + 1}. Go back");
            int[] removedGames = Format.GetManyMenuResponses(franchiseEntryIDs.Length + 1);
            Array.Sort(removedGames);
            if (removedGames.Contains(franchiseEntryIDs.Length + 1))
            {
                return;
            }

            //Generate and present confirmation message for removal of genres
            string removalConfirmation = $"Are you sure you would like to remove all the following games? (y/n):";
            for (int i = 0; i < removedGames.Length; i++)
            {
                removalConfirmation += $"\n -- {Menu.FindGameByID(franchiseEntryIDs[removedGames[i]-1]).gameName} --";
            }
            if (!Format.GetClosedAnswer(removalConfirmation))
            {
                if (Format.GetClosedAnswer($"Would you like to remove a different selection of games from {franchiseName}? (y/n)"))
                {
                    RemoveGames();
                    return;
                }
                return;
            }

            //Remove Game from franchise and franchise from games
            for (int i = removedGames.Length - 1; i >= 0; i--)
            {
                RemoveSelectedGame(franchiseEntryIDs[removedGames[i]-1]);
            }
        }
        public void RemoveSelectedGame(int gameID)
        {
            //Remove game from genre
            Menu.FindGameByID(gameID).franchiseID = -1;
            Menu.FindGameByID(gameID).SaveGameUpdates();
            franchiseEntryIDs = franchiseEntryIDs.Where(ID => ID != gameID).ToArray();
            SaveFranchiseChanges();
        }
        public void EditGenres()
        {
            Console.WriteLine($"These are the current genres for {franchiseName}:");
            for (int i = 0; i < franchiseGenreIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.existingGenres[Menu.FindGenre(franchiseGenreIDs[i])].genreName}");
            }
            Console.WriteLine($"\nWhat would you like to do with {franchiseName}'s genres?");
            Console.WriteLine("1. Add a genre");
            Console.WriteLine("2. Remove a genre");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What would you like to do with {franchiseName}'s genres?");
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
            SaveFranchiseChanges();
            if (Format.GetClosedAnswer($"Would you like to continue editing {franchiseName}'s genres? (y/n)"))
            {
                EditGenres();
            }
        }
        public void AddGenres()
        {
            if (Menu.existingGenres.Count - franchiseGenreIDs.Length < 1)
            {
                //Add new genre if there are no unattached existing genres
                if (Format.GetClosedAnswer($"All existing genres are currently attached to {franchiseName}\nWould you like to create a new genre? (y/n)"))
                {
                    int currentGenreCount = Menu.existingGenres.Count;
                    Menu.AddNewGenre();
                    if (Menu.existingGenres.Count > currentGenreCount)
                    {
                        for (int i = currentGenreCount; i < Menu.existingGenres.Count; i++)
                        {
                            franchiseGenreIDs = franchiseGenreIDs.Append(Menu.existingGenres[i].genreID).ToArray();
                            Menu.existingGenres[i].AttachFranchiseToGenre(franchiseID);
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
                if (franchiseGenreIDs.Contains(Menu.existingGenres[i].genreID))
                {
                    unattachedGameGenres.Remove(Menu.existingGenres[i]);
                } else
                {
                    genreIndex = genreIndex.Append(i).ToArray();
                }
            }

            //Display all non attached genres
            Console.WriteLine($"Please select all genres you would like to add to {franchiseName}");
            for (int i = 0; i < unattachedGameGenres.Count; i++)
            {
                Console.WriteLine($"{i+1}. {unattachedGameGenres[i].genreName}");
            }
            Console.WriteLine($"\n{unattachedGameGenres.Count + 1}. Create a new genre for {franchiseName}");
            int[] newMatchingGenres = Format.GetManyMenuResponses(unattachedGameGenres.Count + 1);
            for (int i = 0; i < newMatchingGenres.Length; i++)
            {
                if (newMatchingGenres[i] == unattachedGameGenres.Count + 1)
                {
                    //Get fitting new game genres
                    if (Format.GetClosedAnswer($"Would you like to create a new genre to attach to {franchiseName}? (y/n)"))
                    {
                        int currentGenreCount = Menu.existingGenres.Count;
                        Menu.AddNewGenre();
                        if (Menu.existingGenres.Count > currentGenreCount)
                        {
                            for (int q = currentGenreCount; q < Menu.existingGenres.Count; q++)
                            {
                                franchiseGenreIDs = franchiseGenreIDs.Append(Menu.existingGenres[q].genreID).ToArray();
                                Menu.existingGenres[q].AttachFranchiseToGenre(franchiseID);
                            }
                        }
                    }
                } 
                else 
                {
                    franchiseGenreIDs = franchiseGenreIDs.Append(unattachedGameGenres[newMatchingGenres[i]-1].genreID).ToArray();
                    Menu.existingGenres[genreIndex[newMatchingGenres[i]-1]].AttachFranchiseToGenre(franchiseID);
                }

            }
        }
        public void RemoveGenres()
        {
            //Retrieve selection of all attached genres to remove
            Console.WriteLine($"Please select all genres you would like to remove from {franchiseName}");
            for (int i = 0; i < franchiseGenreIDs.Length; i++)
            {
                Console.WriteLine($"{i+1}. {Menu.existingGenres[Menu.FindGenre(franchiseGenreIDs[i])].genreName}");
            }
            Console.WriteLine($"\n{franchiseGenreIDs.Length + 1}. Go back");
            int[] removedGenres = Format.GetManyMenuResponses(franchiseGenreIDs.Length + 1);
            Array.Sort(removedGenres);
            if (removedGenres.Contains(franchiseGenreIDs.Length + 1))
            {
                return;
            }

            //Generate and present confirmation message for removal of genres
            string removalConfirmation = $"Are you sure you would like to remove all the following genres? (y/n):";
            for (int i = 0; i < removedGenres.Length; i++)
            {
                removalConfirmation += $"\n -- {Menu.existingGenres[Menu.FindGenre(franchiseGenreIDs[removedGenres[i]-1])].genreName} --";
            }
            if (!Format.GetClosedAnswer(removalConfirmation))
            {
                if (Format.GetClosedAnswer($"Would you like to remove a different selection of genres from {franchiseName}? (y/n)"))
                {
                    RemoveGenres();
                    return;
                }
                return;
            }

            //Remove Game from genres and genres from game
            for (int i = removedGenres.Length - 1; i >= 0; i--)
            {
                Menu.existingGenres[Menu.FindGenre(franchiseGenreIDs[removedGenres[i]-1])].RemoveFranchiseFromGenre(franchiseID);
                franchiseGenreIDs = franchiseGenreIDs.Where(ID => ID != franchiseGenreIDs[removedGenres[i] - 1]).ToArray();
            }
        }
        public void SaveFranchiseChanges()
        {
            CSVHandler.UpdateInfoFile<Franchise>(Menu.existingFranchises, Menu.mainFiles.franchiseFile);
        }
    }
}