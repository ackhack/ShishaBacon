using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace ShishaBacon
{
    class Storage
    {
        public async static Task<Rater> GetRater()
        {
            try
            {
                return JsonConvert.DeserializeObject<Rater>(await SecureStorage.GetAsync("ShishaBacon_Rater"));
            }
            catch (Exception)
            {
                return null;
                // Possible that device doesn't support secure storage on device.
            }
            ;
        }

        public static void SetRater(Rater rater)
        {
            try
            {
                _ = SecureStorage.SetAsync("ShishaBacon_Rater", JsonConvert.SerializeObject(rater, Formatting.Indented));
            }
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }

        public async static Task<List<Tabacco>> GetTabaccoList()
        {
            try
            {
                return new List<Tabacco>(JsonConvert.DeserializeObject<Tabacco[]>(await SecureStorage.GetAsync("ShishaBacon_List")));
            }
            catch (Exception)
            {
                return null;
                // Possible that device doesn't support secure storage on device.
            }
            
        }

        public static void SetTabaccoList(List<Tabacco> list)
        {
            try
            {
                _ = SecureStorage.SetAsync("ShishaBacon_List", JsonConvert.SerializeObject(list.ToArray(), Formatting.Indented));
            }
            catch (Exception)
            {
                // Possible that device doesn't support secure storage on device.
            }
        }
    }
}