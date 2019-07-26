// <copyright file="App.xaml.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Data;
using Microsoft.AppCenter.Push;
using Xamarin.Forms;

namespace PwnedPasswords
{
    /// <summary>
    /// App.
    /// </summary>
    public partial class App : Application
    {
        private static Database database;

        /// <summary>
        /// Initializes a new instance of the <see cref="App"/> class.
        /// </summary>
        public App()
        {
            this.InitializeComponent();

            this.MainPage = new NavigationPage(new View.MainPage());
        }

        /// <summary>
        /// Gets Hash.
        /// </summary>
        public static IHash GetHash { get; private set; }

        /// <summary>
        /// Gets API.
        /// </summary>
        public static IAPI GetAPI { get; private set; }

        /// <summary>
        /// Gets Database.
        /// </summary>
        public static Database Database
        {
            get
            {
                if (database == null)
                {
                    database = new Database();
                }

                return database;
            }
        }

        /// <summary>
        /// InitHash.
        /// </summary>
        /// <param name="hashImplementation">hashImplementation.</param>
        public static void InitHash(IHash hashImplementation)
        {
            App.GetHash = hashImplementation;
        }

        /// <summary>
        /// InitAPI.
        /// </summary>
        /// <param name="apiImplementation">apiImplementation.</param>
        public static void InitAPI(IAPI apiImplementation)
        {
            App.GetAPI = apiImplementation;
        }

        /// <summary>
        /// OnStart.
        /// </summary>
        protected override void OnStart()
        {
            AppCenter.Start("uwp=f497a9fd-3c8b-4072-87ea-2b6e8d057a52;" + "android=29b4ff89-6554-4d25-bb78-93cd14a3b280;", typeof(Analytics), typeof(Crashes), typeof(Push), typeof(Data));
        }

        /// <summary>
        /// OnSleep.
        /// </summary>
        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        /// <summary>
        /// OnResume.
        /// </summary>
        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}