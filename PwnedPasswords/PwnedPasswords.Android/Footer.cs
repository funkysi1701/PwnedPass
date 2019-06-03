// <copyright file="Footer.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using Plugin.CurrentActivity;
using PwnedPasswords.Interfaces;
using PwnedPasswords.View;
using Xamarin.Forms;

[assembly: Dependency(typeof(PwnedPasswords.Droid.Footer))]

namespace PwnedPasswords.Droid
{
    /// <summary>
    /// Footer.
    /// </summary>
    public class Footer : IFooter
    {
        /// <summary>
        /// AddFooter.
        /// </summary>
        /// <param name="mainPage">mainPage.</param>
        /// <param name="stack">stack.</param>
        public void AddFooter(MainPage mainPage, StackLayout stack)
        {
            var context = CrossCurrentActivity.Current.Activity;
            var versioncode = new Label
            {
                Text = " Version: " + context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Small, mainPage),
            };
            var about = new Label
            {
                Text = "  ';** Pwned Pass created by Simon Foster.",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Small, mainPage),
            };
            stack.Children.Add(about);
            stack.Children.Add(versioncode);
        }
    }
}