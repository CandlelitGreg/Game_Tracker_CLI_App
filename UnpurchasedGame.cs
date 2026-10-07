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
            // Menu.existingVideoGames.Add(this);
            SaveGameInfo();
        }

        public void EditFullPrice()
        {
            Console.WriteLine($"The current full price for {gameName} on file is ${fullPrice}\nWhat is the new full price cost?");
            float newPrice = Format.ConvertStringToCost(Console.ReadLine(), $"What is the new full price cost for {gameName}?");
            if (Format.GetClosedAnswer($"Are you sure you want to update the full price for {gameName} from ${fullPrice} to ${newPrice}? (y/n)"))
            {
                fullPrice = newPrice;
            }
        }

        public void EditSalePrice()
        {
            Console.WriteLine($"The current discounted price for {gameName} on file is ${lowestSalePrice}\nWhat is the new lowest discounted cost?");
            float newPrice = Format.ConvertStringToCost(Console.ReadLine(), $"What is the new lowest discounted cost for {gameName}?");
            if (Format.GetClosedAnswer($"Are you sure you want to update the discounted price for {gameName} from ${lowestSalePrice} to ${newPrice}? (y/n)"))
            {
                lowestSalePrice = newPrice;
            }
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

        public void CheckIfReleased()
        {
            if (releaseDate >= DateOnly.FromDateTime(DateTime.Now))
            {
                released = true;
            }
        }

        public void EditPricing()
        {
            if (released)
            {
                Console.WriteLine($"What price to you want to change?\n");
                Console.WriteLine("1. Edit full price");
                Console.WriteLine("2. Edit lowest discounted price");
                Console.WriteLine("0. Go back");
                int userInput = Format.GetSingleResponse(2, $"What price do you want to edit?");
                switch (userInput)
                {
                    case 0:
                        return;
                    case 1:
                        EditFullPrice();
                        break;
                    case 2:
                        EditSalePrice();
                        break;
                }
                SaveGameUpdates();
            } 
            else
            {
                EditFullPrice();
            }
            
        }

        public void EditReleaseDate()
        {
            if (released && !Format.GetClosedAnswer($"According to your records, {gameName} has already released. Are you sure you want to revert this? (y/n)"))
            {
                return;
            } else
            {
                if (!released)
                {
                    Console.WriteLine($"Your records have {gameName} releasing on {releaseDate.ToString("dd/MM/yyyy")}");
                }
                DateOnly newReleaseDate = DateOnly.FromDateTime(Format.GetDateValue($"new release date for {gameName} in format dd/MM/yyyy\nIf game does not have exact date please input the final date within its release window"));
                if (Format.GetClosedAnswer($"Are you sure you want to update the release date for {gameName} from {releaseDate.ToString("dd/MM/yyyy")} to {newReleaseDate.ToString("dd/MM/yyyy")}? (y/n)"))
                {
                    releaseDate = newReleaseDate;
                }
                CheckIfReleased();
            }
            SaveGameUpdates();

        }

        public void EditChildDetails()
        {
            Console.WriteLine($"What further details would you like to edit about {gameName}?\n");
            Console.WriteLine("1. Edit excitement status");
            Console.WriteLine("2. Edit sequel status");
            Console.WriteLine("3. Edit pricing");
            Console.WriteLine("4. Edit release date");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(4, $"What would you like to edit about {gameName}?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    EditExcitementLevel();
                    break;
                case 2:
                    EditSequelStatus();
                    break;
                case 3:
                    EditPricing();
                    break;
                case 4:
                    EditReleaseDate();
                    break;
            }
        }

        public string DisplayUnpurchasedGameInfo()
        {
            string unpurchasedGameInfo = DisplayUnplayedGameInfo() +
                                       $"Full Price:        {fullPrice}\n" +
                                       $"Lowest Sale Price: {lowestSalePrice}\n" +
                                       $"Released:          {(released ? "Yes" : "No")}\n";
            if (!released)
            {
                unpurchasedGameInfo += $"Release Date:      {(!released ? releaseDate.ToString("dd/MM/yyyy") : "N/A")}\n";
            }
            return unpurchasedGameInfo;
        }

        public void SaveGameInfo()
        {
            CSVHandler.UpdateInfoFile<UnpurchasedGame>(Menu.existingUnpurchasedGames, Menu.mainFiles.unpurchasedGameFile);
            if (Menu.FindGameByID(gameID) != null)
            {
                Menu.UpdateGameByID(this);
            } else
            {
                Menu.existingVideoGames.Add(this);
            }
        }
    }
}