using System;
using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public class PlayedGame : VideoGame
    {
        public float hoursPlayed {get; set;}
        public int rating {get; set;}
        public int initialExcitementLevel {get; set;}
        public string initialImpressions {get; set;} //Contains comma replaced format
        public int nextEntryID {get; set;} = -1;

        // public string[] logMessages {get; set;} = [];
        public int[] logMessageIDs {get; set;} = [];

        public string mainPlayDevice {get; set;}
        public bool modded {get; set;}

        public void GetPlayInfo(string completedQuestion = $"Have you completed ", string currentQuestion = "Are you currently playing ")
        {
            //Get current playtime
            Console.WriteLine($"Please input the approximate number of hours you have spent with {gameName}");
            hoursPlayed = Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the approximate play time you have spent with {gameName}");

            EditRating();

            EditInitialExcitement();

            //Get initial impressions
            AddInitialImpressions();
            
            //Check completed status
            completed = Format.GetClosedAnswer($"{completedQuestion}{gameName}? (y/n)");
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
            } else if (Format.GetClosedAnswer($"{currentQuestion}{gameName}? (y/n)"))
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
        public void StartPlaythrough()
        {   
            played = true;
            purchased = true;
            GetPlayInfo("Did you complete ", "Are you going to keep playing ");
            if (Format.GetClosedAnswer($"Do you want to add a log to your first play session of {gameName}? (y/n)"))
            {
                AddLog();
                Menu.FindLog(Menu.mainFiles.nextLogID - 1).startedGame = true;
                if (completed)
                {
                    Menu.FindLog(Menu.mainFiles.nextLogID - 1).finishedGame = true;
                }
                else if (!playing && !completed)
                {
                    Menu.FindLog(Menu.mainFiles.nextLogID - 1).droppedGame = true;
                }
            }
        }
        //TODO: Fix bug where newly created sequel is not attached to original game ?? Fixed ?? (I believe so...)
        public int GetSequelStatus()
        {
            Franchise gameFranchise = Menu.existingFranchises[Menu.FindFranchise(franchiseID)];
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

        public void AddInitialImpressions()
        {
            Console.WriteLine($"What were your inital impressions and thoughts after your first play session of {gameName}?");
            initialImpressions = Format.ReplaceCommasInString(Console.ReadLine());
        }

        public void EditHoursPlayed()
        {
            Console.WriteLine($"You currently have {hoursPlayed} hours logged with {gameName} on file\nWhat is the your playtime?");
            float newHours = Format.ConvertStringToFloat(Console.ReadLine(), $"What is your current playtime on {gameName}?");
            if (Format.GetClosedAnswer($"Are you sure you want to update your hours played from {hoursPlayed} to {newHours} for {gameName}? (y/n)"))
            {
                if (replay)
                {
                    Menu.FindDetailedGameByID<CompletedGame>(Menu.FindDetailedGameByID<CurrentGame>(gameID).originalPlaythroughID).hoursPlayed += newHours - hoursPlayed;
                }
                hoursPlayed = newHours;
            }
            SaveGameUpdates();
        }

        public void EditRating()
        {
            if (rating != 0)
            {
                Console.WriteLine($"Your current rating on file for {gameName} is {rating}\nWhat is your updated rating?");
            } else
            {
                Console.WriteLine($"What is your rating of {gameName} from 1-10? (1: Horrible, 10: Amazing)?");
            }
            int newRating = Format.ConvertStringToInt(Console.ReadLine(), $"What is your current rating for {gameName} from 1-10? (1: Horrible, 10: Amazing)?");
            while (newRating > 10 || newRating < 1)
            {
                Console.WriteLine($"{newRating} is outside of the 1-10 range. Please select a rating within 1 and 10");
                newRating = Format.ConvertStringToInt(Console.ReadLine(), $"What is your rating of {gameName} from 1-10? (1: Horrible, 10: Amazing)");
            }
            if (rating != 0 && !Format.GetClosedAnswer($"Are you sure you want to update your rating of {gameName} from {rating} to {newRating}? (y/n)"))
            {
                return;
            }
            rating = newRating;
        }

        public void EditInitialExcitement()
        {
            if (initialExcitementLevel != 0 )
            {
                if (!Format.GetClosedAnswer($"Would you like to update your initial excitement rating from {initialExcitementLevel} for {gameName}? (y/n)"))
                {
                    return;
                }
                Console.WriteLine($"What would you like to change your initial excitement rating to?");
            }
            else
            {
                Console.WriteLine($"What was your initial excitement level from 1-10 prior to playing {gameName}? (1: Horrible, 10: Amazing)");
            }
            int newExcitementLevel = Format.ConvertStringToInt(Console.ReadLine(), $"What was your initial excitement level from 1-10 prior to playing {gameName}? (1: Horrible, 10: Amazing)");
            while (newExcitementLevel > 10 || newExcitementLevel < 1)
            {
                Console.WriteLine($"{newExcitementLevel} is outside of the 1-10 range. Please select a rating within 1 and 10");
                newExcitementLevel = Format.ConvertStringToInt(Console.ReadLine(), $"What was your initial excitement level from 1-10 prior to playing {gameName}? (1: Horrible, 10: Amazing)");
            }
            if (initialExcitementLevel != 0 && !Format.GetClosedAnswer($"Are you sure you want to update your initial excitement level of {gameName} from {initialExcitementLevel} to {newExcitementLevel}? (y/n)\nThis statistic is intended to be representative of your very first thoughts after the first hour/s."))
            {
                return;
            }
            initialExcitementLevel = newExcitementLevel;
        }

        public void EditSequelStatus()
        {
            if (nextEntryID == -1)
            {
                EditSequel();
                return;
            }
            Console.WriteLine($"What do you want to change regarding {gameName}'s sequel?\n");
            Console.WriteLine("1. Edit sequel details");
            Console.WriteLine("2. Remove attached sequel");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What do you want to change about {gameName}'s sequel?");
            switch (userInput)
            {
                case 0:
                    return;
                case 1:
                    EditSequel();
                    break;
                case 2:
                    RemovePrequel();
                    break;
            }
            SaveGameUpdates();
        }

        public void RemovePrequel()
        {
            if(Format.GetClosedAnswer($"Are you sure you want to remove {Menu.FindGameByID(nextEntryID).gameName} as the sequel to {gameName}? (y/n)"))
            {
                Menu.FindGameByID(nextEntryID).RemoveFromSequel(gameID);
                nextEntryID = -1;
            }
        }

        public void EditSequel()
        {
            if (franchiseID == -1 && Format.GetClosedAnswer($"{gameName} is not attached to any existing franchise, you will have to attach it to an existing or new franchise in order to alter it's sequel status\nWould you like to attach {gameName} to a franchise? (y/n)"))
            {
                AddToFranchise();
            }
            if (franchiseID == -1)
            {
                Console.WriteLine($"{gameName} could not have its sequel status altered as it is not part of a franchise");
                return;
            }
            
            if (nextEntryID == -1 && !Format.GetClosedAnswer($"{gameName} is currently labeled as not having a sequel, are you sure you want to change this? (y/n)"))
            {
                return;
            }
            GetSequelStatus();
            Console.WriteLine($"{Menu.FindGameByID(nextEntryID).gameName} is now set as the sequel to {gameName}");
            SaveGameUpdates();
        }

        public void EditLogs()
        {
            Console.WriteLine("Below are all of your log entries\n");
            string logDisplay = "";
            for (int i = 0; i < logMessageIDs.Length; i++)
            {
                logDisplay += $"Log Message {i + 1}:\n";
                LogMessage currentLog = Menu.FindLog(logMessageIDs[i]);
                logDisplay += $"        Log Date:             {currentLog.logDate}\n";
                logDisplay += $"        Log Play Duration:    {currentLog.playDuration}\n";
                logDisplay += $"        Log Entry:            {currentLog.ReadLogPreview()}\n";
            }
            Console.WriteLine("Please enter the number for the log entry you would like to edit, or press 0 to go back");
            int userInput = Format.GetSingleResponse(logMessageIDs.Length, $"What do you want to change about {gameName}'s sequel?");
            switch (userInput)
            {
                case 0:
                    return;
                default:
                    Menu.FindLog(logMessageIDs[userInput - 1]).EditLogInputs();
                    break;
            }
            EditLogs();
        }

        public void ChangePlayDevice()
        {
            Console.WriteLine($"You have currently recorded the {mainPlayDevice} as your main play device for {gameName}.\nWhat is your new main play device?");
            string newDevice = Format.CheckForCommas(Console.ReadLine(), $"new main device used to play {gameName}");
            if (Format.GetClosedAnswer($"Are you sure you want to update your main play device for {gameName} from {mainPlayDevice} to {newDevice}? (y/n)"))
            {
                mainPlayDevice = newDevice;
            }
            SaveGameUpdates();
        }

        public void ChangeModdedStatus()
        {
            if (modded && Format.GetClosedAnswer($"Are you sure you would like to remove the modded tag from {gameName}? (y/n)"))
            {
                modded = false;
            }
            else if (!modded && Format.GetClosedAnswer($"Are you sure you would like to add the modded tag from {gameName}? (y/n)"))
            {
                modded = true;
            }
            SaveGameUpdates();
        }

        public void CompleteGame<T>(T previousVersion) where T : PlayedGame
        {
            CompletedGame completeVersion = new CompletedGame();
            completeVersion.GrabPlayedGameInfo(previousVersion);
            completeVersion.GetCompletedGameInfo();
            switch (true)
            {
                case var _ when typeof(T) == typeof(CurrentGame):
                    Menu.existingCurrentGames.RemoveAll(game => game.gameID == gameID);
                    break;
                case var _ when typeof(T) == typeof(DroppedGame):
                    Menu.existingDroppedGames.RemoveAll(game => game.gameID == gameID);
                    break;
            }
            Menu.SortGameLists();
            completeVersion.playing = false;
            completeVersion.played = true;
            completeVersion.completed = true;
            if (Format.GetClosedAnswer($"Would you like to make a log entry regarding completing {gameName}? (y/n)"))
            {
                completeVersion.AddLog();
                Menu.FindLog(Menu.mainFiles.nextLogID - 1).finishedGame = true;
            }
        }

        public void DropGame<T>(T previousVersion) where T : PlayedGame
        {
            DroppedGame droppedVersion = new DroppedGame();
            droppedVersion.GrabPlayedGameInfo(previousVersion);
            droppedVersion.GetDroppedGameInfo();
            switch (true)
            {
                case var _ when typeof(T) == typeof(CurrentGame):
                    Menu.existingCurrentGames.RemoveAll(game => game.gameID == gameID);
                    break;
            }
            if (Format.GetClosedAnswer($"Would you like to make a log entry regarding dropping {gameName}? (y/n)"))
            {
                droppedVersion.AddLog();
                Menu.FindLog(Menu.mainFiles.nextLogID - 1).droppedGame = true;
            }
            droppedVersion.playing = false;
            droppedVersion.played = true;
        }

        public string ReadInitialImpressionsPreview()
        {
            string abreviatedImpressions = Format.ReturnCommasToString(initialImpressions);
            if (abreviatedImpressions.Length > 45)
            {
                abreviatedImpressions = $"{abreviatedImpressions.Substring(0,45)}...";
            }
            return abreviatedImpressions;
        }

        public string DisplayPlayedInfo()
        {
            string playedGameInfo = 
                                   $"Hours Played:             {hoursPlayed}\n" +
                                   $"Rating:                   {rating}\n" +
                                   $"Initial Excitement Level: {initialExcitementLevel}\n" +
                                   $"Initial Impressions:      {ReadInitialImpressionsPreview()}\n";
            if (nextEntryID != -1)
            {
                playedGameInfo += $"Next Entry:               {Menu.FindGameByID(nextEntryID).gameName}\n";
            }
            if (logMessageIDs.Length > 0)
            {
                for (int i = 0; i < logMessageIDs.Length; i++)
                {
                    playedGameInfo += $"Log Message {i + 1}:\n";
                    LogMessage currentLog = Menu.FindLog(logMessageIDs[i]);
                    playedGameInfo += $"        Log Date:             {currentLog.logDate}\n";
                    playedGameInfo += $"        Log Play Duration:    {currentLog.playDuration}\n";
                    playedGameInfo += $"        Log Entry:            {currentLog.ReadLogPreview()}\n";
                }
                playedGameInfo += "\n";
            }
            if (!string.IsNullOrWhiteSpace(mainPlayDevice))
            {
                playedGameInfo += $"Main Play Device:         {mainPlayDevice}\n";
            }
            if (modded)
            {
                playedGameInfo += $"Modded:                   Yes\n";
            }
            return playedGameInfo;
        }
        public void GrabPlayedGameInfo(PlayedGame parentGame)
        {
            GrabGameInfo(parentGame);
            hoursPlayed = parentGame.hoursPlayed;
            rating = parentGame.rating;
            initialExcitementLevel = parentGame.initialExcitementLevel;
            initialImpressions = parentGame.initialImpressions;
            nextEntryID = parentGame.nextEntryID;

            logMessageIDs = parentGame.logMessageIDs;
            modded = parentGame.modded;
            mainPlayDevice = parentGame.mainPlayDevice;
        }
    }
}