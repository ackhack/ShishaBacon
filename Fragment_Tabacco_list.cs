using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using AndroidX.AppCompat.View.Menu;
using System;

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

    public class TabaccoListEventArgs: EventArgs
    {
        public Tabacco Tabacco { get; set; }

        public TabaccoListEventArgs(Tabacco tabacco)
        {
            Tabacco = tabacco;
        }
    }
}