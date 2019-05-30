using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PwnedPasswords.Interfaces;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PwnedPasswords.View
{
    /// <summary>
    /// MainPage
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private bool saveFirst = false;
        private StackLayout stack;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            this.AddContent();

            DependencyService.Get<IFooter>().AddFooter(this, this.stack);
        }

        private void AddContent()
        {
            this.PassStack.Children.Clear();
            this.stack = new StackLayout();
            Grid grid = new Grid();
            this.scroll.Content = this.stack;

            Button pass = new Button
            {
                BackgroundColor = Color.Salmon,
                TextColor = Color.Black,
                CornerRadius = 100,
                Text = "password check",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            pass.Clicked += this.PasswordCicked;
            Button email = new Button
            {
                BackgroundColor = Color.LightBlue,
                TextColor = Color.Black,
                CornerRadius = 100,
                Text = "email check",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            email.Clicked += this.HIBPClicked;
            Button breach = new Button
            {
                BackgroundColor = Color.LightGreen,
                TextColor = Color.Black,
                CornerRadius = 100,
                Text = "list of \ndata breaches",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            breach.Clicked += this.BreachesClicked;
            Button cal = new Button
            {
                BackgroundColor = Color.Yellow,
                TextColor = Color.Black,
                CornerRadius = 100,
                Text = "calendar of \nbreaches",
                FontSize = 25,
                FontAttributes = FontAttributes.Bold
            };
            cal.Clicked += this.CalClicked;
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            grid.Children.Add(pass, 0, 0);
            grid.Children.Add(email, 0, 1);
            grid.Children.Add(breach, 1, 0);
            grid.Children.Add(cal, 1, 1);
            grid.VerticalOptions = LayoutOptions.FillAndExpand;
            this.stack.Children.Add(grid);
        }

        private void PasswordCicked(object sender, EventArgs e)
        {
            this.SaveData();
            Analytics.TrackEvent("Password MenuItem");
            this.Navigation.PushAsync(new PasswordCheckPage());
        }

        private void RateClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Rate MenuItem");
            DependencyService.Get<IStore>().GetStore();
        }

        private void HIBPClicked(object sender, EventArgs e)
        {
            this.SaveData();
            Analytics.TrackEvent("Email MenuItem");
            this.Navigation.PushAsync(new EmailCheckPage());
        }

        private void BreachesClicked(object sender, EventArgs e)
        {
            this.SaveData();
            Analytics.TrackEvent("Breaches MenuItem");
            this.Navigation.PushAsync(new BreachesPage(0, false));
        }

        private void CalClicked(object sender, EventArgs e)
        {
            this.SaveData();
            Analytics.TrackEvent("Cal MenuItem");
            this.Navigation.PushAsync(new CalChartPage());
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

        private async Task SaveData()
        {
            try
            {
                this.saveFirst = await Cache.SaveData(this.saveFirst);
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