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
                    GetPrequelStatus();
                }
            } else {
                UnpurchasedGame unpurchasedVersion = new UnpurchasedGame();
                unpurchasedVersion.GrabUnplayedInfo(this);
                unpurchasedVersion.GetUnpurchasedGameInfo();
                //Get sequel status
                if (franchiseID != -1 && Format.GetClosedAnswer($"Is {gameName} a sequel? (y/n)"))
                {
                    sequel = true;
                    GetPrequelStatus();
                    
                }
            }
        }

        public void GetPrequelStatus()
        {
            Franchise gameFranchise = Menu.existingFranchises[Menu.FindFranchise(franchiseID)];
            Console.WriteLine($"Please input the name of the game preceeding {gameName} in the franchise of {gameFranchise.franchiseName}.");
            string previousEntry = Format.CheckForCommas(Console.ReadLine(), "previous franchise entry");
            if (gameFranchise.franchiseEntryIDs.Length > 1)
            {
                for (int i = 0; i < gameFranchise.franchiseEntryIDs.Length; i++)
                {
                    // Console.WriteLine($"franchiseEntryIDs should have {gameFranchise.franchiseEntryIDs.Length} entries, and the current index is {i} and the current gameID is {gameFranchise.franchiseEntryIDs[i]}");
                    // if (Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]) == null)
                    // {
                    //     Console.WriteLine($"Game with ID {gameFranchise.franchiseEntryIDs[i]} could not be found in our records.");
                    // }
                    // Console.WriteLine($"Comparing {previousEntry.ToLower()} to {Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName.ToLower()}");
                    string nextIDName = Menu.FindGameByID(gameFranchise.franchiseEntryIDs[i]).gameName.ToLower();
                    if (previousEntry.ToLower() == nextIDName)
                    {
                        previousEntryID = gameFranchise.franchiseEntryIDs[i];
                        Menu.FindGameByID(previousEntryID).SetSequel(gameID);
                    }
                }
            }
            if (previousEntryID == -1)
            {
                VideoGame titleSearch = Menu.FindGameByTitle(previousEntry);
                if (titleSearch != null)
                {
                    previousEntryID = titleSearch.gameID;
                    Menu.FindGameByID(previousEntryID).SetSequel(gameID);
                } else {
                    if (Format.GetClosedAnswer($"{previousEntry} could not be found in our records. Would you like to add it? (y/n)"))
                    {
                        VideoGame previousGame = new VideoGame();
                        previousGame.AddGameFromFranchise(previousEntry, franchiseID);
                        previousEntryID = previousGame.gameID;
                        Menu.FindGameByID(previousEntryID).SetSequel(gameID);
                    } else {
                        if (Format.GetClosedAnswer($"Would you like to try find the game preceeding {gameName} again? (y/n)"))
                        {
                            GetPrequelStatus();
                            return;
                        } else {
                            previousEntryID = -1;
                        }
                    }
                }
            }
            sequel = (previousEntryID == -1) ? false : true;
            SaveGameUpdates();
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

        public void EditSequelStatus()
        {
            if (previousEntryID == -1)
            {
                EditSequel();
                return;
            }
            Console.WriteLine($"What do you want to change regarding {gameName}'s prequel?\n");
            Console.WriteLine("1. Edit prequel details");
            Console.WriteLine("2. Remove prequel requirement");
            Console.WriteLine("0. Go back");
            int userInput = Format.GetSingleResponse(2, $"What do you want to change regarding {gameName}'s prequel?");
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
            if(Format.GetClosedAnswer($"Are you sure you want to remove {Menu.FindGameByID(previousEntryID).gameName} as the prequel to {gameName}? (y/n)"))
            {
                Menu.FindGameByID(previousEntryID).RemoveFromSequel(gameID);
                previousEntryID = -1;
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
            
            if (!sequel && !Format.GetClosedAnswer($"{gameName} is currently labeled as not being a sequel, are you sure you want to change this? (y/n)"))
            {
                return;
            }
            GetPrequelStatus();
            Console.WriteLine($"{gameName} is now set as the sequel to {Menu.FindGameByID(previousEntryID).gameName}");
            SaveGameUpdates();
        }

        public void EditExcitementLevel()
        {
            Console.WriteLine($"The current excitement level for {gameName} on file is {excitementLevel}\nWhat is the your updated excitement level?");
            int newExcitement = Format.ConvertStringToInt(Console.ReadLine(), $"What is your updated excitement level for {gameName}?");
            if (Format.GetClosedAnswer($"Are you sure you want to update your excitement level for {gameName} from {excitementLevel} to {newExcitement}? (y/n)"))
            {
                excitementLevel = newExcitement;
            }
            SaveGameUpdates();
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