// <copyright file="UWPGetAPI.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

namespace PwnedPasswords.UWP
{
    using Polly;
    using PwnedPasswords.Interfaces;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    /// <summary>
    /// UWPGetAPI.
    /// </summary>
    public class UWPGetAPI : IAPI
    {
        /// <summary>
        /// GetAPI
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>true/false.</returns>
        public async Task<bool> GetAPI(string url)
        {
            HttpResponseMessage response = await this.GetAsyncAPI(url);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// GetAsyncAPI.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>HttpResponseMessage.</returns>
        public async Task<HttpResponseMessage> GetAsyncAPI(string url)
        {
            HttpClient client = new HttpClient();
            DependencyService.Get<ILog>().SendTracking("GetAsyncAPI " + url);
            var response = await Policy
        .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
        .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
        {
            DependencyService.Get<ILog>().SendTracking($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
        })
        .ExecuteAsync(() => client.GetAsync(url));
            return response;
        }

        /// <summary>
        /// GetHIBP.
        /// </summary>
        /// <param name="url">url.</param>
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
                DependencyService.Get<ILog>().SendTracking(e.Message, e);
                DependencyService.Get<ILog>().SendTracking("Details");
                return null;
            }
        }
    }
}
