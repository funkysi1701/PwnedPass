using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Text.RegularExpressions;

namespace PwnedPasswords
{
    /// <summary>
    /// Caches Saved data
    /// </summary>
    public class Cache
    {
        /// <summary>
        /// Save Data
        /// </summary>
        /// <param name="runonce">bool to indicate if run before</param>
        /// <returns>true/false</returns>
        public static bool SaveData(bool runonce)
        {
            if (!runonce)
            {
                try
                {
                    HIBP data = new HIBP();
                    long acc = GetAccounts();
                    int bre = GetBreach();
                    if (acc > 1)
                    {
                        data.TotalAccounts = acc;
                    }

                    if (bre > 1)
                    {
                        data.TotalBreaches = bre;
                    }

                    Analytics.TrackEvent("SAVE DB");
                    if (acc > 1 && bre > 1)
                    {
                        data.Id = 1;
                        App.Database.SaveHIBP(data);
                        App.Database.EmptyDataBreach();
                        string result = App.GetAPI.GetHIBP("https://pwnedpassapifsi.azurewebsites.net/api/HIBP/GetBreaches");
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

            return true;
        }

        /// <summary>
        /// Load last email
        /// </summary>
        /// <returns>string</returns>
        public static string LoadLastEmail()
        {
            Analytics.TrackEvent("LOAD Last Email");
            var table = App.Database.GetLastEmail();
            string email = string.Empty;
            foreach (var s in table)
            {
                email = s.Email;
            }

            return email;
        }

        /// <summary>
        /// Save last email
        /// </summary>
        /// <param name="email">an email address</param>
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

        /// <summary>
        /// Get number of accounts
        /// </summary>
        /// <returns>long</returns>
        public static long GetAccounts()
        {
            string result = App.GetAPI.GetHIBP("https://pwnedpassapifsi.azurewebsites.net/api/HIBP/GetBreaches");
            long count = 0;
            if (result != null && result.Length > 0)
            {
                JArray job = (JArray)JsonConvert.DeserializeObject(result);

                foreach (var item in job.Children())
                {
                    count += (long)item["PwnCount"];
                }

                Analytics.TrackEvent("Get Number of Accounts");
            }

            return count;
        }

        /// <summary>
        /// Get number of breaches
        /// </summary>
        /// <returns>int</returns>
        public static int GetBreach()
        {
            string result = App.GetAPI.GetHIBP("https://pwnedpassapifsi.azurewebsites.net/api/HIBP/GetBreaches");
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