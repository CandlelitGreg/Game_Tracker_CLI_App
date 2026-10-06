using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class CurrentGameMap : ClassMap<CurrentGame>
    {
        public CurrentGameMap()
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
            Map(m => m.lastPlayed);
            Map(m => m.firstPlayedDate);
            Map(m => m.replay);
            Map(m => m.originalPlaythroughID);
            Map(m => m.previouslyDropped);
        }
    }

    public sealed class RetrieveCurrentGameMap : ClassMap<CurrentGame>
    {
        public RetrieveCurrentGameMap()
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
            Map(m => m.lastPlayed);
            Map(m => m.firstPlayedDate);
            Map(m => m.replay);
            Map(m => m.originalPlaythroughID);
            Map(m => m.previouslyDropped);
        }
    }

    public class CurrentGame : PlayedGame
    {
        public DateOnly lastPlayed {get; set;}
        public DateOnly firstPlayedDate {get; set;}
        public bool replay {get; set;}
        public int originalPlaythroughID {get; set;}
        public bool previouslyDropped {get; set;}

        public void GetCurrentGameInfo()
        {
            //Get last played date
            lastPlayed = DateOnly.FromDateTime(Format.GetDateValue($"date you last played {gameName} in format dd/MM/yyyy"));

            //Get replay status
            replay = !Format.GetClosedAnswer($"Is this your first time playing through {gameName}? (y/n)");
            //CREATE A COMPLETED VERSION OF THE GAME
            if (replay)
            {
                CompletedGame originalVersion = new CompletedGame();
                originalVersion.GrabPlayedGameInfo(this);
                originalVersion.gameID = Menu.GetAndUpdateNextGameID();
                originalPlaythroughID = originalVersion.gameID;
                originalVersion.replaying = true;
                originalVersion.replayID = originalVersion.replayID.Append(gameID).ToArray();
                originalVersion.wantToPlayAgain = true;
                if (Format.GetClosedAnswer($"Do you remember the date you originally completed {gameName}? (y/n)"))
                {
                    originalVersion.completionDate = DateOnly.FromDateTime(Format.GetDateValue($"date you originally completed {gameName} in format dd/MM/yyyy"));
                }
                originalVersion.review = originalVersion.WriteReview();

                //Save changes
                Menu.existingCompletedGames.Add(originalVersion);
                CSVHandler.UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, Menu.mainFiles.completedGameFile);
            }


            //Get mainPlay device
            mainPlayDevice = Format.AskForInput($" device you are primarily using to play {gameName}");

            //Get modded status
            modded = Format.GetClosedAnswer($"Is this playthrough of {gameName} modded? (y/n)");

            //Save changes
            Menu.existingCurrentGames.Add(this);
            // Menu.existingVideoGames.Add(this);
            SaveGameInfo();

        }

        public void CreateLogEntry()
        {
            //Add log message

            //Update playtime

            //Update last played date
        }

        public void CompleteGame()
        {
            //Create CompletedGame object

            //Transfer currentGame details

            //Set completion date

            //Get Completed game info

            //remove this object
        }

        public void EditChildDetails()
        {
            
        }

        public string DisplayCurrentGameInfo()
        {
            string currentGameInfo = DisplayPlayedInfo();
            currentGameInfo += 
                               $"First Played: {firstPlayedDate.ToString("dd/MM/yyyy")}\n" +
                               $"Last Played: {lastPlayed.ToString("dd/MM/yyyy")}\n" +
                               $"Previously Dropped: {(previouslyDropped ? "Yes" : "No")}\n";
            return currentGameInfo;
        }

        public void SaveGameInfo()
        {
            CSVHandler.UpdateInfoFile<CurrentGame>(Menu.existingCurrentGames, Menu.mainFiles.currentGameFile);
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