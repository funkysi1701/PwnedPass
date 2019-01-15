using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using System;
using Xamarin.Forms;

namespace PwnedPasswords
{
    public class Page
    {
        public void Setup(Grid PassStack, int height, int width)
        {
            for (int i = 0; i < height; i++)
            {
                PassStack.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            }
            for (int i = 0; i < width; i++)
            {
                PassStack.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            }
        }

        public string GetAccounts()
        {
            Analytics.TrackEvent("Get Number of Accounts from Cache");
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
            return string.Format("{0:n0}", count) + " pwned accounts";
        }

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
            return count.ToString() + " pwned websites";
        }
    }
}
