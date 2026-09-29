namespace GameTrackerEx01
{
    class UnpurchasedGame : UnplayedGame
    {
        private float fullPrice;
        private float lowestSalePrice;
        private bool released;

        public void EditFullPrice()
        {

        }

        public void EditSalePrice()
        {

        }

        public (float fullPrice, float salePrice) ReadPricing()
        {
            return (fullPrice, lowestSalePrice);
        }
    }
}