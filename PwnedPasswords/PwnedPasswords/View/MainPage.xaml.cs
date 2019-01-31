using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PwnedPasswords.Interfaces;
using System;
using Xamarin.Forms;

namespace PwnedPasswords.View
{
    public partial class MainPage : ContentPage
    {
        private void AddContent()
        {
            PassStack.Children.Clear();
            stack = new StackLayout();
            Grid grid = new Grid();
            scroll.Content = stack;

            Button Pass = new Button
            {
                BackgroundColor = Color.Salmon,
                TextColor = Color.Black,
                
                CornerRadius = 100,
                
                Text = "password check",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            Pass.Clicked += PasswordCicked;
            Button Email = new Button
            {
                BackgroundColor = Color.LightBlue,
                TextColor = Color.Black,
                
                CornerRadius = 100,
                
                Text = "email check",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            Email.Clicked += HIBPClicked;
            Button breach = new Button
            {
                BackgroundColor = Color.LightGreen,
                TextColor = Color.Black,
                
                CornerRadius = 100,
                
                Text = "list of \ndata breaches",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            breach.Clicked += BreachesClicked;
            Button cal = new Button
            {
                BackgroundColor = Color.Yellow,
                TextColor = Color.Black,
                
                CornerRadius = 100,
                
                Text = "calendar of \nbreaches",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            cal.Clicked += CalClicked;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(Pass, 0, 0);
            grid.Children.Add(Email, 0, 1);
            grid.Children.Add(breach, 1, 0);
            grid.Children.Add(cal, 1, 1);
            grid.VerticalOptions = LayoutOptions.FillAndExpand;
            stack.Children.Add(grid);
        }
        public MainPage()
        {
            InitializeComponent();
            AddContent();

            DependencyService.Get<IFooter>().AddFooter(this, stack);
        }

        public bool p = false;
        StackLayout stack;
        private void PasswordCicked(object sender, EventArgs e)
        {
            SaveData();
            Analytics.TrackEvent("Password MenuItem");
            Navigation.PushAsync(new PasswordCheckPage());
        }

        private void RateClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Rate MenuItem");
            DependencyService.Get<IStore>().GetStore();
        }

        private void HIBPClicked(object sender, EventArgs e)
        {
            SaveData();
            Analytics.TrackEvent("Email MenuItem");
            Navigation.PushAsync(new EmailCheckPage());
        }

        private void BreachesClicked(object sender, EventArgs e)
        {
            SaveData();
            Analytics.TrackEvent("Breaches MenuItem");
            Navigation.PushAsync(new BreachesPage(0,false));
        }
        private void CalClicked(object sender, EventArgs e)
        {
            SaveData();
            Analytics.TrackEvent("Cal MenuItem");
            Navigation.PushAsync(new CalChartPage());
        }
        private void AboutClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("About MenuItem");
            Device.OpenUri(new Uri("https://haveibeenpwned.com/"));
        }

        private void FSiClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("FSi MenuItem");
            Device.OpenUri(new Uri("https://www.funkysi1701.com/pwned-pass/?pwnedpass"));
        }

        private void SaveData()
        {
            try
            {
                p = Cache.SaveData(p);
            }
            catch (Exception ex)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(ex.Message);
                Crashes.TrackError(ex);
            }
        }
    }
}