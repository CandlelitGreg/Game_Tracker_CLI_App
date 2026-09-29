namespace GameTrackerEx01
{
    class UnplayedGame : VideoGame
    {
        public int excitementLevel;
        public bool sequel;
        public bool purchased;

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
    }
}