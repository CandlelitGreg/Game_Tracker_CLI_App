using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace GameTrackerEx01
{
    public static class CSVHandler
    {
        public static void UpdateAllFiles()
        {
            // Menu.FindSettingsFile(Menu.backupFiles[0].activeSettingsID).dateLastUsed = DateOnly.FromDateTime(DateTime.Now);
            Menu.mainFiles.UpdateLastUsed();
            UpdateInfoFile<Genre>(Menu.existingGenres, Menu.mainFiles.genreFile);
            UpdateInfoFile<Franchise>(Menu.existingFranchises, Menu.mainFiles.franchiseFile);
            UpdateInfoFile<LogMessage>(Menu.existingLogMessages, Menu.mainFiles.logEntryFile);
            UpdateInfoFile<CurrentGame>(Menu.existingCurrentGames, Menu.mainFiles.currentGameFile);
            UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, Menu.mainFiles.completedGameFile);
            UpdateInfoFile<DroppedGame>(Menu.existingDroppedGames, Menu.mainFiles.droppedGameFile);
            UpdateInfoFile<UnpurchasedGame>(Menu.existingUnpurchasedGames, Menu.mainFiles.unpurchasedGameFile);
            UpdateInfoFile<BackloggedGame>(Menu.existingBackloggedGames, Menu.mainFiles.backloggedGameFile);
            UpdateInfoFile<SettingsInfo>(Menu.backupFiles, Menu.settingsPath);
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
                case List<UnpurchasedGame>:
                    csv.Context.RegisterClassMap<UnpurchasedGameMap>();
                    break;
                case List<BackloggedGame>:
                    csv.Context.RegisterClassMap<BackloggedGameMap>();
                    break;
                case List<Genre>:
                    csv.Context.RegisterClassMap<GenreMap>();
                    break;
                case List<Franchise>:
                    csv.Context.RegisterClassMap<FranchiseMap>();
                    break;
                case List<SettingsInfo>:
                    csv.Context.RegisterClassMap<SettingsMap>();
                    break;
                case List<LogMessage>:
                    csv.Context.RegisterClassMap<LogMessageMap>();
                    break;
            }
            csv.WriteRecords(infoData);
        }

        public static SettingsInfo CreateBackup()
        {
            SettingsInfo newSettingsInfo = new SettingsInfo();
            newSettingsInfo.settingsID = Menu.GetAndUpdateSettingsID();
            newSettingsInfo.nextSettingsID = Menu.mainFiles.nextSettingsID;
            newSettingsInfo.nextGameID = Menu.mainFiles.nextGameID;
            newSettingsInfo.nextFranchiseID = Menu.mainFiles.nextFranchiseID;
            newSettingsInfo.nextGenreID = Menu.mainFiles.nextGenreID;
            newSettingsInfo.nextLogID = Menu.mainFiles.nextLogID;
            newSettingsInfo.logEntryFile = $"saveFiles/logMessages_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.genreFile = $"saveFiles/gameGenres_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.franchiseFile = $"saveFiles/gameFranchises_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.currentGameFile = $"saveFiles/currentGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.completedGameFile = $"saveFiles/completedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.droppedGameFile = $"saveFiles/droppedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.unpurchasedGameFile = $"saveFiles/unpurchasedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.backloggedGameFile = $"saveFiles/backloggedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";


            UpdateInfoFile<LogMessage>(Menu.existingLogMessages, newSettingsInfo.logEntryFile);
            UpdateInfoFile<Genre>(Menu.existingGenres, newSettingsInfo.genreFile);
            UpdateInfoFile<Franchise>(Menu.existingFranchises, newSettingsInfo.franchiseFile);
            UpdateInfoFile<CurrentGame>(Menu.existingCurrentGames, newSettingsInfo.currentGameFile);
            UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, newSettingsInfo.completedGameFile);
            UpdateInfoFile<DroppedGame>(Menu.existingDroppedGames, newSettingsInfo.droppedGameFile);
            UpdateInfoFile<UnpurchasedGame>(Menu.existingUnpurchasedGames, newSettingsInfo.unpurchasedGameFile);
            UpdateInfoFile<BackloggedGame>(Menu.existingBackloggedGames, newSettingsInfo.backloggedGameFile);

            Menu.backupFiles.Add(newSettingsInfo);

            UpdateInfoFile<SettingsInfo>(Menu.backupFiles, Menu.settingsPath);

            return newSettingsInfo;
        }

        public static int CreateRefresh()
        {
            SettingsInfo newSettingsInfo = new SettingsInfo();
            newSettingsInfo.settingsID = Menu.GetAndUpdateSettingsID();
            newSettingsInfo.nextSettingsID = Menu.mainFiles.nextSettingsID;
            newSettingsInfo.nextGameID = 0;
            newSettingsInfo.nextFranchiseID = 0;
            newSettingsInfo.nextGenreID = 0;
            newSettingsInfo.nextLogID = 0;
            newSettingsInfo.logEntryFile = $"saveFiles/logMessages_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.genreFile = $"saveFiles/gameGenres_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.franchiseFile = $"saveFiles/gameFranchises_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.currentGameFile = $"saveFiles/currentGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.completedGameFile = $"saveFiles/completedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.droppedGameFile = $"saveFiles/droppedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.unpurchasedGameFile = $"saveFiles/unpurchasedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";
            newSettingsInfo.backloggedGameFile = $"saveFiles/backloggedGames_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv";

            Menu.backupFiles.Add(newSettingsInfo);

            UpdateInfoFile<LogMessage>(new List<LogMessage>(), newSettingsInfo.logEntryFile);
            UpdateInfoFile<Genre>(new List<Genre>(), newSettingsInfo.genreFile);
            UpdateInfoFile<Franchise>(new List<Franchise>(), newSettingsInfo.franchiseFile);
            UpdateInfoFile<CurrentGame>(new List<CurrentGame>(), newSettingsInfo.currentGameFile);
            UpdateInfoFile<CompletedGame>(new List<CompletedGame>(), newSettingsInfo.completedGameFile);
            UpdateInfoFile<DroppedGame>(new List<DroppedGame>(), newSettingsInfo.droppedGameFile);
            UpdateInfoFile<UnpurchasedGame>(new List<UnpurchasedGame>(), newSettingsInfo.unpurchasedGameFile);
            UpdateInfoFile<BackloggedGame>(new List<BackloggedGame>(), newSettingsInfo.backloggedGameFile);

            Menu.SetCurrentSettings(newSettingsInfo.settingsID);

            return newSettingsInfo.settingsID;
        }

        
    }
}