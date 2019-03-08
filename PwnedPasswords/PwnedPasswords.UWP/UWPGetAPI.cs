using Microsoft.AppCenter.Analytics;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace PwnedPasswords.UWP
{
    public class UWPGetAPI : IAPI
    {
        public bool GetAPI(string url)
        {
            HttpResponseMessage response = GetAsyncAPI(url);
            return response.IsSuccessStatusCode;
        }

        public HttpResponseMessage GetAsyncAPI(string url)
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            client.DefaultRequestHeaders.Add("User-Agent", "Pwned Pass");
            return client.GetAsync(url).Result;
        }

        public string GetHIBP(string url)
        {
            try
            {
                this.GetMyApi();
                HttpResponseMessage response = this.GetAsyncAPI(url);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Analytics.TrackEvent("Details", new Dictionary<string, string> {
                        { "StackTrace", e.StackTrace },
                        { "Inner", e.InnerException.Message}
                    });
                return null;
            }
        }

        /// <summary>
        /// Call My new API
        /// </summary>
        public void GetMyApi()
        {
            this.GetAsyncAPI("https://pwnedpassapi.azurewebsites.net/api/values");
        }
    }
}
