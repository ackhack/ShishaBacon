using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using Android.Bluetooth;
using System;
using Java.Util;
using Newtonsoft.Json;
using System.Text;
using Android.Runtime;
using System.Reflection;

namespace ShishaBacon
{
    public class Fragment_Sync_Bluetooth : Fragment
    {
        public EventHandler<BTDeviceArgs> finished;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.layout_sync_bluetooth, container, false);

            string possible = BluetoothHelper.isPossible();

            if (possible != "")
            {
                view.FindViewById<TextView>(Resource.Id.sb_text).Text = possible;
                return view;
            }
            
            view.FindViewById<TextView>(Resource.Id.sb_text).Text = "Dein GeräteName: " + BluetoothHelper.getDeviceName();

            ListView list = view.FindViewById<ListView>(Resource.Id.sb_list);

            list.Adapter = new ArrayAdapter<string>(Context, Resource.Layout.sb_listitem, BluetoothHelper.getAvaiableDeviceNames());
            list.TextFilterEnabled = true;
            list.ItemClick += (sender, e) =>
            {
                BluetoothDevice device = BluetoothHelper.GetDeviceByName(((TextView)e.View).Text);


                if (device != null)
                {
                    BluetoothHelper.GetAdapter().CancelDiscovery();
                    Console.WriteLine(device.GetType().ToString());
                    device.CreateBond();
                    Type t = device.GetType();
                    MethodInfo info = t.GetMethod("CreateRfcommSocket");
                    object o = info.Invoke(device, new object[] { 1 });
                    //		Name	"CreateRfcommSocketToServiceRecord"	string

                    var socket = (BluetoothSocket)o;
                    //var socket = device.CreateRfcommSocketToServiceRecord(UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
                    socket.Connect();

                    BTDeviceArgs args = new BTDeviceArgs();
                    args.socket = socket;
                    finished(this,args);
                }
            };

            return view;
        }
    }

    public class BTDeviceArgs: EventArgs
    {
        public BluetoothSocket socket { get; set; }
    }
}