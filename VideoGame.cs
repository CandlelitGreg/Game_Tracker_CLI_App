using System;
using System.Collections.Generic;

namespace GameTrackerEx01
{
    class VideoGame
    {
        public int gameID;
        public string gameName {get; set;}
        public float avgGameLength;
        public List<Menu.Genre> gameGenres = [];
        public bool deckPlayable = false;
        public bool sessionGame;
        public bool worthCompletionist;
        public int franchiseID;
        public bool played;

        public void AddGame()
        {
            //Assign the gameID
            // gameID = Menu.GetAndUpdateNextGameID();
            
            //Get the game's title
            Console.WriteLine("Please input the video game title below");
            gameName = Format.CheckForCommas(Console.ReadLine(), "video game title");

            //Get deckPlayable stat
            if (Format.GetClosedAnswer($"Is {gameName} playable on the SteamDeck? (y/n)"))
            {
                deckPlayable = true;
            }

            //Get fitting existing game genres
            if (Menu.existingGenres.Count > 0)
            {
                Console.WriteLine($"Please select the appropriate genres for {gameName}");
                for (int i = 0; i < Menu.existingGenres.Count; i++)
                {
                    Console.WriteLine($"{i+1}. {Menu.existingGenres[i].genreName}");
                }
                Console.WriteLine($"\n{Menu.existingGenres.Count + 1}. Create a new genre for {gameName}");
                int[] matchingGenres = Format.GetManyMenuResponses(Menu.existingGenres.Count + 1);
                for (int i = 0; i < matchingGenres.Length; i++)
                {
                    if (matchingGenres[i] == Menu.existingGenres.Count + 1)
                    {
                        //Get fitting new game genres
                        if (Format.GetClosedAnswer($"Would you like to create a new genre to attach to {gameName}? (y/n)"))
                        {
                            int currentGenreCount = Menu.existingGenres.Count;
                            Menu.AddNewGenre();
                            if (Menu.existingGenres.Count > currentGenreCount)
                            {
                                for (int q = currentGenreCount; q < Menu.existingGenres.Count; q++)
                                {
                                    gameGenres.Add(Menu.existingGenres[q]);
                                }
                            }
                        }
                    } 
                    else 
                    {
                        gameGenres.Add(Menu.existingGenres[matchingGenres[i]-1]);
                    }
                    
                }
            }

            

            //Get average completion time
            Console.WriteLine($"Please input the average play time to complete {gameName}");
            avgGameLength = Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the average play time to complete {gameName}");

            //Get Session game status
            if (Format.GetClosedAnswer($"Would you consider {gameName} a session game? (y/n)"))
            {
                sessionGame = true;
            }

            //Get Completionist status
            if (Format.GetClosedAnswer($"Would you consider achieving 100% completion on {gameName}? (y/n)"))
            {
                worthCompletionist = true;
            }

            //Check franchise status
            if (Format.GetClosedAnswer($"Is {gameName} part of a larger franchise? (y/n)"))
            {
                Console.WriteLine($"Please select the franchise {gameName} is a part of:");
                for (int i = 0; i < Menu.existingFranchises.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {Menu.existingFranchises[i].franchiseName}");
                }
                Console.WriteLine($"\n0. If {gameName} is part of an unlisted franchise please press 0\n");
                int franchiseInput = Format.GetSingleResponse(1, $"Please select the franchise {gameName} is a part of:");
                if (franchiseInput == 0)
                {
                    franchiseID = Menu.AddNewFranchise();
                } 
                else 
                {
                    franchiseID = Menu.existingFranchises[franchiseInput-1].franchiseID;
                }

                //Add game to franchise
                if (franchiseID != -1)
                {
                    Menu.AddGameToFranchise(gameID, franchiseID);
                }
                
                
            }


            //Get game played status
            played = Format.GetClosedAnswer($"Have you played {gameName} before? (y/n)");
            if (played) {
                PlayedGame playedVersion = new PlayedGame();
                playedVersion.gameID = gameID;
                playedVersion.gameName = gameName;
                playedVersion.avgGameLength = avgGameLength;
                playedVersion.gameGenres = gameGenres;
                playedVersion.deckPlayable = deckPlayable;
                playedVersion.franchiseID = franchiseID;
                playedVersion.sessionGame = sessionGame;
                playedVersion.worthCompletionist = worthCompletionist;
                playedVersion.played = true;
                playedVersion.GetInfo();
            }

            

        }

        public void UpdateGameLength()
        {
            
        }

        public void UpdatePlayStatus()
        {

        }

        public void EditTitle()
        {

        }

        public void EditDeckStatus()
        {

        }

        public void addGenres()
        {

        }

        public void RemoveGenres()
        {

        }

        public string ReadTitle()
        {
            return gameName;
        }

        public float ReadAvgGameTime()
        {
            return avgGameLength;
        }

        public List<Menu.Genre> ReadGenres()
        {
            return gameGenres;
        }

        public bool ReadDeckPlayable()
        {
            return deckPlayable;
        }

        public bool ReadPlayed()
        {
            return played;
        }

        public string DisplayGameDetails()
        {
            string details = "";
            details += $"Title:             {gameName}\n";
            details += $"Avg Game Length:   {avgGameLength}\n";
            if (gameGenres.Count > 0)
            {
                details += $"Game Genres:       {gameGenres[0].genreName}";
            }
            for (int i = 1; i < gameGenres.Count; i++)
            {
                details += $" || {gameGenres[i].genreName}";
            }
            details += "\n";
            details += $"Playable on Deck:  {deckPlayable}\n";
            return details;
        }
    }
}