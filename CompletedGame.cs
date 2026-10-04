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
            Menu.existingVideoGames.Add(this);
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

        public void SaveGameInfo()
        {
            CSVHandler.UpdateInfoFile<CompletedGame>(Menu.existingCompletedGames, Menu.mainFiles.completedGameFile);
        }
    }
}