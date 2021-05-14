using System;
using System.Threading.Tasks;

namespace ShishaBacon
{
    public class Rater
    {
        public const string DefaultName = "Unknown";
        private string name;
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value.Trim();
            }
        }

        public Rater(string name)
        {
            Name = name;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return ((Rater)obj).Name == Name;
        }
    }

    static class RaterSaved
    {
        private static Rater raterr = new Rater(Rater.DefaultName);

        public async static void Init(Action callback)
        {
            var ret = await Storage.GetRater();
            if (ret != null)
            {
                raterr = ret;
            }
            callback?.Invoke();
        }

        public static Rater GetRater()
        {
            return raterr;
        }

        public static void SetRater(Rater rater)
        {
            raterr = rater;
            Storage.SetRater(raterr);
        }
    }
}