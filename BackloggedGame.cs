namespace GameTrackerEx01
{
    public class BackloggedGame : UnplayedGame
    {
        public DateOnly purchaseDate {get; set;}


        public void GetBackloggedGameInfo()
        {
            //Get purchaseDate
            purchaseDate = DateOnly.FromDateTime(Format.GetDateValue($"date you purchased {gameName} in format dd/MM/yyyy"));

            //Save changes
            Menu.existingBackloggedGames.Add(this);
            CSVHandler.UpdateInfoFile<BackloggedGame>(Menu.existingBackloggedGames, Menu.mainFiles.backloggedGameFile);
        }
    }
}