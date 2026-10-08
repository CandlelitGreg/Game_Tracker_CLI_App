using System.Data.Common;
using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class CompletedGameMap : ClassMap<CompletedGame>
    {
        public CompletedGameMap()
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
            Map(m => m.firstPlayedDate);
            Map(m => m.completionDate);
            Map(m => m.review);
            Map(m => m.wantToPlayAgain);
            Map(m => m.replaying);
            Map(m => m.replayID).Convert(args => string.Join(";", args.Value.replayID));
        }
    }

    public sealed class RetrieveCompletedGameMap : ClassMap<CompletedGame>
    {
        public RetrieveCompletedGameMap()
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
            Map(m => m.firstPlayedDate);
            Map(m => m.completionDate);
            Map(m => m.review);
            Map(m => m.wantToPlayAgain);
            Map(m => m.replaying);
            Map(m => m.replayID).Convert(args => string.IsNullOrWhiteSpace(args.Row.GetField("replayID")) ? Array.Empty<int>() : args.Row.GetField("replayID")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
        }
    }


    public class CompletedGame : PlayedGame
    {
        public DateOnly firstPlayedDate {get; set;}
        public DateOnly completionDate {get; set;}
        public string review {get; set;}
        public bool wantToPlayAgain {get; set;}
        public bool replaying {get; set;}
        public int[] replayID {get; set;} = []; //Treat replay as a second gameID but hidden to user, only used to access replay specific information
        
        


        public void GetCompletedGameInfo()
        {
            //Get Completion Date
            completionDate = DateOnly.FromDateTime(Format.GetDateValue($"date you completed your playthrough of {gameName} in format dd/MM/yyyy"));

            //Get review
            review = Format.ReplaceCommasInString(WriteReview());
            //TODO: Change commas in review when saving to .csv

            //Get replaying status
            replaying = Format.GetClosedAnswer($"Are you currently replaying {gameName}? (y/n)");
            if (replaying)
            {
                //TODO: create currentGame instance for replay
                InitiateReplay();
            }

            //Get play again desire
            if (!replaying)
            {
                wantToPlayAgain = Format.GetClosedAnswer($"Do you want to replay {gameName} in the future? (y/n)");
            }

            //Save changes
            Menu.existingCompletedGames.Add(this);
            // Menu.existingVideoGames.Add(this);
            SaveGameInfo();
        }

        public void InitiateReplay()
        {
            //Create new currentGame object
            CurrentGame replayVersion = new CurrentGame();
            replayVersion.GrabPlayedGameInfo(this);
            replayVersion.gameID = Menu.GetAndUpdateNextGameID();
            replayVersion.originalPlaythroughID = gameID;
            replayVersion.replay = true;


            //Assign new GameID to replayId array
            replayID = replayID.Append(replayVersion.gameID).ToArray();

            //Save Changes
            Menu.existingCurrentGames.Add(replayVersion);
            CSVHandler.UpdateInfoFile<CurrentGame>(Menu.existingCurrentGames, Menu.mainFiles.currentGameFile);
        }

        public string WriteReview()
        {
            Console.WriteLine($"Please enter your review for {gameName}:");
            return Console.ReadLine();
            //TODO: build this out to be a multi-line reading func
        }

        public string ReadReview()
        {
            return Format.ReturnCommasToString(review);
        }

        public void EditPlayAgainStatus()
        {
            if (wantToPlayAgain && Format.GetClosedAnswer($"Are you sure you would like to remove your desire to replay {gameName} from your files? (y/n)"))
            {
                wantToPlayAgain = false;
            }
            else if (!wantToPlayAgain && Format.GetClosedAnswer($"Are you sure you would like to add your desire to replay {gameName} from your files? (y/n)"))
            {
                wantToPlayAgain = true;
            }
            SaveGameUpdates();
        }
        public void AlterStartDate()
        {
            if (firstPlayedDate != null)
            {
                Console.WriteLine($"The currently recorded start date for {gameName} is {firstPlayedDate.ToString("dd/MM/yyyy")}");
            }
            DateOnly newStart = DateOnly.FromDateTime(Format.GetDateValue($"new date you first played {gameName} in format dd/MM/yyyy"));
            if (firstPlayedDate != null && !Format.GetClosedAnswer($"Are you sure you want to change the first date you played {gameName} to be {newStart.ToString("dd/MM/yyyy")} instead of {firstPlayedDate}? (y/n)"))
            {
                return;
            }
            firstPlayedDate = newStart;
        }
        public void AlterCompletionDate()
        {
            Console.WriteLine($"{gameName} is currently recorded to have been completed on {completionDate.ToString("dd/MM/yyyy")}");
            DateOnly newCompletion = DateOnly.FromDateTime(Format.GetDateValue($"new date you finished playing {gameName} in format dd/MM/yyyy"));
            if (!Format.GetClosedAnswer($"Are you sure you want to change the completion date for {gameName} to be {newCompletion.ToString("dd/MM/yyyy")} instead of {completionDate}? (y/n)"))
            {
                return;
            }
            completionDate = newCompletion;
        }
        public void EditCompletionDates()
        {
            Console.WriteLine($"What date would you like to change regarding {gameName}'s completion?\n");
            Console.WriteLine("1. Edit first played date");
            Console.WriteLine("2. Edit date completed");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What date would you like to edit?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    AlterStartDate();
                    break;
                case 2:
                    AlterCompletionDate();
                    break;
            }
            SaveGameUpdates();
        }

        public void AlterReview()
        {
            Console.WriteLine($"Your current review for {gameName} is as follows:\n -----   -----   -----\n{review}\n -----   -----   -----\nWhat would you like to have as your new review?");
            string newReview = Format.CheckForCommas(Console.ReadLine(), $"new review for {gameName}");
            if (Format.GetClosedAnswer($"Are you sure you want to remove your old review for {gameName} and replace it with:\n -----   -----   -----\n{review}\n -----    -----   -----\nWe suggest selecting NO if you are not sure as your previous review will be lost unless you have a backup of your files. (y/n)"))
            {
                review = newReview;
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
            Console.WriteLine("8. Edit completion dates");
            if (wantToPlayAgain)
            {
                Console.WriteLine("9. Set 'want to replay' to false");
            }
            else
            {
                Console.WriteLine("9. Set 'want to replay' to true");
            }
            Console.WriteLine("10. Edit review");
            // Console.WriteLine("11. Edit replay information");
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
                    EditCompletionDates();
                    break;
                case 9:
                    EditPlayAgainStatus();
                    break;
                case 10:
                    AlterReview();
                    break;
                default:
                    Console.WriteLine("This functionality is not yet implemented");
                    break;
            }
        }

        public string DisplayCompletedGameInfo()
        {
            string completedGameInfo = DisplayPlayedInfo();
            completedGameInfo += 
                                 $"First Played Date: {firstPlayedDate.ToString("dd/MM/yyyy")}\n" +
                                 $"Completion Date:   {completionDate.ToString("dd/MM/yyyy")}\n" +
                                 $"\n vvv---Review---vvv \n{ReadReview()}\n ^^^---Review---^^^\n\n";
            if (replaying)
            {
                completedGameInfo += $"Currently Replaying: Yes\n";
            } else if (wantToPlayAgain)
            {
                completedGameInfo += $"Want to Play Again: {(wantToPlayAgain ? "Yes" : "No")}\n";
            }
            return completedGameInfo;
        }

        public void SaveGameInfo()
        {
            CSVHandler.UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, Menu.mainFiles.completedGameFile);
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