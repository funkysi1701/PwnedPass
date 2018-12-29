using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace PwnedPasswords
{
    public class Cache
    {
        public static int SaveData(int p)
        {
            if(p == 0)
            {
                try
                {
                    HIBP data = new HIBP();
                    long acc = GetAccounts();
                    int bre = GetBreach();
                    if (acc > 1) data.TotalAccounts = acc;
                    if (bre > 1) data.TotalBreaches = bre;
                    Analytics.TrackEvent("SAVE DB");
                    if (acc > 1 && bre > 1)
                    {
                        data.Id = 1;
                        App.Database.SaveHIBP(data);
                        App.Database.EmptyDataBreach();
                        string result = App.GetAPI.GetHIBP("https://haveibeenpwned.com/api/v2/breaches");
                        if (result != null && result.Length > 0)
                        {
                            JArray job = (JArray)JsonConvert.DeserializeObject(result);
                            foreach (var item in job.Children())
                            {
                                DataBreach db = new DataBreach
                                {
                                    Title = item["Title"].ToString(),
                                    Name = item["Name"].ToString(),
                                    Domain = item["Domain"].ToString(),
                                    PwnCount = (int)item["PwnCount"],
                                    BreachDate = (DateTime)item["BreachDate"],
                                    AddedDate = (DateTime)item["AddedDate"],
                                    ModifiedDate = (DateTime)item["ModifiedDate"],
                                    IsVerified = (bool)item["IsVerified"],
                                    IsSensitive = (bool)item["IsSensitive"],
                                    IsRetired = (bool)item["IsRetired"],
                                    IsSpamList = (bool)item["IsSpamList"],
                                    IsFabricated = (bool)item["IsFabricated"],
                                    Description = Regex.Replace(item["Description"].ToString().Replace("&quot;", "'"), "<.*?>", string.Empty)
                                };
                                App.Database.SaveDataBreach(db);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Analytics.TrackEvent("Error");
                    Analytics.TrackEvent(e.Message);
                    Crashes.TrackError(e);
                }
            }
            return 1;
        }

        public static string LoadLastEmail()
        {
            Analytics.TrackEvent("LOAD Last Email");
            var table = App.Database.GetLastEmail();
            string email = "";
            foreach (var s in table)
            {
                email = s.Email;
            }
            return email;
        }

        public static void SaveLastEmail(string email)
        {
            Analytics.TrackEvent("SAVE Last Email");
            
            LastEmail data = new LastEmail
            {
                Id = 1,
                Email = email
            };
            App.Database.SaveLastEmail(data);
        }

        public static long GetAccounts()
        {
            string result = App.GetAPI.GetHIBP("https://haveibeenpwned.com/api/v2/breaches");
            long count = 0;
            if (result != null && result.Length > 0)
            {
                JArray job = (JArray)JsonConvert.DeserializeObject(result);

                foreach (var item in job.Children())
                {
                    count = count + (long)item["PwnCount"];
                }
                Analytics.TrackEvent("Get Number of Accounts");
            }
            return count;
        }
        public static int GetBreach()
        {
            string result = App.GetAPI.GetHIBP("https://haveibeenpwned.com/api/v2/breaches");
            int count = 0;
            if (result != null && result.Length > 0)
            {
                JArray job = (JArray)JsonConvert.DeserializeObject(result);

                foreach (var item in job.Children())
                {
                    count++;
                }
                Analytics.TrackEvent("Get Number of Breaches");
            }
            return count;
        }
    }
}