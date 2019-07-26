// <copyright file="ILog.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using System;

namespace PwnedPasswords.Interfaces
{
    /// <summary>
    /// ILog.
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// SendTracking.
        /// </summary>
        /// <param name="message">message.</param>
        void SendTracking(string message);

        /// <summary>
        /// SendTracking.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="e">e.</param>
        void SendTracking(string message, Exception e);
    }
}