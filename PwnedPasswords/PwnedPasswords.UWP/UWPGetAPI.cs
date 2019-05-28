using Microsoft.AppCenter.Analytics;
using ModernHttpClient;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PwnedPasswords.UWP
{
    /// <summary>
    /// UWPGetAPI
    /// </summary>
    public class UWPGetAPI : IAPI
    {
        /// <summary>
        /// GetAPI
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>true/false</returns>
        public async Task<bool> GetAPI(string url)
        {
            HttpResponseMessage response = await this.GetAsyncAPI(url);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// GetAsyncAPI
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessage> GetAsyncAPI(string url)
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            return await client.GetAsync(url);
        }

        /// <summary>
        /// GetHIBP
        /// </summary>
        /// <param name="url">url</param>
        /// <returns>string</returns>
        public async Task<string> GetHIBP(string url)
        {
            try
            {
                HttpResponseMessage response = await this.GetAsyncAPI(url);
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Analytics.TrackEvent("Details", new Dictionary<string, string>
                {
                        { "StackTrace", e.StackTrace },
                        { "Inner", e.InnerException.Message },
                });
                return null;
            }
        }
    }
}
