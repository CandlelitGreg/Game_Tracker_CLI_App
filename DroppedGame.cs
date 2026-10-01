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
            Map(m => m.DLCIDs).Convert(args => string.Join(";", args.Value.DLCIDs));
            Map(m => m.hoursPlayed);
            Map(m => m.rating);
            Map(m => m.initialExcitementLevel);
            Map(m => m.initialImpressions);
            Map(m => m.nextEntryID);
            Map(m => m.completed);
            Map(m => m.playing);
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
            Map(m => m.gameGenreIDs).Convert(args => args.Row.GetField("gameGenreIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.deckPlayable);
            Map(m => m.sessionGame);
            Map(m => m.worthCompletionist);
            Map(m => m.franchiseID);
            Map(m => m.played);
            Map(m => m.DLCIDs).Convert(args => args.Row.GetField("DLCIDs")?.Split(";").Select(int.Parse).ToArray() ?? Array.Empty<int>());
            Map(m => m.hoursPlayed);
            Map(m => m.rating);
            Map(m => m.initialExcitementLevel);
            Map(m => m.initialImpressions);
            Map(m => m.nextEntryID);
            Map(m => m.completed);
            Map(m => m.playing);
            Map(m => m.logMessages).Convert(args => args.Row.GetField("logMessages")?.Split("/?/>>^&*!/?/").ToArray() ?? Array.Empty<string>());
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