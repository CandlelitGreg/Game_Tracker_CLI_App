namespace GameTrackerEx01
{
    class DroppedGame : PlayedGame
    {
        private string reasonDropped;
        private bool wouldRetry;

        public void AddWhyDropped()
        {

        }

        public void updateRetryStatus()
        {

        }

        public string ReadWhyDropped()
        {
            return reasonDropped;
        }

        public bool ReadRetry()
        {
            return wouldRetry;
        }
    }
}