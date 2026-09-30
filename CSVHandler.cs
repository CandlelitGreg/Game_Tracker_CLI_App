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

        // public static void UpdateGenreFile(List<Menu.Genre> genres, string filepath)
        // {
        //     using var writer = new StreamWriter(filepath);
        //     using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        //     csv.WriteRecords(genres);
        // }
        // //ABOVE AND BELOW FUNCS CAN BE COMBINED INTO A TYPE T FUNC - Could then also use same func for all game files
        // public static void UpdateFranchiseFile(List<Menu.Franchise> franchises, string filepath)
        // {
        //     using var writer = new StreamWriter(filepath);
        //     using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        //     csv.WriteRecords(franchises);
        // }

        public static void UpdateInfoFile<T>(List<T> infoData, string filepath)
        {
            using var writer = new StreamWriter(filepath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(infoData);
        }

        public static void CreateBackups()
        {
            Menu.settingsInfo newSettingsInfo = new Menu.settingsInfo();
            // newSettingsInfo.nextGameID = Menu.mainFiles.nextGameID;
            // newSettingsInfo.nextFranchiseID = Menu.mainFiles.nextFranchiseID;
            newSettingsInfo.nextGameID = 0;
            newSettingsInfo.nextFranchiseID = 0;
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