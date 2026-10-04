using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class UnpurchasedGameMap : ClassMap<UnpurchasedGame>
    {
        public UnpurchasedGameMap()
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
            Map(m => m.excitementLevel);
            Map(m => m.sequel);
            Map(m => m.previousEntryID);
            Map(m => m.fullPrice);
            Map(m => m.lowestSalePrice);
            Map(m => m.released);
            Map(m => m.releaseDate);
        }
    }

    public sealed class RetrieveUnpurchasedGameMap : ClassMap<UnpurchasedGame>
    {
        public RetrieveUnpurchasedGameMap()
        {
            Map(m => m.gameID);
            Map(m => m.gameName);
            Map(m => m.avgGameLength);
            Map(m => m.gameGenreIDs).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("gameGenreIDs")) ? Array.Empty<int>() : args.Row.GetField("gameGenreIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.deckPlayable);
            Map(m => m.sessionGame);
            Map(m => m.worthCompletionist);
            Map(m => m.franchiseID);
            Map(m => m.played);
            Map(m => m.purchased);
            Map(m => m.completed);
            Map(m => m.playing);
            Map(m => m.DLCIDs).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("DLCIDs")) ? Array.Empty<int>() : args.Row.GetField("DLCIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.excitementLevel);
            Map(m => m.sequel);
            Map(m => m.previousEntryID);
            Map(m => m.fullPrice);
            Map(m => m.lowestSalePrice);
            Map(m => m.released);
            Map(m => m.releaseDate);
        }
    }

    public class UnpurchasedGame : UnplayedGame
    {
        public float fullPrice {get; set;}
        public float lowestSalePrice {get; set;}
        public bool released {get; set;}
        public DateOnly releaseDate {get; set;}

        public void GetUnpurchasedGameInfo()
        {
            //Get fullprice
            Console.WriteLine($"What is the full price cost of {gameName}?");
            fullPrice = Format.ConvertStringToCost(Console.ReadLine(), $"What is the full price cost of {gameName}?");

            //Get release status
            released = Format.GetClosedAnswer($"Has {gameName} already released? (y/n)");

            
            if (released)
            {
                //Get sale price
                Console.WriteLine($"What is the lowest sale price cost for {gameName}?");
                lowestSalePrice = Format.ConvertStringToCost(Console.ReadLine(), $"What is the lowest sale price cost for {gameName}?");
            } else {
                //Get release date
                releaseDate = DateOnly.FromDateTime(Format.GetDateValue($"current release date for {gameName} in format dd/MM/yyyy\nIf game does not have exact date please input the final date within its release window"));
            }

            //Save changes
            Menu.existingUnpurchasedGames.Add(this);
            Menu.existingVideoGames.Add(this);
            SaveGameInfo();
        }

        public void EditFullPrice()
        {

        }

        public void EditSalePrice()
        {

        }

        public (float fullPrice, float salePrice) ReadPricing()
        {
            return (fullPrice, lowestSalePrice);
        }

        public void PurchaseGame()
        {
            //Create BackloggedGame object

            //Transfer UnplayedData

            //Get Backlogged Data

            //Remove this object
        }

        public void SaveGameInfo()
        {
            CSVHandler.UpdateInfoFile<UnpurchasedGame>(Menu.existingUnpurchasedGames, Menu.mainFiles.unpurchasedGameFile);
        }
    }
}