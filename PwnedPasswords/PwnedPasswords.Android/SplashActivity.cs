// <copyright file="SplashActivity.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using Android.App;
using Android.Content;
using Android.Support.V7.App;

namespace PwnedPasswords.Droid
{
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
            var intent = new Intent(this, typeof(MainActivity));
            if (this.Intent.Extras != null)
            {
                intent.PutExtras(this.Intent.Extras); // copy push info from splash to main
            }

            this.StartActivity(intent);
        }
    }
}