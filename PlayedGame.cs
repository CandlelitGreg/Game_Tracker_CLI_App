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
        }
    }


    public class PlayedGame : VideoGame
    {
        public float hoursPlayed {get; set;}
        public int rating {get; set;}
        public int initialExcitementLevel {get; set;}
        public string initialImpressions {get; set;} //Contains comma replaced format
        public int nextEntryID {get; set;} = -1;

        public string[] logMessages {get; set;} = [];
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
            
            //Check completed status
            completed = Format.GetClosedAnswer($"Have you completed {gameName}? (y/n)");
            if (completed)
            {
                CompletedGame completedVersion = new CompletedGame();
                completedVersion.GrabPlayedGameInfo(this);
                completedVersion.GetCompletedGameInfo();
                //Get sequel info
                if (franchiseID != -1 && Format.GetClosedAnswer($"Is there a game following {gameName} in the franchise of {Menu.existingFranchises[Menu.FindFranchise(franchiseID)].franchiseName}? (y/n)"))
                {
                    nextEntryID = GetSequelStatus();
                    completedVersion.nextEntryID = nextEntryID;
                    completedVersion.SaveGameInfo();
                }
            } else if (Format.GetClosedAnswer($"Are you currently playing {gameName}? (y/n)"))
            {
                playing = true;
                CurrentGame currentVersion = new CurrentGame();
                currentVersion.GrabPlayedGameInfo(this);
                currentVersion.GetCurrentGameInfo();
                //Get sequel info
                if (franchiseID != -1 && Format.GetClosedAnswer($"Is there a game following {gameName} in the franchise of {Menu.existingFranchises[Menu.FindFranchise(franchiseID)].franchiseName}? (y/n)"))
                {
                    nextEntryID = GetSequelStatus();
                    currentVersion.nextEntryID = nextEntryID;
                    currentVersion.SaveGameInfo();
                }
            } else {
                DroppedGame droppedVersion = new DroppedGame();
                droppedVersion.GrabPlayedGameInfo(this);
                droppedVersion.GetDroppedGameInfo();
                //Get sequel info
                if (franchiseID != -1 && Format.GetClosedAnswer($"Is there a game following {gameName} in the franchise of {Menu.existingFranchises[Menu.FindFranchise(franchiseID)].franchiseName}? (y/n)"))
                {
                    nextEntryID = GetSequelStatus();
                    droppedVersion.nextEntryID = nextEntryID;
                    droppedVersion.SaveGameInfo();
                }
            }

            


            


        }
        //TODO: Fix bug where newly created sequel is not attached to original game
        public int GetSequelStatus()
        {
            Menu.Franchise gameFranchise = Menu.existingFranchises[Menu.FindFranchise(franchiseID)];
            Console.WriteLine($"Please input the name of the game after {gameName} in the franchise of {gameFranchise.franchiseName}.");
            string followingEntry = Format.CheckForCommas(Console.ReadLine(), $"title for the sequel to {gameName}");
            if (gameFranchise.franchiseEntryIDs.Length > 1)
            {
                for (int i = 0; i < gameFranchise.franchiseEntryIDs.Length; i++)
                {
                    Console.WriteLine($"franchiseEntryIDs should have {gameFranchise.franchiseEntryIDs.Length} entries, and the current index is {i} and the current gameID is {gameFranchise.franchiseEntryIDs[i]}");
                    Console.WriteLine($"Comparing {followingEntry.ToLower()} to {Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName.ToLower()}");
                    string nextIDName = Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName.ToLower();
                    if (followingEntry.ToLower() == nextIDName)
                    {
                        return gameFranchise.franchiseEntryIDs[i];
                    }
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

        public VideoGame GetNextEntry()
        {
            if (nextEntryID != -1)
            {
                return Menu.FindGameByID(nextEntryID);
            }
            return null;
        }

        public void EditPlayAgainStatus()
        {

        }




        public string ReadInitialImpressions()
        {
            return Format.ReturnCommasToString(initialImpressions);
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

        public string DisplayPlayedInfo()
        {
            string playedGameInfo = 
                                   $"Hours Played:      {hoursPlayed}\n" +
                                   $"Rating:            {rating}\n" +
                                   $"Initial Excitement Level: {initialExcitementLevel}\n" +
                                   $"\n vvv---Initial Impressions---vvv \n{ReadInitialImpressions()}\n ^^^---Initial Impressions---^^^\n\n";
            if (nextEntryID != -1)
            {
                playedGameInfo += $"Next Entry:        {Menu.FindGameByID(nextEntryID).gameName}\n";
            }
            if (logMessages.Length > 0)
            {
                for (int i = 0; i < logMessages.Length; i++)
                {
                    playedGameInfo += $"Log Message {i + 1}:      {Format.ReturnCommasToString(logMessages[i])}\n";
                }
            }
            if (!string.IsNullOrWhiteSpace(mainPlayDevice))
            {
                playedGameInfo += $"Main Play Device:  {mainPlayDevice}\n";
            }
            if (modded)
            {
                playedGameInfo += $"Modded:      Yes\n";
            }
            return playedGameInfo;
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