namespace GameTrackerEx01
{
    class CompletedGame : PlayedGame
    {
        private string review;
        private bool wantToPlayAgain;
        private bool replaying;
        private int replayID; //Treat replay as a second gameID but hidden to user, only used to access replay specific information
        private int replayCount;
    }
}