using System;
using System.Collections.Generic;
using System.Drawing;

namespace ShishaBacon
{
    public class Tabacco : IComparable<Tabacco>
    {
        private string name = "";
        private string manufactorer = "";
        public List<Rating> ratings = new List<Rating>();

        public string Name
        {
            get { return name; }
            set { name = value.Trim(); }
        }
        public string Manufactorer
        {
            get { return manufactorer; }
            set { manufactorer = value.Trim(); }
        }

        public Tabacco()
        {

        }

        public Tabacco(Tabacco old)
        {
            Name = old.Name;
            ratings = old.ratings;
            Manufactorer = old.Manufactorer;
        }



        private void SortRatings()
        {
            ratings.Sort();
        }

        public double GetAverageRating()
        {
            if (ratings.Count == 0)
            {
                return 0;
            }

            double tmp = 0;
            foreach (Rating rating in ratings)
            {
                tmp += rating.Value;
            }
            return tmp / ratings.Count;
        }

        public bool AddRating(Rating rating)
        {
            if (rating.Value == -1)
            {
                return false;
            }

            foreach (Rating rat in ratings)
            {
                if (rat.rater.Equals(rating.rater))
                {
                    return false;
                }
            }
            ratings.Add(rating);
            SortRatings();
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
            SortRatings();
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
            return -1;
        }

        public override string ToString()
        {
            return Name + " - " + Manufactorer;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            Tabacco tb = (Tabacco)obj;

            return tb.Name == Name && tb.Manufactorer == Manufactorer;
        }

        public int CompareTo(Tabacco other)
        {
            if (Name == other.Name)
            {
                return Manufactorer.CompareTo(other.Manufactorer);
            }
            return Name.CompareTo(other.Name);
        }
    }

    static class TabaccoList
    {
        private static List<Tabacco> list = new List<Tabacco>();

        private static void SortList()
        {
            list.Sort();
        }

        public static List<Tabacco> GetList()
        {
            return list;
        }

        public static List<Tabacco> GetFilteredList(string manufactorerName)
        {
            if (manufactorerName == "" || manufactorerName.ToLower() == "all")
            {
                return list;
            }
            return list.FindAll(x => { return x.Manufactorer.Equals(manufactorerName); });
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
            SortList();
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
                }
                else
                {
                    SortList();
                    list.Add(tb);
                }
            }
            Storage.SetTabaccoList(list);
        }

        public static List<string> GetManufactorers()
        {
            List<string> listn = new List<string>();

            foreach (Tabacco tb in list)
            {
                if (!listn.Contains(tb.Manufactorer))
                {
                    listn.Add(tb.Manufactorer);
                }
            }
            return listn;
        }
    }
}