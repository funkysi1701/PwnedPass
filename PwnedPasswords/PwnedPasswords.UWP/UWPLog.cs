// <copyright file="UWPLog.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
using PwnedPasswords.Interfaces;
using Windows.ApplicationModel;
using Xamarin.Forms;

[assembly: Dependency(typeof(PwnedPasswords.UWP.UWPLog))]

namespace PwnedPasswords.UWP
{
    /// <summary>
    /// UWPLog.
    /// </summary>
    public class UWPLog : ILog
    {
        /// <summary>
        /// SendTracking.
        /// </summary>
        /// <param name="message">message.</param>
        public void SendTracking(string message)
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            var details = new Dictionary<string, string>
                {
                        { "Build", version.Build.ToString() },
                        { "Major", version.Major.ToString() },
                        { "Minor", version.Minor.ToString() },
                        { "Revision", version.Revision.ToString() },
                };
            Analytics.TrackEvent(message, details);
        }

        /// <summary>
        /// SendTracking.
        /// </summary>
        /// <param name="message">message.</param>
        /// <param name="e">Exception.</param>
        public void SendTracking(string message, Exception e)
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            var details = new Dictionary<string, string>
                {
                        { "Build", version.Build.ToString() },
                        { "Major", version.Major.ToString() },
                        { "Minor", version.Minor.ToString() },
                        { "Revision", version.Revision.ToString() },
                        { "StackTrace", e.StackTrace },
                        { "Revision", e.InnerException.Message },
                };
            Analytics.TrackEvent(message, details);
        }
    }
}
