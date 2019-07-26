// <copyright file="UWPGetAPI.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Polly;
using PwnedPasswords.Interfaces;
using Xamarin.Forms;

namespace PwnedPasswords.UWP
{
    /// <summary>
    /// UWPGetAPI.
    /// </summary>
    public class UWPGetAPI : IAPI
    {
        /// <summary>
        /// GetAsyncAPI.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>HttpResponseMessage.</returns>
        public HttpResponseMessage GetAsyncAPI(string url)
        {
            HttpClient client = new HttpClient();
            DependencyService.Get<ILog>().SendTracking("GetAsyncAPI " + url);
            var response = client.GetAsync(url).Result;
            return response;
        }

        /// <summary>
        /// GetHIBP.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>string.</returns>
        public string GetHIBP(string url)
        {
            try
            {
                HttpResponseMessage response = this.GetAsyncAPI(url);
                return response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception e)
            {
                DependencyService.Get<ILog>().SendTracking("Error");
                DependencyService.Get<ILog>().SendTracking(e.Message, e);
                DependencyService.Get<ILog>().SendTracking("Details");
                return null;
            }
        }
    }
}