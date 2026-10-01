using System;
using System.Collections.Generic;
using CsvHelper.Configuration;

namespace GameTrackerEx01
{
    public sealed class VideoGameMap : ClassMap<VideoGame>
    {
        public VideoGameMap()
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
        }
    }



    public class VideoGame
    {
        public int gameID {get; set;}
        public string gameName {get; set;}
        public float avgGameLength {get; set;}
        public int[] gameGenreIDs {get; set;} = [];
        public bool deckPlayable {get; set;}
        public bool sessionGame {get; set;}
        public bool worthCompletionist {get; set;}
        public int franchiseID {get; set;} = -1;
        public bool played {get; set;}

        public int[] DLCIDs {get;set;} = [];


        public void AddGame()
        {
            //Get the game's title
            Console.WriteLine("Please input the video game title below");
            gameName = Format.CheckForCommas(Console.ReadLine(), "video game title");
            VideoGame existenceCheck = Menu.FindGameByTitle(gameName);
            while (existenceCheck != null)
            {
                if (Format.GetClosedAnswer($"{gameName} already exists, do you want to read it's information? (y/n)"))
                {
                    existenceCheck.DisplayGameDetails();
                    return;
                } else if (Format.GetClosedAnswer($"Would you like to add a different game? (y/n)"))
                {
                    gameName = Format.CheckForCommas(Console.ReadLine(), "video game title");
                    existenceCheck = Menu.FindGameByTitle(gameName);
                }
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
                                    gameGenreIDs = gameGenreIDs.Append(Menu.existingGenres[q].genreID).ToArray();
                                    Menu.existingGenres[q].AttachGameToGenre(gameID);
                                }
                            }
                        }
                    } 
                    else 
                    {
                        gameGenreIDs = gameGenreIDs.Append(Menu.existingGenres[matchingGenres[i]-1].genreID).ToArray();
                        Menu.existingGenres[matchingGenres[i]-1].AttachGameToGenre(gameID);
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

        public int[] ReadGenres()
        {
            return gameGenreIDs;
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
            if (gameGenreIDs.Length > 0)
            {
                details += $"Game Genres:       {Menu.existingGenres[Menu.FindGenre(gameGenreIDs[0])].genreName}";
            }
            for (int i = 1; i < gameGenreIDs.Length; i++)
            {
                details += $" || {Menu.existingGenres[Menu.FindGenre(gameGenreIDs[i])].genreName}";
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
            gameGenreIDs = parentGame.gameGenreIDs;
            deckPlayable = parentGame.deckPlayable;
            sessionGame = parentGame.sessionGame;
            worthCompletionist = parentGame.worthCompletionist;
            franchiseID = parentGame.franchiseID;
            played = parentGame.played;
            DLCIDs = parentGame.DLCIDs;
        }
    }
}