using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.AppCompat.View.Menu;
using System;
using System.Collections.Generic;

namespace ShishaBacon
{
    public class Fragment_Tabacco_list : Fragment
    {
        public EventHandler<TabaccoListEventArgs> itemClicked;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.layout_tabacco_list, container, false);

            ListView list = view.FindViewById<ListView>(Resource.Id.tl_list);
            Spinner spinner = view.FindViewById<Spinner>(Resource.Id.tl_spinner);
            var manList = new List<string>();
            manList.Add("All");
            manList.AddRange(TabaccoList.GetManufactorers());
            spinner.Adapter = new ArrayAdapter(Activity, Resource.Layout.sb_listitem, manList);

            spinner.ItemSelected += (sender, e) =>
            {
                list.Adapter = new ArrayAdapter<Tabacco>(Context, Resource.Layout.tl_listitem, TabaccoList.GetFilteredList(spinner.GetItemAtPosition(e.Position).ToString()));
                list.Invalidate();
            };

            list.Adapter = new ArrayAdapter<Tabacco>(Context, Resource.Layout.tl_listitem, TabaccoList.GetList());
            list.TextFilterEnabled = true;
            list.ItemClick += (sender, e) =>
            {
                Tabacco tb = TabaccoList.GetItemByString(((TextView)e.View).Text);

                itemClicked(this, new TabaccoListEventArgs(tb));
            };

            return view;
        }
    }

    public class TabaccoListEventArgs : EventArgs
    {
        public Tabacco Tabacco { get; set; }

        public TabaccoListEventArgs(Tabacco tabacco)
        {
            Tabacco = tabacco;
        }
    }
}