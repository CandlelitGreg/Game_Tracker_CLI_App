namespace GameTrackerEx01
{
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
            reasonDropped = Format.AskForInput($"reason you stopped playing {gameName}");

            //Get wouldRetry bool
            wouldRetry = Format.GetClosedAnswer($"Would you conisder retrying {gameName}? (y/n)");

            //Save changes
            Menu.existingDroppedGames.Add(this);
            CSVHandler.UpdateInfoFile<DroppedGame>(Menu.existingDroppedGames, Menu.mainFiles.droppedGameFile);
        }

        public void AddWhyDropped()
        {

        }

        public void updateRetryStatus()
        {

        }

        public string ReadWhyDropped()
        {
            return reasonDropped;
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
    }
}