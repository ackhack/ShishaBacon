using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;

namespace ShishaBacon
{
    class Fragment_Rater_change : Fragment
    {
        public EventHandler finished;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.layout_rater_change, container, false);

            view.FindViewById<EditText>(Resource.Id.rc_text).Text = RaterSaved.GetRater().name;

            view.FindViewById<Button>(Resource.Id.rc_btn).Click += (sender, e) =>
            {
                RaterSaved.SetRater(new Rater(view.FindViewById<EditText>(Resource.Id.rc_text).Text));
                finished(this, null);
            };

            return view;
        }
    }
}