using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GameTrackerEx01
{
    public static class Menu
    {
        private static string settingsPath = "Game_Tracker_Settings_Info.csv";

        public class Genre {
            public string genreName {get; set;}
        }
        
        public class Franchise {
            public int franchiseID {get; set;}
            public string franchiseName {get; set;}
            public int[] franchiseEntryIDs {get; set;}
            public List<Genre> franchiseGenres {get; set;}
        }

        public static List<Genre> existingGenres = [];
        public static List<Franchise> existingFranchises = [];

        public class settingsInfo {
            public int nextGameId {get; set;}
            public int nextFranchiseId {get; set;}
            public string genreFile {get;}
            public string franchiseFile {get;}
            public string currentGameFile {get;}
            public string completedGameFile {get;}
            public string droppedGameFile {get;}
            public string unpurchasedGameFile {get;}
            public string backloggedGameFile {get;}
        }

        public static List<settingsInfo> backupFiles = [];

        public static settingsInfo mainFiles;

        public static void GetStats()
        {
            using var reader = new StreamReader(settingsPath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            backupFiles = csv.GetRecords<settingsInfo>().ToList();
            mainFiles = backupFiles[0];
            existingGenres = DownloadGenres(mainFiles.genreFile);
        }

        public static List<Genre> DownloadGenres(string filepath)
        {
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Genre>().ToList();
        }
        //ABOVE AND BELOW FUNCS CAN BE COMBINED INTO A TYPE T FUNC - Could then also use same func for all game files
        public static List<Franchise> DownloadFranchises(string filepath)
        {
            using var reader = new StreamReader(filepath);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            return csv.GetRecords<Franchise>().ToList();
        }

        public static int GetAndUpdateNextGameID()
        {
            mainFiles.nextGameId++;
            return mainFiles.nextGameId - 1;
        }

        public static void AddNewGenre()
        {
            Console.WriteLine("Please write the name of the new genre below");
            Genre newGenre = new Genre();

            //CURRENTLY NO COMMA CHECK!!!!!!!
            newGenre.genreName = Console.ReadLine();
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
            existingGenres.Add(newGenre);
            Console.WriteLine($"{newGenre.genreName} has been added to the existing list of genres");
            if (Format.GetClosedAnswer("Would you like to add another genre? (y/n)"))
            {
                AddNewGenre();
            }
            CSVHandler.UpdateGenreFile(existingGenres, $"gameGenres_20260927225625.csv");
        }

        public static int AddNewFranchise()
        {
            Console.WriteLine("Please write the name of the new franchise below");
            Franchise newFranchise = new Franchise();

            //Get the franchise name (CURRENTLY NO COMMA CHECK!!!!!!)
            newFranchise.franchiseName = Console.ReadLine();
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
            newFranchise.franchiseID = mainFiles.nextFranchiseId;
            mainFiles.nextFranchiseId++;


            //Get franchise genres
            if (Menu.existingGenres.Count > 0)
            {
                Console.WriteLine($"Please select the appropriate genres for the franchise of {newFranchise.franchiseName}");
                for (int i = 0; i < existingGenres.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {existingGenres[i].genreName}");
                }
                int[] matchingGenres = Format.GetManyMenuResponses(existingGenres.Count);
                for (int i = 0; i < matchingGenres.Length; i++)
                {
                    newFranchise.franchiseGenres.Add(Menu.existingGenres[matchingGenres[i]-1]);
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
                        newFranchise.franchiseGenres.Add(existingGenres[i]);
                    }
                }
            }
            existingFranchises.Add(newFranchise);
            Console.WriteLine($"{newFranchise.franchiseName} has been added to the existing list of franchises");
            CSVHandler.UpdateFranchiseFile(existingFranchises, $"gameFranchises_20260927225625.csv");
            return newFranchise.franchiseID;
        }

        public static void AddGameToFranchise(int gameId, int franchiseID)
        {
            int franchiseIndex = FindFranchise(franchiseID);
            existingFranchises[franchiseIndex].franchiseEntryIDs = existingFranchises[franchiseIndex].franchiseEntryIDs.Append(gameId).ToArray();
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

        public static void HomePage()
        {
            
        }

        public static void ViewBackups()
        {

        }

        
    }
}