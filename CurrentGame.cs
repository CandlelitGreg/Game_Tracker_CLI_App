namespace GameTrackerEx01
{
    public class CurrentGame : PlayedGame
    {
        public DateOnly lastPlayed {get; set;}
        public DateOnly firstPlayedDate {get; set;}
        public bool replay {get; set;}
        public int originalPlaythroughID {get; set;}
        public string mainPlayDevice {get; set;}
        public bool modded {get; set;}
        public string[] logMessages {get; set;}
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
            CSVHandler.UpdateInfoFile<CurrentGame>(Menu.existingCurrentGames, Menu.mainFiles.currentGameFile);

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
    }
}