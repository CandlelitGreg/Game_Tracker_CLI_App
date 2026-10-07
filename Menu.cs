using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace GameTrackerEx01
{
    public static class Menu
    {
        public static string settingsPath = "saveFiles/Game_Tracker_Settings_Info.csv";

        public static List<Genre> existingGenres = [];
        public static List<Franchise> existingFranchises = [];
        public static List<VideoGame> existingVideoGames = [];
        public static List<VideoGame> existingReplays = [];


        public static List<CurrentGame> existingCurrentGames = [];
        public static List<CompletedGame> existingCompletedGames = [];
        public static List<DroppedGame> existingDroppedGames = [];
        public static List<UnpurchasedGame> existingUnpurchasedGames = [];
        public static List<BackloggedGame> existingBackloggedGames = [];

        public class settingsInfo {
            public int nextGameID {get; set;}
            public int nextFranchiseID {get; set;}
            public int nextGenreID {get;set;}
            public string genreFile {get; set;}
            public string franchiseFile {get; set;}
            public string currentGameFile {get; set;}
            public string completedGameFile {get; set;}
            public string droppedGameFile {get; set;}
            public string unpurchasedGameFile {get; set;}
            public string backloggedGameFile {get; set;}
        }

        public static List<settingsInfo> backupFiles = [];

        public static settingsInfo mainFiles;
        
        public static void GetStats()
        {
            backupFiles = DownloadInfo<settingsInfo>(settingsPath);
            mainFiles = backupFiles[backupFiles.Count-1];
            existingGenres = DownloadInfo<Genre>(mainFiles.genreFile);
            existingFranchises = DownloadInfo<Franchise>(mainFiles.franchiseFile);
            existingCurrentGames = DownloadInfo<CurrentGame>(mainFiles.currentGameFile);
            existingCompletedGames = DownloadInfo<CompletedGame>(mainFiles.completedGameFile);
            existingDroppedGames = DownloadInfo<DroppedGame>(mainFiles.droppedGameFile);
            existingUnpurchasedGames = DownloadInfo<UnpurchasedGame>(mainFiles.unpurchasedGameFile);
            CheckAllReleaseDates();
            existingBackloggedGames = DownloadInfo<BackloggedGame>(mainFiles.backloggedGameFile);
            var allGames = GetAllVideoGames();
            existingVideoGames.AddRange(allGames.vgs);
            existingReplays.AddRange(allGames.replays);
            SortGameLists();
            RecalculateAllGenreStats();
            Console.WriteLine($"There are {existingVideoGames.Count} total games in the tracker\n");
            for (int i = 0; i < existingVideoGames.Count; i++)
            {
                Console.WriteLine("\n" +existingVideoGames[i].DisplayGameDetails() + "\n");
            }

            Console.WriteLine($"\nThere are {existingFranchises.Count} total franchises in the tracker\n");
            for (int i = 0; i < existingFranchises.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {existingFranchises[i].franchiseName}");
            }

            Console.WriteLine($"\nThere are {existingGenres.Count} total genres in the tracker\n");
            for (int i = 0; i < existingGenres.Count; i++)
            {
                Console.WriteLine($"{i + 1}.");
                existingGenres[i].ViewGenreAverages();
            }
        }
        public static List<T> DownloadInfo<T>(string filepath)
        {
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            switch (true)
            {
                case var _ when typeof(T) == typeof(CurrentGame):
                    csv.Context.RegisterClassMap<RetrieveCurrentGameMap>();
                    break;
                case var _ when typeof(T) == typeof(DroppedGame):
                    csv.Context.RegisterClassMap<RetrieveDroppedGameMap>();
                    break;
                case var _ when typeof(T) == typeof(CompletedGame):
                    csv.Context.RegisterClassMap<RetrieveCompletedGameMap>();
                    break;
                case var _ when typeof(T) == typeof(UnpurchasedGame):
                    csv.Context.RegisterClassMap<RetrieveUnpurchasedGameMap>();
                    break;
                case var _ when typeof(T) == typeof(BackloggedGame):
                    csv.Context.RegisterClassMap<RetrieveBackloggedGameMap>();
                    break;
                case var _ when typeof(T) == typeof(Genre):
                    csv.Context.RegisterClassMap<RetrieveGenreMap>();
                    break;
                case var _ when typeof(T) == typeof(Franchise):
                    csv.Context.RegisterClassMap<RetrieveFranchiseMap>();
                    break;
            }
            return csv.GetRecords<T>().ToList();
        }
        public static void SortGameLists()
        {
            existingVideoGames.Sort((x, y) => x.gameID.CompareTo(y.gameID));
            existingReplays.Sort((x, y) => x.gameID.CompareTo(y.gameID));
            existingCurrentGames.Sort((x, y) => x.gameID.CompareTo(y.gameID));
            existingCompletedGames.Sort((x, y) => x.gameID.CompareTo(y.gameID));
            existingDroppedGames.Sort((x, y) => x.gameID.CompareTo(y.gameID));
            existingUnpurchasedGames.Sort((x, y) => x.gameID.CompareTo(y.gameID));
            existingBackloggedGames.Sort((x, y) => x.gameID.CompareTo(y.gameID));
        }
        public static int GetAndUpdateNextGameID()
        {
            mainFiles.nextGameID++;
            backupFiles[backupFiles.Count - 1] = mainFiles;
            CSVHandler.UpdateInfoFile<settingsInfo>(backupFiles, settingsPath);
            return mainFiles.nextGameID - 1;
        }
        public static int GetAndUpdateNextGenreID()
        {
            mainFiles.nextGenreID++;
            backupFiles[backupFiles.Count - 1] = mainFiles;
            CSVHandler.UpdateInfoFile<settingsInfo>(backupFiles, settingsPath);
            return mainFiles.nextGenreID - 1;
        }
        public static void AddNewGenre()
        {
            Console.WriteLine("Please write the name of the new genre below");
            Genre newGenre = new Genre();

            newGenre.genreName = Format.CheckForCommas(Console.ReadLine(), "new genre");
            for (int i = 0; i < existingGenres.Count; i++)
            {
                if (existingGenres[i].genreName.ToLower() == newGenre.genreName.ToLower())
                {
                    Console.WriteLine($"{newGenre.genreName} already exists");
                    if (Format.GetClosedAnswer("Would you like to add a different genre? (y/n)"))
                    {
                        AddNewGenre();
                    }
                    return;
                }
            }
            newGenre.genreID = GetAndUpdateNextGenreID();
            existingGenres.Add(newGenre);
            Console.WriteLine($"{newGenre.genreName} has been added to the existing list of genres");
            if (Format.GetClosedAnswer("Would you like to add another genre? (y/n)"))
            {
                AddNewGenre();
            }
            CSVHandler.UpdateInfoFile<Genre>(existingGenres, mainFiles.genreFile);
        }
        public static int FindGenre(int searchID)
        {
            int highIndex = existingGenres.Count-1;
            int lowIndex = 0;

            while (lowIndex <= highIndex)
            {
                int midIndex = lowIndex + ((highIndex - lowIndex) / 2);

                
                if (existingGenres[midIndex].genreID == searchID)
                {
                    return midIndex;
                }
                if (existingGenres[midIndex].genreID < searchID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
            }

            Console.WriteLine($"Fell through: High index: {highIndex}, Low index: {lowIndex}, Search ID: {searchID}");
            return -1;
        }
        public static int AddNewFranchise()
        {
            Console.WriteLine("Please write the name of the new franchise below");
            Franchise newFranchise = new Franchise();

            //Get the franchise name (CURRENTLY NO COMMA CHECK!!!!!!)
            newFranchise.franchiseName = Format.CheckForCommas(Console.ReadLine(), "franchise name");
            for (int i = 0; i < existingFranchises.Count; i++)
            {
                if (existingFranchises[i].franchiseName.ToLower() == newFranchise.franchiseName.ToLower())
                {
                    Console.WriteLine($"{newFranchise.franchiseName} already exists");
                    if (Format.GetClosedAnswer("Would you like to add a different franchise? (y/n)"))
                    {
                        return AddNewFranchise();
                    }
                    return -1;
                }
            }


            //Set franchise id
            newFranchise.franchiseID = mainFiles.nextFranchiseID;
            mainFiles.nextFranchiseID++;
            backupFiles[backupFiles.Count - 1] = mainFiles;
            CSVHandler.UpdateInfoFile<settingsInfo>(backupFiles, settingsPath);
            


            //Get franchise genres
            if (existingGenres.Count > 0)
            {
                newFranchise.franchiseGenreIDs = [];
                Console.WriteLine($"Please select the appropriate genres for the franchise of {newFranchise.franchiseName}");
                for (int i = 0; i < existingGenres.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {existingGenres[i].genreName}");
                }
                Console.WriteLine($"\n{existingGenres.Count + 1}. None of the above");
                int[] matchingGenres = Format.GetManyMenuResponses(existingGenres.Count + 1);
                if (!matchingGenres.Contains(existingGenres.Count + 1))
                {
                    for (int i = 0; i < matchingGenres.Length; i++)
                    {
                        newFranchise.franchiseGenreIDs = newFranchise.franchiseGenreIDs.Append(existingGenres[matchingGenres[i]-1].genreID).ToArray();
                        existingGenres[matchingGenres[i]-1].AttachFranchiseToGenre(newFranchise.franchiseID);
                        existingGenres[matchingGenres[i]-1].SaveGenreChanges();
                    }
                }
            }
            

            //Get fitting new game genres
            if (Format.GetClosedAnswer($"Would you like to create a new genre to attach to the franchise of {newFranchise.franchiseName}? (y/n)"))
            {
                int currentGenreCount = existingGenres.Count;
                AddNewGenre();
                if (existingGenres.Count > currentGenreCount)
                {
                    for (int i = currentGenreCount; i < existingGenres.Count; i++)
                    {
                        newFranchise.franchiseGenreIDs = newFranchise.franchiseGenreIDs.Append(existingGenres[i].genreID).ToArray();
                        existingGenres[i].AttachFranchiseToGenre(newFranchise.franchiseID);
                        existingGenres[i].SaveGenreChanges();
                    }
                }
            }
            existingFranchises.Add(newFranchise);
            Console.WriteLine($"{newFranchise.franchiseName} has been added to the existing list of franchises");
            CSVHandler.UpdateInfoFile<Franchise>(existingFranchises, mainFiles.franchiseFile);
            return newFranchise.franchiseID;
        }
        public static void AddGameToFranchise(int gameID, int franchiseID)
        {
            int franchiseIndex = FindFranchise(franchiseID);
            // Console.WriteLine($"Franchise ID: {franchiseID}, Franchise Index: {franchiseIndex}");
            // Console.WriteLine($"Game ID: {gameID}");
            existingFranchises[franchiseIndex].franchiseEntryIDs = existingFranchises[franchiseIndex].franchiseEntryIDs.Append(gameID).ToArray();
            CSVHandler.UpdateInfoFile<Franchise>(existingFranchises, mainFiles.franchiseFile);
        }
        public static int FindFranchise(int searchID)
        {
            int highIndex = existingFranchises.Count-1;
            int lowIndex = 0;
            while (lowIndex <= highIndex)
            {
                int midIndex = lowIndex + (highIndex - lowIndex) / 2;
                if (existingFranchises[midIndex].franchiseID == searchID)
                {
                    return midIndex;
                }
                if (existingFranchises[midIndex].franchiseID < searchID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
            }
            return -1;

        }
        public static VideoGame FindGameByID(int searchID)
        {
            int highIndex = existingVideoGames.Count-1;
            int lowIndex = 0;
            while (lowIndex <= highIndex)
            {
                int midIndex = lowIndex + (highIndex - lowIndex) / 2;
                if (existingVideoGames[midIndex].gameID == searchID)
                {
                    return existingVideoGames[midIndex];
                }
                if (existingVideoGames[midIndex].gameID < searchID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
            }
            return null;
        }
        public static void UpdateGameByID(VideoGame updatedGame)
        {
            int highIndex = existingVideoGames.Count-1;
            int lowIndex = 0;
            int midIndex = 0;
            while (lowIndex <= highIndex)
            {
                midIndex = lowIndex + (highIndex - lowIndex) / 2;
                if (existingVideoGames[midIndex].gameID == updatedGame.gameID)
                {
                    break;
                }
                if (existingVideoGames[midIndex].gameID < updatedGame.gameID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
            }
            if (existingVideoGames[midIndex].gameID == updatedGame.gameID)
            {
                existingVideoGames[midIndex] = updatedGame;
            }
            
        }
        public static T FindDetailedGameByID<T>(int searchID)
        {
            var gameList = new List<T>();
            switch (true)
            {
                case var _ when typeof(T) == typeof(CurrentGame):
                    gameList= existingCurrentGames as List<T>;
                    break;
                case var _ when typeof(T) == typeof(CompletedGame):
                    gameList = existingCompletedGames as List<T>;
                    break;
                case var _ when typeof(T) == typeof(DroppedGame):
                    gameList = existingDroppedGames as List<T>;
                    break;
                case var _ when typeof(T) == typeof(UnpurchasedGame):
                    gameList = existingUnpurchasedGames as List<T>;
                    break;
                case var _ when typeof(T) == typeof(BackloggedGame):
                    gameList = existingBackloggedGames as List<T>;
                    break;
                default:
                    return default(T);
            }
            int highIndex = gameList.Count-1;
            int lowIndex = 0;
            while (lowIndex <= highIndex)
            {
                int midIndex = lowIndex + (highIndex - lowIndex) / 2;
                VideoGame midGame = gameList[midIndex] as VideoGame;
                if (midGame.gameID == searchID)
                {
                    return gameList[midIndex];
                }
                if (midGame.gameID < searchID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
            }
            return default(T);
        }
        public static VideoGame FindGameByTitle(string searchTitle)
        {
            for (int i = 0; i < existingVideoGames.Count; i++)
            {
                if (existingVideoGames[i].gameName.ToLower() == searchTitle.ToLower())
                {
                    return existingVideoGames[i];
                }
            }
            return null;
        }
        public static (List<VideoGame> vgs, List<VideoGame> replays) GetAllVideoGames()
        {
            //Get replay ids
            List<VideoGame> replays = [];
            List<VideoGame> vgs = [];
            int[] replayIDs = [];
            for (int i = 0; i < existingCompletedGames.Count; i++)
            {
                if (existingCompletedGames[i].replayID.Length > 0)
                {
                    foreach (int ID in existingCompletedGames[i].replayID)
                    {
                        replayIDs = replayIDs.Append(ID).ToArray();
                    }
                }
            }

            var completedGames = SeperateReplaysFromList<CompletedGame>(existingCompletedGames, replayIDs);
            vgs.AddRange(completedGames.vgs);
            replays.AddRange(completedGames.replays);

            var currentGames = SeperateReplaysFromList<CurrentGame>(existingCurrentGames, replayIDs);
            vgs.AddRange(currentGames.vgs);
            replays.AddRange(currentGames.replays);

            var droppedGames = SeperateReplaysFromList<DroppedGame>(existingDroppedGames, replayIDs);
            vgs.AddRange(droppedGames.vgs);
            replays.AddRange(droppedGames.replays);

            var backloggedGames = SeperateReplaysFromList<BackloggedGame>(existingBackloggedGames, replayIDs);
            vgs.AddRange(backloggedGames.vgs);
            replays.AddRange(backloggedGames.replays);

            var unpurchasedGames = SeperateReplaysFromList<UnpurchasedGame>(existingUnpurchasedGames, replayIDs);
            vgs.AddRange(unpurchasedGames.vgs);
            replays.AddRange(unpurchasedGames.replays);

            return (vgs, replays);
            
        } 
        public static (List<VideoGame> vgs, List<VideoGame> replays) SeperateReplaysFromList<T>(List<T> inputList, int[] replayIDs) where T : VideoGame
        {
            List<VideoGame> vgs = [];
            List<VideoGame> replays = [];
            for (int i = 0; i < inputList.Count; i++)
            {
                if (replayIDs.Contains(inputList[i].gameID))
                {
                    replays.Add(inputList[i]);
                } else {
                    vgs.Add(inputList[i]);
                }
            }
            return (vgs, replays);
        }
        public static void CheckAllReleaseDates()
        {
            for (int i = 0; i < existingUnpurchasedGames.Count; i++)
            {
                if (!existingUnpurchasedGames[i].released)
                {
                    existingUnpurchasedGames[i].CheckIfReleased();
                }
            }
        }       
        public static void RecalculateAllGenreStats()
        {
            for (int i = 0; i < existingGenres.Count; i++)
            {
                existingGenres[i].RecalculateGenreStats();
            }
        }        
        public static void ViewBackups()
        {

        }        
    }
}