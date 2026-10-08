using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class DroppedGameMap : ClassMap<DroppedGame>
    {
        public DroppedGameMap()
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
            Map(m => m.hoursPlayed);
            Map(m => m.rating);
            Map(m => m.initialExcitementLevel);
            Map(m => m.initialImpressions);
            Map(m => m.nextEntryID);
            Map(m => m.logMessages).Convert(args => string.Join("/?/>>^&*!/?/", args.Value.logMessages));
            Map(m => m.mainPlayDevice);
            Map(m => m.modded);
            Map(m => m.dateDropped);
            Map(m => m.reasonDropped);
            Map(m => m.wouldRetry);
        }
    }

    public sealed class RetrieveDroppedGameMap : ClassMap<DroppedGame>
    {
        public RetrieveDroppedGameMap()
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
            Map(m => m.hoursPlayed);
            Map(m => m.rating);
            Map(m => m.initialExcitementLevel);
            Map(m => m.initialImpressions);
            Map(m => m.nextEntryID);
            Map(m => m.logMessages).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("logMessages")) ? Array.Empty<string>() : args.Row.GetField("logMessages")?.Split("/?/>>^&*!/?/").ToArray() ?? Array.Empty<string>());
            Map(m => m.mainPlayDevice);
            Map(m => m.modded);
            Map(m => m.dateDropped);
            Map(m => m.reasonDropped);
            Map(m => m.wouldRetry);
        }
    }


    public class DroppedGame : PlayedGame
    {
        public DateOnly dateDropped {get; set;}
        public string reasonDropped {get; set;}
        public bool wouldRetry {get; set;}
        

        

        public void GetDroppedGameInfo()
        {
            //Get date dropped
            dateDropped = DateOnly.FromDateTime(Format.GetDateValue($"date you last played {gameName} in format dd/MM/yyyy"));
            
            //Get reason dropped
            reasonDropped = Format.ReplaceCommasInString(WriteWhyDropped());

            //Get wouldRetry bool
            wouldRetry = Format.GetClosedAnswer($"Would you conisder retrying {gameName}? (y/n)");

            //Save changes
            Menu.existingDroppedGames.Add(this);
            // Menu.existingVideoGames.Add(this);
            SaveGameInfo();
        }

        public string WriteWhyDropped()
        {
            Console.WriteLine($"Please enter the reason's you stopped playing {gameName}:");
            return Console.ReadLine();
            //TODO: build this out to be a multi-line reading func
        }

        public void updateRetryStatus()
        {

        }

        public string ReadWhyDropped()
        {
            return Format.ReturnCommasToString(reasonDropped);
        }

        public bool ReadRetry()
        {
            return wouldRetry;
        }

        public void LogRetry()
        {
            //Create current game object


            //Add dropped details as initial log entry in current game object


            //Get current game details


            //remove this object
            
        }
        public void AlterDropDate()
        {
            Console.WriteLine($"{gameName} is currently recorded to have been last played on {dateDropped.ToString("dd/MM/yyyy")}");
            DateOnly newDate = DateOnly.FromDateTime(Format.GetDateValue($"new date you finished playing {gameName} in format dd/MM/yyyy"));
            if (!Format.GetClosedAnswer($"Are you sure you want to change the date {gameName} was last played to be {newDate.ToString("dd/MM/yyyy")} instead of {dateDropped}? (y/n)"))
            {
                return;
            }
            dateDropped = newDate;
        }

        public void AlterDropReason()
        {
            Console.WriteLine($"Your current reason for having stopped playing {gameName} is as follows:\n -----   -----   -----\n{reasonDropped}\n -----   -----   -----\nWhat would you like to have as your new review?");
            string newReview = Format.CheckForCommas(Console.ReadLine(), $"new review for {gameName}");
            if (Format.GetClosedAnswer($"Are you sure you want to remove your old reasoning for having stopped playing {gameName} and replace it with:\n -----   -----   -----\n{reasonDropped}\n -----    -----   -----\nWe suggest selecting NO if you are not sure as your previous entry will be lost unless you have a backup of your files. (y/n)"))
            {
                reasonDropped = newReview;
            }
            SaveGameUpdates();
        }
        public void EditPlayAgainStatus()
        {
            if (wouldRetry && Format.GetClosedAnswer($"Are you sure you would like to remove your desire to retry/continue {gameName} from your files? (y/n)"))
            {
                wouldRetry = false;
            }
            else if (!wouldRetry && Format.GetClosedAnswer($"Are you sure you would like to add your desire to retry/continue {gameName} from your files? (y/n)"))
            {
                wouldRetry = true;
            }
            SaveGameUpdates();
        }

        public void EditChildDetails()
        {
            Console.WriteLine($"What further details would you like to edit about {gameName}?\n");
            Console.WriteLine("1. Edit hours played");
            Console.WriteLine("2. Edit rating");
            Console.WriteLine("3. Edit initial excitement level");
            Console.WriteLine("4. Edit sequel status");
            Console.WriteLine("5. Edit log messages");
            Console.WriteLine("6. Edit main play device");
            Console.WriteLine("7. Edit modded playthrough status");
            Console.WriteLine("8. Edit the date you stopped playing");
            if (wouldRetry)
            {
                Console.WriteLine("9. Set 'want to retry/continue' to false");
            }
            else
            {
                Console.WriteLine("9. Set 'want to retry/continue' to true");
            }
            Console.WriteLine($"10. Edit the reason you stopped playing {gameName}");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(10, $"What would you like to edit about {gameName}?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    EditHoursPlayed();
                    break;
                case 2:
                    EditRating();
                    break;
                case 3:
                    EditInitialExcitement();
                    break;
                case 4:
                    EditSequelStatus();
                    break;
                case 5:
                    //vvvv TODO: Change log storage to log IDs - make log class object - make logs get saved in seperate file that gets called from by id vvvv
                    EditLogs();
                    break;
                case 6:
                    ChangePlayDevice();
                    break;
                case 7:
                    ChangeModdedStatus();
                    break;
                case 8:
                    AlterDropDate();
                    break;
                case 9:
                    EditPlayAgainStatus();
                    break;
                case 10:
                    AlterDropReason();
                    break;
                default:
                    Console.WriteLine("This functionality is not yet implemented");
                    break;
            }
        }

        public string DisplayDroppedGameInfo()
        {
            string droppedGameInfo = DisplayPlayedInfo();
            droppedGameInfo += 
                                $"Date Dropped: {dateDropped.ToString("dd/MM/yyyy")}\n" +
                                $"Reason Dropped: {ReadWhyDropped()}\n" +
                                $"Would Retry: {(wouldRetry ? "Yes" : "No")}\n";
            return droppedGameInfo;
        }

        public void SaveGameInfo()
        {
            CSVHandler.UpdateInfoFile<DroppedGame>(Menu.existingDroppedGames, Menu.mainFiles.droppedGameFile);
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