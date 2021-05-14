using System;
using System.Threading.Tasks;

namespace ShishaBacon
{
    public class Rater
    {
        public string name;

        public Rater(string name)
        {
            this.name = name;
        }
        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }
            return ((Rater)obj).name == name;
        }
    }

    static class RaterSaved
    {
        private static Rater raterr = new Rater("Unknown");

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