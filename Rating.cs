namespace ShishaBacon
{
    public class Rating
    {
        public const int Scaling = 10;
        private const int maxRating = 10;
        private const int minRating = 0;

        public Rater rater;
        private int value;
        public int Value
        {
            get { return value; }
            set
            {
                if (value >= minRating * Scaling && value <= maxRating * Scaling)
                {
                    this.value = value;
                }
            }
        }

        public Rating(Rater rater, int rating)
        {
            this.rater = rater;
            this.value = rating;
        }

        public override string ToString()
        {
            return rater.name + ": " + ((double)value / Scaling);
        }
    }
}