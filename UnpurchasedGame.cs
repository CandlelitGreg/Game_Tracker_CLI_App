namespace GameTrackerEx01
{
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
            CSVHandler.UpdateInfoFile<UnpurchasedGame>(Menu.existingUnpurchasedGames, Menu.mainFiles.unpurchasedGameFile);
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
    }
}