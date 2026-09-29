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

        public static void UpdateGenreFile(List<Menu.Genre> genres, string filepath)
        {
            using var writer = new StreamWriter(filepath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(genres);
        }
        //ABOVE AND BELOW FUNCS CAN BE COMBINED INTO A TYPE T FUNC - Could then also use same func for all game files
        public static void UpdateFranchiseFile(List<Menu.Franchise> franchises, string filepath)
        {
            using var writer = new StreamWriter(filepath);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
            csv.WriteRecords(franchises);
        }

        public static void CreateBackups()
        {
            UpdateGenreFile(Menu.existingGenres, $"gameGenres_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
            UpdateFranchiseFile(Menu.existingFranchises, $"gameFranchises_{DateTime.Now.ToString("yyyyMMddHHmmss")}.csv");
        }

        
    }
}