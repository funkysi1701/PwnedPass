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
        /// GetHIBP.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>Task string.</returns>
        string GetHIBP(string url);

        /// <summary>
        /// GetAsyncAPI.
        /// </summary>
        /// <param name="url">url.</param>
        /// <returns>Task HttpResponseMessage.</returns>
        HttpResponseMessage GetAsyncAPI(string url);
    }
}