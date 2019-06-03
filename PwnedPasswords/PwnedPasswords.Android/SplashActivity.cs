// <copyright file="SplashActivity.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

namespace PwnedPasswords.Droid
{
    using Android.App;
    using Android.Support.V7.App;

    /// <summary>
    /// SplashActivity.
    /// </summary>
    [Activity(Label = "Pwned Pass", Icon = "@drawable/icon", Theme = "@style/splashscreen", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        /// <summary>
        /// OnResume.
        /// </summary>
        protected override void OnResume()
        {
            base.OnResume();
            this.StartActivity(typeof(MainActivity));
        }
    }
}