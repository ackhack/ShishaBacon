using Android;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using AndroidX.Fragment.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;

namespace ShishaBacon
{
    public class Fragment_Sync_File : Fragment
    {
        public ErrorEventHandler finished;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate(Resource.Layout.layout_sync_file, container, false);

            view.FindViewById<Button>(Resource.Id.sf_export).Click += async (sender, e) =>
            {
                ActivityCompat.RequestPermissions(Activity, new string[] { Manifest.Permission.WriteExternalStorage }, 1);

                string filename = "ShishaBacon_List_" + DateTime.Now.ToString("dd-MM-yyyy_HH:mm") + ".json";

                string filePath = Path.Combine(Context.GetExternalFilesDir("").AbsolutePath, filename);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                File.WriteAllText(filePath, JsonConvert.SerializeObject(TabaccoList.GetList(), Formatting.Indented));

                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "ShishaBacon-Liste",
                    File = new ShareFile(filePath)
                });

                finished(this, null);
            };

            view.FindViewById<Button>(Resource.Id.sf_import).Click += async (sender, e) =>
            {
                try
                {
                    var result = await FilePicker.PickAsync();
                    if (result != null)
                    {
                        ActivityCompat.RequestPermissions(Activity, new string[] { Manifest.Permission.ReadExternalStorage }, 1);
                        string resString = File.ReadAllText(result.FullPath);
                        TabaccoList.ImportList(new List<Tabacco>(JsonConvert.DeserializeObject<Tabacco[]>(resString)));
                        view.FindViewById<TextView>(Resource.Id.sf_text).Text = "Imported File: " + result.FileName ;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    // The user canceled or something went wrong
                }
            };

            return view;
        }
    }
}