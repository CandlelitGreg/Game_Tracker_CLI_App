using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class UnplayedGameMap : ClassMap<UnplayedGame>
    {
        public UnplayedGameMap()
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
        }
    }

    public class UnplayedGame : VideoGame
    {
        public int excitementLevel {get; set;}
        public bool sequel {get; set;}
        public int previousEntryID {get; set;} = -1;

        public void GetUnplayedInfo()
        {
            //Get excitement level
            Console.WriteLine($"How would you rate your current excitement for {gameName} on a scale from 1-10? (1: Horrible, 10: Amazing)");
            excitementLevel = Format.ConvertStringToInt(Console.ReadLine(), $"How would you rate your current excitement for {gameName} on a scale from 1-10? (1: Horrible, 10: Amazing)");
            while (excitementLevel > 10 || excitementLevel < 1)
            {
                Console.WriteLine($"{excitementLevel} is outside of the 1-10 range. Please select a rating within 1 and 10");
                excitementLevel = Format.ConvertStringToInt(Console.ReadLine(), $"How would you rate your current excitement for {gameName} on a scale from 1-10? (1: Horrible, 10: Amazing)");
            }

            // //Get sequel status
            // if (franchiseID != -1 && Format.GetClosedAnswer($"Is {gameName} a sequel? (y/n)"))
            // {
            //     sequel = true;
            //     previousEntryID = GetPrequelStatus();
            //     if (previousEntryID == -1) sequel = false;
            // }

            //Get purchased status
            if (Format.GetClosedAnswer($"Do you already own {gameName}? (y/n)"))
            {
                purchased = true;
                BackloggedGame backloggedVersion = new BackloggedGame();
                backloggedVersion.GrabUnplayedInfo(this);
                backloggedVersion.GetBackloggedGameInfo();
                //Get sequel status
                if (franchiseID != -1 && Format.GetClosedAnswer($"Is {gameName} a sequel? (y/n)"))
                {
                    sequel = true;
                    previousEntryID = GetPrequelStatus();
                    if (previousEntryID == -1) sequel = false;
                    backloggedVersion.previousEntryID = previousEntryID;
                    backloggedVersion.sequel = sequel;
                    backloggedVersion.SaveGameInfo();
                }
            } else {
                UnpurchasedGame unpurchasedVersion = new UnpurchasedGame();
                unpurchasedVersion.GrabUnplayedInfo(this);
                unpurchasedVersion.GetUnpurchasedGameInfo();
                //Get sequel status
                if (franchiseID != -1 && Format.GetClosedAnswer($"Is {gameName} a sequel? (y/n)"))
                {
                    sequel = true;
                    previousEntryID = GetPrequelStatus();
                    if (previousEntryID == -1) sequel = false;
                    unpurchasedVersion.previousEntryID = previousEntryID;
                    unpurchasedVersion.sequel = sequel;
                    unpurchasedVersion.SaveGameInfo();
                }
            }
        }

        public int GetPrequelStatus()
        {
            Franchise gameFranchise = Menu.existingFranchises[Menu.FindFranchise(franchiseID)];
            Console.WriteLine($"Please input the name of the game preceeding {gameName} in the franchise of {gameFranchise.franchiseName}.");
            string previousEntry = Format.CheckForCommas(Console.ReadLine(), "previous franchise entry");
            if (gameFranchise.franchiseEntryIDs.Length > 1)
            {
                for (int i = 0; i < gameFranchise.franchiseEntryIDs.Length; i++)
                {
                    Console.WriteLine($"franchiseEntryIDs should have {gameFranchise.franchiseEntryIDs.Length} entries, and the current index is {i} and the current gameID is {gameFranchise.franchiseEntryIDs[i]}");
                    if (Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]) == null)
                    {
                        Console.WriteLine($"Game with ID {gameFranchise.franchiseEntryIDs[i]} could not be found in our records.");
                    }
                    Console.WriteLine($"Comparing {previousEntry.ToLower()} to {Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName.ToLower()}");
                    string nextIDName = Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName.ToLower();
                    if (previousEntry.ToLower() == nextIDName)
                    {
                        return gameFranchise.franchiseEntryIDs[i];
                    }
                }
            }
            if (previousEntryID == -1)
            {
                VideoGame titleSearch = Menu.FindGameByTitle(previousEntry);
                if (titleSearch != null)
                {
                    return titleSearch.gameID;
                } else {
                    if (Format.GetClosedAnswer($"{previousEntry} could not be found in our records. Would you like to add it? (y/n)"))
                    {
                        VideoGame previousGame = new VideoGame();
                        previousGame.AddGameFromFranchise(previousEntry, franchiseID);
                        return previousGame.gameID;
                    } else {
                        if (Format.GetClosedAnswer($"Would you like to try find the game preceeding {gameName} again? (y/n)"))
                        {
                            return GetPrequelStatus();
                        } else {
                            return -1;
                        }
                    }
                }
            }
            return -1;
        }

        public bool CheckWaitingForPreviousEntry()
        {
            if (franchiseID == -1 || previousEntryID == -1)
            {
                return false;
            }
            if (Menu.FindGameByID(previousEntryID).completed)
            {
                return false;
            } 
            else
            {
                return true;
            }
        }

        public void EditPurchased()
        {

        }

        public void EditSequel()
        {

        }

        public void EditExcitementLevel()
        {

        }

        public int ReadExcitement()
        {
            return excitementLevel;
        }

        public bool ReadSequel()
        {
            return sequel;
        }

        public bool ReadPurchased()
        {
            return purchased;
        }

        public void GrabUnplayedInfo(UnplayedGame parentGame)
        {
            GrabGameInfo(parentGame);
            excitementLevel = parentGame.excitementLevel;
            sequel = parentGame.sequel;
            previousEntryID = parentGame.previousEntryID;
            purchased = parentGame.purchased;
        }

        public void PlayGame()
        {
            //Create Playedgame object

            //Transfer VG info

            //Get Played Game info

            //Create currentgame object

            //transfer PlayedGame info

            //Set FirstPlay date

            //get currentgame info

            //remove this object
        }

        public string DisplayUnplayedGameInfo()
        {
            string unplayedGameInfo = $"Excitement Level:  {excitementLevel}\n";
            if (sequel)
            {
                unplayedGameInfo += $"Sequel to:         {Menu.FindGameByID(previousEntryID).gameName}\n";
            } else
            {
                unplayedGameInfo += $"No relevant previous entry\n";
            }
            return unplayedGameInfo;
        }
    }
}