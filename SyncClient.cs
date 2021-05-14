using Android;
using Android.Bluetooth;
using Android.Content.PM;
using Android.Runtime;
using AndroidX.AppCompat.App;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using Java.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShishaBacon
{
    public class SyncClient
    {
        public static void startSyncing()
        {
            
        }
    }

    class BluetoothHelper
    {
        private static bool hasPermission = false;

        public static BluetoothAdapter GetAdapter()
        {
            BluetoothAdapter adapter = BluetoothAdapter.DefaultAdapter;

            if (adapter == null)
                throw new Exception("No Bluetooth adapter found.");

            if (!adapter.IsEnabled)
                throw new Exception("Bluetooth adapter is not enabled.");

            if (!hasPermission)
                throw new Exception("Bluetooth Permission not granted.");

            return adapter;
        }
        
        public static void getPermission(Android.Content.Context content,Android.App.Activity activity)
        {
            if (ContextCompat.CheckSelfPermission(content, Manifest.Permission.Bluetooth) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new string[] { Manifest.Permission.Bluetooth }, 1);
            }
            hasPermission = ContextCompat.CheckSelfPermission(content, Manifest.Permission.Bluetooth) == (int)Permission.Granted;
        }

        public static string isPossible()
        {
            try
            {
                GetAdapter();
                return "";
            } catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string getDeviceName()
        {
            BluetoothAdapter adapter = GetAdapter();
            return adapter.Name;
        }

        public static List<BluetoothDevice> getAvaiableDevices()
        {
            BluetoothAdapter adapter = GetAdapter();
            return adapter.BondedDevices.ToList();
        }

        public static List<string> getAvaiableDeviceNames()
        {
            List<string> list = new List<string>();
            BluetoothAdapter adapter = GetAdapter();

            foreach (var device in adapter.BondedDevices)
            {
                list.Add(device.Name);
            }

            return list;
        }

        public static BluetoothDevice GetDeviceByName(string name)
        {
            BluetoothAdapter adapter = GetAdapter();
            foreach (var device in adapter.BondedDevices)
            {
                if (device.Name == name)
                {
                    return device;
                }
            }

            return null;
        }

        public static async void SendTo(BluetoothSocket socket)
        {
            var list = TabaccoList.GetList();
            string listString = JsonConvert.SerializeObject(list, Formatting.Indented);
            byte[] array = Encoding.ASCII.GetBytes(listString);

            await socket.OutputStream.WriteAsync(array, 0, array.Length);
        }

        public static async void RecieveFrom(BluetoothSocket socket)
        {

            string res = await Task.Factory.StartNew(() => {
                StringBuilder builder = new StringBuilder();
                byte[] buffer = new byte[1024];

                int bytes;
                while (true)
                {
                    try
                    {

                        bytes = socket.InputStream.Read(buffer, 0, buffer.Length);

                        if (bytes > 0)
                        {
                            builder.Append(Encoding.ASCII.GetString(buffer));
                        }
                    }
                    catch (Java.IO.IOException)
                    {
                        break;
                    }
                }
                Console.WriteLine(builder.ToString());
                return builder.ToString();
            });
            Console.WriteLine(res);
        }
    }
}