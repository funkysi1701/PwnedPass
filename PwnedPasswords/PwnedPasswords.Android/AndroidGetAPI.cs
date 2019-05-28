// <copyright file="AndroidGetAPI.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

namespace PwnedPasswords.Droid
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using ModernHttpClient;

    /// <summary>
    /// AndroidGetAPI.
    /// </summary>
    public class AndroidGetAPI : IAPI
    {
        /// <inheritdoc/>
        public async Task<bool> GetAPI(string url)
        {
            HttpResponseMessage response = await this.GetAsyncAPI(url);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// GetAsyncAPI.
        /// </summary>
        /// <param name="url">url goes here.</param>
        /// <returns>HttpResponseMessage.</returns>
        public async Task<HttpResponseMessage> GetAsyncAPI(string url)
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            return await client.GetAsync(url);
        }

        /// <summary>
        /// GetHIBP.
        /// </summary>
        /// <param name="url">url goes here.</param>
        /// <returns>string.</returns>
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
                Crashes.TrackError(e);
                Analytics.TrackEvent(e.Message);
                var details = new Dictionary<string, string>
                {
                        { "StackTrace", e.StackTrace },
                        { "Inner", e.InnerException.Message },
                };
                Analytics.TrackEvent("Details", details);
                return null;
            }
        }
    }
}