// <copyright file="IAPI.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

namespace PwnedPasswords
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// IAPI.
    /// </summary>
    public interface IAPI
    {
        /// <summary>
        /// GetAPI.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>Task bool.</returns>
        Task<bool> GetAPI(string url);

        /// <summary>
        /// GetHIBP.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>Task string.</returns>
        Task<string> GetHIBP(string url);

        /// <summary>
        /// GetAsyncAPI.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>Task HttpResponseMessage.</returns>
        Task<HttpResponseMessage> GetAsyncAPI(string url);
    }
}
