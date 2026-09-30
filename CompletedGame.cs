namespace GameTrackerEx01
{
    public class CompletedGame : PlayedGame
    {
        public DateOnly firstPlayedDate {get; set;}
        public DateOnly completionDate {get; set;}
        public string review {get; set;}
        public bool wantToPlayAgain {get; set;}
        public bool replaying {get; set;}
        public int[] replayID {get; set;} //Treat replay as a second gameID but hidden to user, only used to access replay specific information
        
        //Bring from current game object
        public string[] logMessages {get; set;}
        public string mainPlayDevice {get; set;}
        public bool modded {get; set;}


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
            CSVHandler.UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, Menu.mainFiles.completedGameFile);
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
    }
}