namespace GameTrackerEx01
{
    class PlayedGame : VideoGame
    {
        private float hoursPlayed;
        private int rating;
        private int initialExcitementLevel;
        private string initialImpressions;
        private bool completed;
        private bool playing;

        public void GetInfo()
        {
            //Get current playtime
            Console.WriteLine($"Please input the approximate number of hours you have spent with {gameName}");
            hoursPlayed = Format.ConvertStringToFloat(Console.ReadLine(), $"Please input the approximate play time you have spent with {gameName}");

            //Get current rating
            

            //Get initial excitement level


            //Get initial impressions

            
            //Check currently playing status
            

            //Check completed status


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

        public void EditPlayAgainStatus()
        {

        }




        public string ReadInitialImpressions()
        {
            return initialImpressions;
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

        public int ReadIntialExcitment()
        {
            return initialExcitementLevel;
        }
    }
}