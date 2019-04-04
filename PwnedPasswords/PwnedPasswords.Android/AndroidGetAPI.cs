// <copyright file="AndroidGetAPI.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using ModernHttpClient;

namespace PwnedPasswords.Droid
{
    /// <summary>
    /// AndroidGetAPI
    /// </summary>
    public class AndroidGetAPI : IAPI
    {
        /// <inheritdoc/>
        public bool GetAPI(string url)
        {
            HttpResponseMessage response = this.GetAsyncAPI(url);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// GetAsyncAPI
        /// </summary>
        /// <param name="url">url goes here</param>
        /// <returns>HttpResponseMessage</returns>
        public HttpResponseMessage GetAsyncAPI(string url)
        {
            HttpClient client = new HttpClient(new NativeMessageHandler());
            return client.GetAsync(url).Result;
        }

        /// <summary>
        /// GetHIBP
        /// </summary>
        /// <param name="url">url goes here</param>
        /// <returns>string</returns>
        public string GetHIBP(string url)
        {
            try
            {
                HttpResponseMessage response = this.GetAsyncAPI(url);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Crashes.TrackError(e);
                Analytics.TrackEvent(e.Message);
                var details = new Dictionary<string, string>
                {
                        { "StackTrace", e.StackTrace },
                        { "Inner", e.InnerException.Message }
                };
                Analytics.TrackEvent("Details", details);
                return null;
            }
        }
    }
}