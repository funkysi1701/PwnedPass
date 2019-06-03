// <copyright file="MainActivity.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

namespace PwnedPasswords.Droid
{
    using Android.App;
    using Android.Content.PM;
    using Android.OS;

    /// <summary>
    /// MainActivity.
    /// </summary>
    [Activity(Label = "Pwned Pass", Icon = "@drawable/icon", Theme="@style/MainTheme", MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        /// <summary>
        /// OnCreate.
        /// </summary>
        /// <param name="bundle">bundle.</param>
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            App.InitHash(new AndroidGetHash());
            App.InitAPI(new AndroidGetAPI());
            this.LoadApplication(new App());
        }
    }
}