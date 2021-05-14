using System;

namespace ShishaBacon
{
    public class Rating: IComparable<Rating>
    {
        public const int Scaling = 10;
        public const int MaxRating = 10;
        public const int MinRating = 0;

        public Rater rater;
        private int value = -1;
        public int Value
        {
            get { return value; }
            set
            {
                if (value >= MinRating * Scaling && value <= MaxRating * Scaling)
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
            return rater.Name + ": " + ((double)value / Scaling);
        }

        public int CompareTo(Rating other)
        {
            return rater.Name.CompareTo(other.rater.Name);
        }
    }
}