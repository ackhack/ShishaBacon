using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.AppCompat.View.Menu;
using System;

namespace ShishaBacon
{
    public class Fragment_Tabacco_menu : Fragment
    {
        public EventHandler finished;
        private Tabacco tabacco;

        public Fragment_Tabacco_menu(Tabacco tabacco)
        {
            this.tabacco = tabacco;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.layout_tabacco_menu, container, false);

            //Name
            view.FindViewById<EditText>(Resource.Id.tm_name).Text = tabacco.name;

            //Manufactorer
            view.FindViewById<EditText>(Resource.Id.tm_manufacturer).Text = tabacco.manufactorer;

            //Rating
            view.FindViewById<SeekBar>(Resource.Id.tm_rating).Progress = tabacco.GetOwnRating();
            view.FindViewById<TextView>(Resource.Id.tm_rating_name).Text = "Bewertung: " + ((double)view.FindViewById<SeekBar>(Resource.Id.tm_rating).Progress) / Rating.Scaling;
            view.FindViewById<SeekBar>(Resource.Id.tm_rating).ProgressChanged += (sender, e) =>
            {
                view.FindViewById<TextView>(Resource.Id.tm_rating_name).Text = "Bewertung: " + ((double)e.Progress) / Rating.Scaling;
            };

            //AvgRating
            view.FindViewById<TextView>(Resource.Id.tm_avgRating).Text = "Durchschnittliche Bewertung: " + tabacco.GetAverageRating() / Rating.Scaling;

            //Rating List
            ListView listView = view.FindViewById<ListView>(Resource.Id.tm_ratings);
            listView.Adapter = new ArrayAdapter<Rating>(Context, Resource.Layout.tm_listitem, tabacco.GetRatings());

            //Save
            view.FindViewById<Button>(Resource.Id.tm_save).Click += (sender, e) =>
            {
                Tabacco newT = new Tabacco(tabacco);

                newT.name = view.FindViewById<EditText>(Resource.Id.tm_name).Text;
                newT.manufactorer = view.FindViewById<EditText>(Resource.Id.tm_manufacturer).Text;
                int rating = view.FindViewById<SeekBar>(Resource.Id.tm_rating).Progress;

                newT.UpdateRating(new Rating(RaterSaved.GetRater(), rating));

                TabaccoList.RemoveTabacco(tabacco);
                TabaccoList.AddTabacco(newT);
                finished(this, null);
            };

            //Delete
            view.FindViewById<Button>(Resource.Id.tm_delete).Click += (sender, e) =>
            {
                TabaccoList.RemoveTabacco(tabacco);
                finished(this, null);
            };


            return view;
        }
    }
}