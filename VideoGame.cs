using System;
using System.Collections.Generic;

namespace GameTrackerEx01
{
    public class VideoGame
    {
        public int gameID {get; set;}
        public string gameName {get; set;}
        public float avgGameLength {get; set;}
        public List<Menu.Genre> gameGenres {get; set;} = [];
        public bool deckPlayable {get; set;}
        public bool sessionGame {get; set;}
        public bool worthCompletionist {get; set;}
        public int franchiseID {get; set;} = -1;
        public bool played {get; set;}

        public void AddGame()
        {
            //Get the game's title
            Console.WriteLine("Please input the video game title below");
            gameName = Format.CheckForCommas(Console.ReadLine(), "video game title");

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

            GetGameInfo();
        }


        public void GetGameInfo()
        {
            //Assign the gameID
            gameID = Menu.GetAndUpdateNextGameID();
            

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


            //Get game played status
            played = Format.GetClosedAnswer($"Have you played {gameName} before? (y/n)");
            if (played) {
                PlayedGame playedVersion = new PlayedGame();
                playedVersion.GrabGameInfo(this);
                playedVersion.GetPlayInfo();
            } else {
                UnplayedGame unplayedVersion = new UnplayedGame();
                unplayedVersion.GrabGameInfo(this);
                unplayedVersion.GetUnplayedInfo();
            }

            

            

        }

        public void AddGameFromFranchise(string gameTitle, int existingFranchiseID)
        {
            gameName = gameTitle;
            franchiseID = existingFranchiseID;
            GetGameInfo();
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

        public void GrabGameInfo(VideoGame parentGame)
        {
            gameID = parentGame.gameID;
            gameName = parentGame.gameName;
            avgGameLength = parentGame.avgGameLength;
            gameGenres = parentGame.gameGenres;
            deckPlayable = parentGame.deckPlayable;
            sessionGame = parentGame.sessionGame;
            worthCompletionist = parentGame.worthCompletionist;
            franchiseID = parentGame.franchiseID;
            played = parentGame.played;
        }
    }
}