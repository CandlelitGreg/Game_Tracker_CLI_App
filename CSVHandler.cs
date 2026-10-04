using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GameTrackerEx01
{
    public static class CSVHandler
    {
        public static void UpdateVideoGame()
        {

        }

        public static void UpdatePlayedGame()
        {

        }

        public static void UpdateDroppedGame()
        {

        }

        public static void UpdateUnplayedGame()
        {

        }

        public static void UpdateUnpurchasedGame()
        {

        }

        public static void UpdateInfoFile<T>(List<T> infoData, string filepath)
        {
            using var writer = new StreamWriter(filepath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            //Use typeof(T) for switch case to determine what map to use
            switch (infoData)
            {
                case List<CurrentGame>:
                    csv.Context.RegisterClassMap<CurrentGameMap>();
                    break;
                case List<DroppedGame>:
                    csv.Context.RegisterClassMap<DroppedGameMap>();
                    break;
                case List<CompletedGame>:
                    csv.Context.RegisterClassMap<CompletedGameMap>();
                    break;
                // case List<PlayedGame>:
                //     csv.Context.RegisterClassMap<PlayedGameMap>();
                //     break;
                case List<UnpurchasedGame>:
                    csv.Context.RegisterClassMap<UnpurchasedGameMap>();
                    break;
                case List<BackloggedGame>:
                    csv.Context.RegisterClassMap<BackloggedGameMap>();
                    break;
                case List<Menu.Genre>:
                    csv.Context.RegisterClassMap<Menu.GenreMap>();
                    break;
                case List<Menu.Franchise>:
                    csv.Context.RegisterClassMap<Menu.FranchiseMap>();
                    break;
                // case List<VideoGame>:
                //     csv.Context.RegisterClassMap<VideoGameMap>();
                //     break;
            }
            csv.WriteRecords(infoData);
        }

        public static void CreateBackups()
        {
            Menu.settingsInfo newSettingsInfo = new Menu.settingsInfo();
            newSettingsInfo.nextGameID = Menu.mainFiles.nextGameID;
            newSettingsInfo.nextFranchiseID = Menu.mainFiles.nextFranchiseID;
            newSettingsInfo.nextGenreID = Menu.mainFiles.nextGenreID;
            newSettingsInfo.genreFile = $"saveFiles/gameGenres_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.franchiseFile = $"saveFiles/gameFranchises_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.currentGameFile = $"saveFiles/currentGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.completedGameFile = $"saveFiles/completedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.droppedGameFile = $"saveFiles/droppedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.unpurchasedGameFile = $"saveFiles/unpurchasedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.backloggedGameFile = $"saveFiles/backloggedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";


            UpdateInfoFile<Menu.Genre>(Menu.existingGenres, newSettingsInfo.genreFile);
            UpdateInfoFile<Menu.Franchise>(Menu.existingFranchises, newSettingsInfo.franchiseFile);
            UpdateInfoFile<CurrentGame>(Menu.existingCurrentGames, newSettingsInfo.currentGameFile);
            UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, newSettingsInfo.completedGameFile);
            UpdateInfoFile<DroppedGame>(Menu.existingDroppedGames, newSettingsInfo.droppedGameFile);
            UpdateInfoFile<UnpurchasedGame>(Menu.existingUnpurchasedGames, newSettingsInfo.unpurchasedGameFile);
            UpdateInfoFile<BackloggedGame>(Menu.existingBackloggedGames, newSettingsInfo.backloggedGameFile);

            Menu.backupFiles.Add(newSettingsInfo);
            Menu.mainFiles = newSettingsInfo;

            UpdateInfoFile<Menu.settingsInfo>(Menu.backupFiles, Menu.settingsPath);
        }

        public static void CreateRefresh()
        {
            Menu.settingsInfo newSettingsInfo = new Menu.settingsInfo();
            newSettingsInfo.nextGameID = 0;//Menu.mainFiles.nextGameID;
            newSettingsInfo.nextFranchiseID = 0;//Menu.mainFiles.nextFranchiseID;
            newSettingsInfo.nextGenreID = 0;//Menu.mainFiles.nextGenreID;
            newSettingsInfo.genreFile = $"saveFiles/gameGenres_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.franchiseFile = $"saveFiles/gameFranchises_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.currentGameFile = $"saveFiles/currentGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.completedGameFile = $"saveFiles/completedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.droppedGameFile = $"saveFiles/droppedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.unpurchasedGameFile = $"saveFiles/unpurchasedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.backloggedGameFile = $"saveFiles/backloggedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";


            UpdateInfoFile<Menu.Genre>(Menu.existingGenres, newSettingsInfo.genreFile);
            UpdateInfoFile<Menu.Franchise>(Menu.existingFranchises, newSettingsInfo.franchiseFile);
            UpdateInfoFile<CurrentGame>(Menu.existingCurrentGames, newSettingsInfo.currentGameFile);
            UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, newSettingsInfo.completedGameFile);
            UpdateInfoFile<DroppedGame>(Menu.existingDroppedGames, newSettingsInfo.droppedGameFile);
            UpdateInfoFile<UnpurchasedGame>(Menu.existingUnpurchasedGames, newSettingsInfo.unpurchasedGameFile);
            UpdateInfoFile<BackloggedGame>(Menu.existingBackloggedGames, newSettingsInfo.backloggedGameFile);

            Menu.backupFiles.Add(newSettingsInfo);
            Menu.mainFiles = newSettingsInfo;

            UpdateInfoFile<Menu.settingsInfo>(Menu.backupFiles, Menu.settingsPath);
        }

        
    }
}