using Android.Bluetooth;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;
using System.Text;

namespace ShishaBacon
{
    public class Fragment_Bluetooth_Menu : Fragment
    {
        public EventHandler finished;
        BluetoothSocket socket;

        public Fragment_Bluetooth_Menu(BluetoothSocket socket)
        {
            this.socket = socket;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.layout_bluetooth_connection, container, false);

            view.FindViewById<Button>(Resource.Id.bc_go).Click += (sender, e) =>
            {
                bool isSend = view.FindViewById<Switch>(Resource.Id.bc_switch).Checked;

                if (isSend)
                {
                    BluetoothHelper.SendTo(socket);
                }  else
                {
                    BluetoothHelper.RecieveFrom(socket);
                }
            };
            return view;
        }
    }
}