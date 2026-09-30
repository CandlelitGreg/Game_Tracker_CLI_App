namespace GameTrackerEx01
{
    public class UnplayedGame : VideoGame
    {
        public int excitementLevel {get; set;}
        public bool sequel {get; set;}
        public int previousEntryID {get; set;} = -1;
        public bool purchased {get; set;}

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

            //Get sequel status
            if (franchiseID != -1 && Format.GetClosedAnswer($"Is {gameName} a sequel? (y/n)"))
            {
                sequel = true;
                previousEntryID = GetSequelStatus();
                if (previousEntryID == -1) sequel = false;
            }

            //Get purchased status
            if (Format.GetClosedAnswer($"Do you already own {gameName}? (y/n)"))
            {
                purchased = true;
                BackloggedGame backloggedVersion = new BackloggedGame();
                backloggedVersion.GrabUnplayedInfo(this);
                backloggedVersion.GetBackloggedGameInfo();
            } else {
                UnpurchasedGame unpurchasedVersion = new UnpurchasedGame();
                unpurchasedVersion.GrabUnplayedInfo(this);
                unpurchasedVersion.GetUnpurchasedGameInfo();
            }
        }

        public int GetSequelStatus()
        {
            Menu.Franchise gameFranchise = Menu.existingFranchises[Menu.FindFranchise(franchiseID)];
            Console.WriteLine($"Please input the name of the game preceeding {gameName} in the franchise of {gameFranchise.franchiseName}.");
            string previousEntry = Format.CheckForCommas(Console.ReadLine(), "previous franchise entry");
            for (int i = 0; i < gameFranchise.franchiseEntryIDs.Length; i++)
            {
                if (previousEntry.ToLower() == Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName)
                {
                    return gameFranchise.franchiseEntryIDs[i];
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
                            return GetSequelStatus();
                        } else {
                            return -1;
                        }
                    }
                }
            }
            return -1;
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
    }
}