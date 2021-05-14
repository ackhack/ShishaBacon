using System;
using System.Collections.Generic;
using System.Drawing;

namespace ShishaBacon
{
    public class Tabacco
    {
        public string name = "";
        public string manufactorer = "";
        public List<Rating> ratings = new List<Rating>();
        public Image image = null;

        public Tabacco()
        {

        }

        public Tabacco(Tabacco old)
        {
            name = old.name;
            ratings = old.ratings;
            image = old.image;
            manufactorer = old.manufactorer;
        }

        public double GetAverageRating()
        {
            double tmp = 0;
            foreach (Rating rating in ratings)
            {
                tmp += rating.Value;
            }
            return tmp / ratings.Count;
        }

        public bool AddRating(Rating rating)
        {
            foreach (Rating rat in ratings)
            {
                if (rat.rater.Equals(rating.rater))
                {
                    return false;
                }
            }
            ratings.Add(rating);
            return true;
        }

        public void AddRatings(List<Rating> ratings)
        {
            foreach (Rating rating in ratings)
            {
                if (rating.rater.Equals(RaterSaved.GetRater()))
                {
                    continue;
                }

                UpdateRating(rating);
            }
        }

        public List<Rating> GetRatings()
        {
            return ratings;
        }

        public bool UpdateRating(Rating rating)
        {
            foreach (Rating rat in ratings)
            {
                if (rat.rater.Equals(rating.rater))
                {
                    rat.Value = rating.Value;
                    return true;
                }
            }
            ratings.Add(rating);
            return false;
        }

        public int GetOwnRating()
        {
            Rater me = RaterSaved.GetRater();
            foreach (Rating rat in ratings)
            {
                if (rat.rater.Equals(me))
                {
                    return rat.Value;
                }
            }
            return 0;
        }

        public override string ToString()
        {
            return name + " - " + manufactorer;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            Tabacco tb = (Tabacco)obj;

            return tb.name == name && tb.manufactorer == manufactorer;
        }
    }

    static class TabaccoList
    {
        private static List<Tabacco> list = new List<Tabacco>();

        public static List<Tabacco> GetList()
        {
            return list;
        }

        public async static void Init(Action callback)
        {
            var ret = await Storage.GetTabaccoList();

            if (ret != null)
            {
                list = ret;
            }
            else
            {
                list = new List<Tabacco>();
            }
            callback?.Invoke();
        }

        public static bool AddTabacco(Tabacco tabacco)
        {
            foreach (Tabacco tb in list)
            {
                if (tb.Equals(tabacco))
                {
                    return false;
                }
            }

            list.Add(tabacco);
            Storage.SetTabaccoList(list);

            return true;
        }

        public static bool RemoveTabacco(Tabacco tabacco)
        {
            bool success = list.Remove(tabacco);
            if (success)
            {
                Storage.SetTabaccoList(list);
            }
            return success;
        }

        public static Tabacco GetItemByString(string name)
        {
            foreach (Tabacco tb in list)
            {
                if (tb.ToString().Equals(name))
                {
                    return tb;
                }
            }
            return null;
        }

        public static void ImportList(List<Tabacco> importList)
        {
            foreach (Tabacco tb in importList)
            {
                if (list.Contains(tb))
                {
                    list.Find(t => { return t.Equals(tb); }).AddRatings(tb.GetRatings());
                } else
                {
                    list.Add(tb);
                }
            }
            Storage.SetTabaccoList(list);
        }
    }
}