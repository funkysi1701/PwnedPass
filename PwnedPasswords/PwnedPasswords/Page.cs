using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;

namespace PwnedPasswords
{
    /// <summary>
    /// Page
    /// </summary>
    public class Page
    {
        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="passStack">PassStack</param>
        /// <param name="height">height</param>
        /// <param name="width">width</param>
        public void Setup(Grid passStack, int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                passStack.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }

            for (int i = 0; i < width; i++)
            {
                passStack.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }

        /// <summary>
        /// GetAccountsRaw
        /// </summary>
        /// <returns>long</returns>
        public long GetAccountsRaw()
        {
            long count = 0;
            try
            {
                var table = App.Database.GetHIBP();

                foreach (var s in table)
                {
                    count = s.TotalAccounts;
                }
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Crashes.TrackError(e);
            }

            return count;
        }

        /// <summary>
        /// GetAccounts
        /// </summary>
        /// <returns>string</returns>
        public string GetAccounts()
        {
            Analytics.TrackEvent("Get Number of Accounts from Cache");
            long count = this.GetAccountsRaw();
            return string.Format("{0:n0}", count) + " pwned accounts";
        }

        /// <summary>
        /// GetBreach
        /// </summary>
        /// <returns>string</returns>
        public string GetBreach()
        {
            Analytics.TrackEvent("Get Number of Breaches from Cache");
            int count = 0;
            try
            {
                var table = App.Database.GetHIBP();

                foreach (var s in table)
                {
                    count = s.TotalBreaches;
                }
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Crashes.TrackError(e);
            }

            return count.ToString() + " data breaches";
        }
    }
}
