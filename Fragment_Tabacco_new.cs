using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;
using Android.Views.InputMethods;

namespace ShishaBacon
{
    public class Fragment_Tabacco_new : Fragment
    {
        public EventHandler finished;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            var view = inflater.Inflate(Resource.Layout.layout_tabacco_new, container, false);

            view.FindViewById<Button>(Resource.Id.tn_btn).Click += (sender, e) =>
            {

                Tabacco tb = new Tabacco();
                tb.name = view.FindViewById<EditText>(Resource.Id.tn_name).Text;
                tb.manufactorer = view.FindViewById<EditText>(Resource.Id.tn_manufactorer).Text;
                TabaccoList.AddTabacco(tb);
                finished(this, null);
            };

            return view;
        }
    }
}