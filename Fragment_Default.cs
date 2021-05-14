using Android.OS;
using Android.Views;
using AndroidX.Fragment.App;

namespace ShishaBacon
{
    class Fragment_Default : Fragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.layout_default, container, false);
            return view;
        }
    }
}