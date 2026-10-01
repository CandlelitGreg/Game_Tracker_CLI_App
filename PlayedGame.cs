using CsvHelper.Configuration;

namespace GameTrackerEx01
{

    public sealed class PlayedGameMap : ClassMap<PlayedGame>
    {
        public PlayedGameMap()
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
        }
    }


    public class PlayedGame : VideoGame
    {
        public float hoursPlayed {get; set;}
        public int rating {get; set;}
        public int initialExcitementLevel {get; set;}
        public string initialImpressions {get; set;} //Contains comma replaced format
        public int nextEntryID {get; set;} = -1;
        public bool completed {get; set;}
        public bool playing {get; set;}

        public string[] logMessages {get; set;}
        public string mainPlayDevice {get; set;}
        public bool modded {get; set;}

        public void GetPlayInfo()
        {
            //Get current playtime
            Console.WriteLine($"Please input the approximate number of hours you have spent with {gameName}");
            hoursPlayed = Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the approximate play time you have spent with {gameName}");

            //Get current rating
            Console.WriteLine($"What is your current rating of {gameName} from 1-10? (1: Horrible, 10: Amazing)");
            rating = Format.ConvertStringToInt(Console.ReadLine(), $"What is your current rating of {gameName} from 1-10? (1: Horrible, 10: Amazing)");
            while (rating > 10 || rating < 1)
            {
                Console.WriteLine($"{rating} is outside of the 1-10 range. Please select a rating within 1 and 10");
                rating = Format.ConvertStringToInt(Console.ReadLine(), $"What is your current rating of {gameName} from 1-10? (1: Horrible, 10: Amazing)");
            }

            //Get initial excitement level
            Console.WriteLine($"What was your initial excitement level from 1-10 prior to playing {gameName}? (1: Horrible, 10: Amazing)");
            initialExcitementLevel = Format.ConvertStringToInt(Console.ReadLine(), $"What was your initial excitement level from 1-10 prior to playing {gameName}? (1: Horrible, 10: Amazing)");
            while (initialExcitementLevel > 10 || initialExcitementLevel < 1)
            {
                Console.WriteLine($"{initialExcitementLevel} is outside of the 1-10 range. Please select a rating within 1 and 10");
                initialExcitementLevel = Format.ConvertStringToInt(Console.ReadLine(), $"What was your initial excitement level from 1-10 prior to playing {gameName}? (1: Horrible, 10: Amazing)");
            }


            //Get initial impressions
            Console.WriteLine($"What were your inital impressions and thoughts after your first play session of {gameName}?");
            initialImpressions = Format.ReplaceCommasInString(Console.ReadLine());

            //Get sequel info
            if (franchiseID != -1 && Format.GetClosedAnswer($"Is there a game following {gameName} in the franchise of {Menu.existingFranchises[Menu.FindFranchise(franchiseID)].franchiseName}? (y/n)"))
            {
                nextEntryID = GetSequelStatus();
            }
            
            //Check completed status
            completed = Format.GetClosedAnswer($"Have you completed {gameName}? (y/n)");
            if (completed)
            {
                CompletedGame completedVersion = new CompletedGame();
                completedVersion.GrabPlayedGameInfo(this);
                completedVersion.GetCompletedGameInfo();
            } else if (Format.GetClosedAnswer($"Are you currently playing {gameName}? (y/n)"))
            {
                playing = true;
                CurrentGame currentVersion = new CurrentGame();
                currentVersion.GrabPlayedGameInfo(this);
                currentVersion.GetCurrentGameInfo();
            } else {
                DroppedGame droppedVersion = new DroppedGame();
                droppedVersion.GrabPlayedGameInfo(this);
                droppedVersion.GetDroppedGameInfo();
            }


            


        }

        public int GetSequelStatus()
        {
            Menu.Franchise gameFranchise = Menu.existingFranchises[Menu.FindFranchise(franchiseID)];
            Console.WriteLine($"Please input the name of the game after {gameName} in the franchise of {gameFranchise.franchiseName}.");
            string followingEntry = Format.CheckForCommas(Console.ReadLine(), $"title for the sequel to {gameName}");
            for (int i = 0; i < gameFranchise.franchiseEntryIDs.Length; i++)
            {
                if (followingEntry.ToLower() == Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName)
                {
                    return gameFranchise.franchiseEntryIDs[i];
                }
            }
            if (nextEntryID == -1)
            {
                VideoGame titleSearch = Menu.FindGameByTitle(followingEntry);
                if (titleSearch != null)
                {
                    return titleSearch.gameID;
                } else {
                    if (Format.GetClosedAnswer($"{followingEntry} could not be found in our records. Would you like to add it? (y/n)"))
                    {
                        VideoGame nextGame = new VideoGame();
                        nextGame.AddGameFromFranchise(followingEntry, franchiseID);
                        return nextGame.gameID;
                    } else {
                        if (Format.GetClosedAnswer($"Would you like to try find the game following {gameName} again? (y/n)"))
                        {
                            return GetSequelStatus();
                        } else {
                            return -1;
                        }
                    }
                }
            }
            return -1;
        }

        public void AddToPlayTime(float minutesPlayed)
        {
            hoursPlayed += minutesPlayed / 60;
        }

        public void AddInitialImpressions()
        {

        }

        public void AddInitialExcitement(int initialExcitement)
        {
            initialExcitementLevel = initialExcitement;
        }

        public void AddRating()
        {

        }

        public void EditPlayAgainStatus()
        {

        }




        public string ReadInitialImpressions()
        {
            return initialImpressions;
        }

        public float ReadHoursPlayed()
        {
            return hoursPlayed;
        }

        public int ReadInitialExcitement()
        {
            return initialExcitementLevel;
        }

        public int ReadRating()
        {
            return rating;
        }

        public int ReadIntialExcitment()
        {
            return initialExcitementLevel;
        }

        public void GrabPlayedGameInfo(PlayedGame parentGame)
        {
            GrabGameInfo(parentGame);
            hoursPlayed = parentGame.hoursPlayed;
            rating = parentGame.rating;
            initialExcitementLevel = parentGame.initialExcitementLevel;
            initialImpressions = parentGame.initialImpressions;
            nextEntryID = parentGame.nextEntryID;
            completed = parentGame.completed;
            playing = parentGame.playing;

            logMessages = parentGame.logMessages;
            modded = parentGame.modded;
            mainPlayDevice = parentGame.mainPlayDevice;
        }
    }
}