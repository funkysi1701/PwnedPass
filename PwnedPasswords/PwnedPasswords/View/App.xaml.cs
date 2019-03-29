using Microsoft.AppCenter;
using Microsoft.AppCenter.Push;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Analytics;
using Xamarin.Forms;

namespace PwnedPasswords
{
    public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new NavigationPage(new View.MainPage());
		}

        static Database database;

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

        protected override void OnStart ()
		{
            AppCenter.Start("uwp=f497a9fd-3c8b-4072-87ea-2b6e8d057a52;" + "android=29b4ff89-6554-4d25-bb78-93cd14a3b280;", typeof(Analytics), typeof(Crashes), typeof(Push));
        }

        public static IHash GetHash { get; private set; }
        public static IAPI GetAPI { get; private set; }
        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

        public static void InitHash(IHash HashImplementation)
        {
            App.GetHash = HashImplementation;
        }

        public static void InitAPI(IAPI APIImplementation)
        {
            App.GetAPI = APIImplementation;
        }
        protected override void OnResume ()
		{
			// Handle when your app resumes
		}
    }
}
