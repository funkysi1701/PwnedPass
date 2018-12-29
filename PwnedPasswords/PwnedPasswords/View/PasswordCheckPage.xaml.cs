using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PwnedPasswords.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PwnedPasswords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordCheckPage : ContentPage
    {
        public Entry password;
        public Button passButton;
        public Label TotalBreaches;
        public Label TotalAccounts;
        public PasswordCheckPage()
        {
            try
            {
                InitializeComponent();
                PassStack.Children.Clear();
                Setup(7, 7);
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Crashes.TrackError(e);
            }
        }

        public void Setup(int height, int width)
        {
            password = new Entry { AutomationId = "password", Placeholder = "Pwned Password", IsPassword = true };
            passButton = new Button { BackgroundColor = Color.LightBlue, Text = "GO" };
            Page pg = new Page();
            string breach = pg.GetBreach();
            string accounts = pg.GetAccounts();
            TotalBreaches = new Label { Text = breach, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            TotalAccounts = new Label { Text = accounts, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            password.Completed += Passbutton;
            passButton.Clicked += Passbutton;
            int halfwidth = width / 2;

            pg.Setup(PassStack, height, width - 4);

            PassStack.Children.Add(password, 0, 1);
            Grid.SetColumnSpan(password, width - 2);

            PassStack.Children.Add(passButton, width - 2, 1);
            Grid.SetColumnSpan(passButton, 2);

            PassStack.Children.Add(TotalBreaches, 0, 0);
            Grid.SetColumnSpan(TotalBreaches, width - 4);

            PassStack.Children.Add(TotalAccounts, width - 4, 0);
            Grid.SetColumnSpan(TotalAccounts, 4);
            Analytics.TrackEvent("Pass Page Setup");
        }

        private void Passbutton(object sender, EventArgs e)
        {
            if (password.Text != null && password.Text.Length > 0)
            {
                int width = 7;
                int height = 7;
                this.ToolbarItems.Clear();
                InitializeComponent();
                var text = password.Text;
                if (text == null) { text = ""; }
                Analytics.TrackEvent("Hash");
                string hash = PwnedPasswords.App.GetHash.GetHash(text);
                PassStack.Children.Clear();
                Setup(height, width);
                var info = new Button { AutomationId = "goodbad", FontSize = Device.GetNamedSize(NamedSize.Large, this) };
                string output = App.GetAPI.GetHIBP("https://api.pwnedpasswords.com/range/" + hash.Substring(0, 5));
                string count = GetCount(output, hash);
                if (count == "0")
                {
                    info.Text = "This password has not been indexed by haveibeenpwned.com";
                    info.BackgroundColor = Color.Green;
                    info.TextColor = Color.White;
                    info.Margin = 5;
                    info.CornerRadius = 100;
                    info.FontSize = 20;
                    info.FontAttributes = FontAttributes.Bold;
                    Analytics.TrackEvent("Pass False");
                }
                else
                {
                    info.Text = "This password has previously appeared in a data breach " + count + " times and should never be used. ";
                    info.BackgroundColor = Color.Red;
                    info.TextColor = Color.White;
                    info.Margin = 5;
                    info.CornerRadius = 100;
                    info.FontSize = 20;
                    info.FontAttributes = FontAttributes.Bold;
                    Analytics.TrackEvent("Pass True");
                }

                PassStack.Children.Add(info, 0, 2);
                Grid.SetColumnSpan(info, width);

                Analytics.TrackEvent("Pass APICall");
            }
        }

        private string GetCount(string output, string hash)
        {
            string[] lines = output.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            string count = "0";
            foreach (var item in lines)
            {
                if(item.Substring(0,35) == hash.Substring(5))
                {
                    count = item.Substring(36);
                }
            }
            return count;
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

        private void RateClicked(object sender, EventArgs e)
        {
            Analytics.TrackEvent("Rate MenuItem");
            DependencyService.Get<IStore>().GetStore();
        }
    }
}