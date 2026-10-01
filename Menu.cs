using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GameTrackerEx01
{
    public static class Menu
    {
        public static string settingsPath = "saveFiles/Game_Tracker_Settings_Info.csv";

        public sealed class GenreMap : ClassMap<Genre>
        {
            public GenreMap()
            {
                Map(m => m.genreID);
                Map(m => m.genreName);
                Map(m => m.attachedFranchiseIDs).Convert(args => string.Join(";", args.Value.attachedFranchiseIDs));
                Map(m => m.attachedGameIDs).Convert(args => string.Join(";", args.Value.attachedGameIDs));
                Map(m => m.avgGenreRating);
            }
        }

        public sealed class RetrieveGenreMap : ClassMap<Genre>
        {
            public RetrieveGenreMap()
            {
                Map(m => m.genreID);
                Map(m => m.genreName);
                Map(m => m.attachedFranchiseIDs).Convert(args => args.Row.GetField("attachedFranchiseIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
                Map(m => m.attachedGameIDs).Convert(args => args.Row.GetField("attachedGameIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
                Map(m => m.avgGenreRating);
            }
        }

        public class Genre {
            public int genreID {get;set;}
            public string genreName {get; set;}
            public int[] attachedFranchiseIDs {get;set;} = [];
            public int[] attachedGameIDs {get;set;} = [];
            public float avgGenreRating {get;set;}

            public void AttachFranchiseToGenre(int franchiseID)
            {
                attachedFranchiseIDs = attachedFranchiseIDs.Append(franchiseID).ToArray();
            }

            public void AttachGameToGenre(int gameID)
            {
                attachedGameIDs = attachedGameIDs.Append(gameID).ToArray();
            }
        }

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
                Map(m => m.franchiseEntryIDs).Convert(args => args.Row.GetField("franchiseEntryIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
                Map(m => m.franchiseGenreIDs).Convert(args => args.Row.GetField("franchiseGenreIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
                Map(m => m.avgFranchiseRating);
            }
        }
        
        public class Franchise {
            public int franchiseID {get; set;}
            public string franchiseName {get; set;}
            public int[] franchiseEntryIDs {get; set;} = [];
            public int[] franchiseGenreIDs {get; set;} = [];
            public float avgFranchiseRating {get;set;}
        }

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
            existingBackloggedGames = DownloadInfo<BackloggedGame>(mainFiles.backloggedGameFile);
            var allGames = GetAllVideoGames();
            existingVideoGames = allGames.vgs;
            existingReplays = allGames.replays;
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
                // case var _ when typeof(T) == typeof(PlayedGame):
                //     csv.Context.RegisterClassMap<PlayedGameMap>();
                //     break;
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
                // case var _ when typeof(T) == typeof(VideoGame):
                //     csv.Context.RegisterClassMap<VideoGameMap>();
                //     break;
            }
            return csv.GetRecords<T>().ToList();
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
            // if (searchID == existingGenres[existingGenres.Count-1].genreID)
            // {
            //     return existingGenres.Count-1;
            // }
            int highIndex = existingGenres.Count-1;
            int lowIndex = 0;
            int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
            while (lowIndex < highIndex)
            {
                if (existingGenres[midIndex].genreID == searchID)
                {
                    return midIndex;
                }
                if (existingGenres[midIndex].genreID > searchID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
                midIndex = lowIndex + ((highIndex - lowIndex) / 2);
            }
            if (existingGenres[highIndex].genreID == searchID)
            {
                return highIndex;
            } else {
                return lowIndex;
            }
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
                int[] matchingGenres = Format.GetManyMenuResponses(existingGenres.Count);
                for (int i = 0; i < matchingGenres.Length; i++)
                {
                    newFranchise.franchiseGenreIDs = newFranchise.franchiseGenreIDs.Append(existingGenres[matchingGenres[i]-1].genreID).ToArray();
                    existingGenres[matchingGenres[i]-1].AttachFranchiseToGenre(newFranchise.franchiseID);
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
                    }
                }
            }
            existingFranchises.Add(newFranchise);
            Console.WriteLine($"{newFranchise.franchiseName} has been added to the existing list of franchises");
            CSVHandler.UpdateInfoFile<Franchise>(existingFranchises, mainFiles.franchiseFile);
            return newFranchise.franchiseID;
        }

        public static void AddGameToFranchise(int gameId, int franchiseID)
        {
            int franchiseIndex = FindFranchise(franchiseID);
            existingFranchises[franchiseIndex].franchiseEntryIDs = existingFranchises[franchiseIndex].franchiseEntryIDs.Append(gameId).ToArray();
            CSVHandler.UpdateInfoFile<Franchise>(existingFranchises, mainFiles.franchiseFile);
        }

        public static int FindFranchise(int searchID)
        {
            int highIndex = existingFranchises.Count-1;
            int lowIndex = 0;
            int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
            while (lowIndex < highIndex)
            {
                if (existingFranchises[midIndex].franchiseID == searchID)
                {
                    return midIndex;
                }
                if (existingFranchises[midIndex].franchiseID > searchID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
                midIndex = lowIndex + ((highIndex - lowIndex) / 2);
            }
            if (existingFranchises[highIndex].franchiseID == searchID)
            {
                return highIndex;
            } else {
                return lowIndex;
            }

        }

        public static VideoGame FindGameByID(int searchID)
        {
            int highIndex = existingVideoGames.Count-1;
            int lowIndex = 0;
            int midIndex = lowIndex + ((highIndex - lowIndex) / 2);
            while (lowIndex < highIndex)
            {
                if (existingVideoGames[midIndex].gameID == searchID)
                {
                    return existingVideoGames[midIndex];
                }
                if (existingVideoGames[midIndex].gameID > searchID)
                {
                    lowIndex = midIndex + 1;
                }
                else
                {
                    highIndex = midIndex - 1;
                }
                midIndex = lowIndex + ((highIndex - lowIndex) / 2);
            }
            if (existingVideoGames[lowIndex].gameID == searchID)
            {
                return existingVideoGames[lowIndex];
            } else if (existingVideoGames[highIndex].gameID == searchID)
            {
                return existingVideoGames[highIndex];
            } else {
                return null;
            }
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

        public static void HomePage()
        {
            
        }

        public static void ViewBackups()
        {

        }

        
    }
}