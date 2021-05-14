using Android.App;
using Android.Content;
using Android.OS;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ShishaBacon
{
    [Activity(Label = "ShishaBacon Importer")]
    [IntentFilter(new[] { "android.intent.action.VIEW"}, DataMimeType = "application/json", Categories = new[] { "android.intent.category.DEFAULT", "android.intent.category.BROWSABLE" })]
    public class Activity_File_Import : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                Stream stream = ContentResolver.OpenInputStream(Intent.Data);
                string res = new StreamReader(stream).ReadToEnd();
                if (!string.IsNullOrWhiteSpace(res))
                    TabaccoList.ImportList(new List<Tabacco>(JsonConvert.DeserializeObject<Tabacco[]>(res)));
            }
            catch (Exception)
            {

            }

            StartActivity(typeof(MainActivity));
        }
    }
}