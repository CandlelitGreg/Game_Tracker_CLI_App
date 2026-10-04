using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class BackloggedGameMap : ClassMap<BackloggedGame>
    {
        public BackloggedGameMap()
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
            Map(m => m.purchaseDate);
        }
    }

    public sealed class RetrieveBackloggedGameMap : ClassMap<BackloggedGame>
    {
        public RetrieveBackloggedGameMap()
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
            Map(m => m.purchaseDate);
        }
    }

    public class BackloggedGame : UnplayedGame
    {
        public DateOnly purchaseDate {get; set;}


        public void GetBackloggedGameInfo()
        {
            //Get purchaseDate
            purchaseDate = DateOnly.FromDateTime(Format.GetDateValue($"date you purchased {gameName} in format dd/MM/yyyy"));

            //Save changes
            Menu.existingBackloggedGames.Add(this);
            Menu.existingVideoGames.Add(this);
            SaveGameInfo();
        }

        public void SaveGameInfo()
        {
            CSVHandler.UpdateInfoFile<BackloggedGame>(Menu.existingBackloggedGames, Menu.mainFiles.backloggedGameFile);
        }
    }
}