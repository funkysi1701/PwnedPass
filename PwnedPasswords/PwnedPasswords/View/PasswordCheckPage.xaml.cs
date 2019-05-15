using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PwnedPasswords.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PwnedPasswords.View
{
    /// <summary>
    /// PasswordCheckPage
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordCheckPage : ContentPage
    {
        private readonly ViewModel.ViewModel vm;

        /// <summary>
        /// password
        /// </summary>
        private Entry password;

        /// <summary>
        /// passButton
        /// </summary>
        private Button passButton;

        /// <summary>
        /// TotalBreaches
        /// </summary>
        private Label totalBreaches;

        /// <summary>
        /// TotalAccounts
        /// </summary>
        private Label totalAccounts;

        /// <summary>
        /// Initializes a new instance of the <see cref="PasswordCheckPage"/> class.
        /// </summary>
        public PasswordCheckPage()
        {
            try
            {
                this.InitializeComponent();
                this.BindingContext = this.vm = new ViewModel.ViewModel();
                this.PassStack.Children.Clear();
                this.Setup(7, 7);
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Crashes.TrackError(e);
            }
        }

        /// <summary>
        /// Setup
        /// </summary>
        /// <param name="height">height</param>
        /// <param name="width">width</param>
        public void Setup(int height, int width)
        {
            this.password = new Entry { AutomationId = "password", Placeholder = "Pwned Password", IsPassword = true };
            this.passButton = new Button { BackgroundColor = Color.LightBlue, Text = "GO" };
            this.totalBreaches = new Label { Text = this.vm.Breach, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.totalAccounts = new Label { Text = this.vm.Accounts, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.password.Completed += this.Passbutton;
            this.passButton.Clicked += this.Passbutton;
            int halfwidth = width / 2;

            this.vm.Pg.Setup(this.PassStack, height, width - 4);

            this.PassStack.Children.Add(this.password, 0, 1);
            Grid.SetColumnSpan(this.password, width - 2);

            this.PassStack.Children.Add(this.passButton, width - 2, 1);
            Grid.SetColumnSpan(this.passButton, 2);

            this.PassStack.Children.Add(this.totalBreaches, 0, 0);
            Grid.SetColumnSpan(this.totalBreaches, width - 4);

            this.PassStack.Children.Add(this.totalAccounts, width - 4, 0);
            Grid.SetColumnSpan(this.totalAccounts, 4);
            Analytics.TrackEvent("Pass Page Setup");
        }

        private void Passbutton(object sender, EventArgs e)
        {
            if (this.password.Text != null && this.password.Text.Length > 0)
            {
                int width = 7;
                int height = 7;
                this.ToolbarItems.Clear();
                this.InitializeComponent();
                var text = this.password.Text;
                if (text == null)
                {
                    text = string.Empty;
                }

                Analytics.TrackEvent("Hash");
                string hash = App.GetHash.GetHash(text);
                this.PassStack.Children.Clear();
                this.Setup(height, width);
                var info = new Button { AutomationId = "goodbad", FontSize = Device.GetNamedSize(NamedSize.Large, this) };
                string output = App.GetAPI.GetHIBP("https://pwnedpassapifsi.azurewebsites.net/api/HIBP/CheckPasswords?hash=" + hash.Substring(0, 5));
                string count = this.GetCount(output, hash);
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

                this.PassStack.Children.Add(info, 0, 2);
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
                if (item.Substring(0, 35) == hash.Substring(5))
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