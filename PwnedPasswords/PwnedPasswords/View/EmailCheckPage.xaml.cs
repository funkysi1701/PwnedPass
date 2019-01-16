using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json.Linq;
using PwnedPasswords.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PwnedPasswords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailCheckPage : ContentPage
    {
        public Entry emailinput;
        public Button passButton;
        public Label TotalBreaches;
        public Label TotalAccounts;
        public EmailCheckPage()
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
            emailinput = new Entry { AutomationId = "password", Placeholder = "Your Email Address", IsPassword = false };
            passButton = new Button { BackgroundColor = Color.LightBlue, Text = "GO" };
            Page pg = new Page();
            string breach = pg.GetBreach();
            string accounts = pg.GetAccounts();
            TotalBreaches = new Label { Text = breach, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            TotalAccounts = new Label { Text = accounts, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            emailinput.Completed += Passbutton;
            passButton.Clicked += Passbutton;
            int halfwidth = width / 2;

            pg.Setup(PassStack, height, width - 4);

            PassStack.Children.Add(emailinput, 0, 1);
            Grid.SetColumnSpan(emailinput, width - 2);

            PassStack.Children.Add(passButton, width - 2, 1);
            Grid.SetColumnSpan(passButton, 2);

            PassStack.Children.Add(TotalBreaches, 0, 0);
            Grid.SetColumnSpan(TotalBreaches, width - 4);

            PassStack.Children.Add(TotalAccounts, width - 4, 0);
            Grid.SetColumnSpan(TotalAccounts, 4);
            emailinput.Text = Cache.LoadLastEmail();
            Analytics.TrackEvent("Email Page Setup");
        }

        private void Passbutton(object sender, EventArgs e)
        {
            int count = 3;
            var email = emailinput.Text;
            this.ToolbarItems.Clear();
            InitializeComponent();
            if (email != null && email.Length > 0)
            {
                Cache.SaveLastEmail(email.Trim());
                string result = App.GetAPI.GetHIBP("https://haveibeenpwned.com/api/v2/breachedaccount/" + email.Trim()+ "?includeUnverified=true");
                if(result.Contains("Request Blocked"))
                {
                    PassStack.Children.Clear();
                    int width = 7;
                    int height = 7;
                    Setup(height, width);
                    var info = new Label { AutomationId = "goodbad", Text = "It was not possible to check this email at this time.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    PassStack.Children.Add(info);
                    PassStack.Children.Add(info, 0, 2);
                    Grid.SetColumnSpan(info, width);
                }
                else if (result != null && result.Length > 0)
                {
                    PassStack.Children.Clear();
                    int width = 7;
                    int height = 7;
                    Setup(height, width);
                    var info = new Label { AutomationId = "goodbad", Text = "Your email address has been included data breaches.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };

                    PassStack.Children.Add(info, 0, 2);
                    Grid.SetColumnSpan(info, width);
                    JArray job = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    foreach (var item in job.Children())
                    {
                        DataBreach db = new DataBreach
                        {
                            Name = item["Name"].ToString(),
                            Title = item["Title"].ToString()
                        };
                        var breachbutt = new Button { AutomationId = db.Name, BackgroundColor = Color.LightBlue, Text = db.Title, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                        breachbutt.Clicked += OnButtonClicked;
                        PassStack.Children.Add(breachbutt, 0, count);
                        Grid.SetColumnSpan(breachbutt, width);
                        count++;
                    }
                }
                else
                {
                    PassStack.Children.Clear();
                    int width = 7;
                    int height = 7;
                    Setup(height, width);
                    var info = new Label { AutomationId = "goodbad", Text = "Your email address has not been included in any data breach.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    PassStack.Children.Add(info);
                    PassStack.Children.Add(info, 0, 2);
                    Grid.SetColumnSpan(info, width);
                }
                Analytics.TrackEvent("HIBP");
            }
        }

        public void OnButtonClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            Navigation.PushAsync(new BreachesPage(btn.AutomationId));
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