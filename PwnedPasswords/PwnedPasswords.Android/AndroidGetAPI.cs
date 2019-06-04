// <copyright file="AndroidGetAPI.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

namespace PwnedPasswords.Droid
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.AppCenter.Crashes;
    using Polly;
    using PwnedPasswords.Interfaces;
    using Xamarin.Forms;

    /// <summary>
    /// AndroidGetAPI.
    /// </summary>
    public class AndroidGetAPI : IAPI
    {
        /// <summary>
        /// GetAsyncAPI.
        /// </summary>
        /// <param name="url">url goes here.</param>
        /// <returns>HttpResponseMessage.</returns>
        public async Task<HttpResponseMessage> GetAsyncAPI(string url)
        {
            HttpClient client = new HttpClient();
            DependencyService.Get<ILog>().SendTracking("GetAsyncAPI " + url);
            try
            {
                return await client.GetAsync(url);
            }
            catch (Exception e)
            {
                DependencyService.Get<ILog>().SendTracking(e.Message, e);
                Crashes.TrackError(e);
                return new HttpResponseMessage();
            }
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
                DependencyService.Get<ILog>().SendTracking("Error");
                Crashes.TrackError(e);
                DependencyService.Get<ILog>().SendTracking(e.Message, e);
                DependencyService.Get<ILog>().SendTracking("Details");
                return null;
            }
        }
    }
}