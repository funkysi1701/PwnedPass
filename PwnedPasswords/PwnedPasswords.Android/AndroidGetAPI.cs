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
                var response = await Policy
.HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
.WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
{
DependencyService.Get<ILog>().SendTracking($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
})
.ExecuteAsync(() => client.GetAsync(url));
                return response;
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