using System;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json.Linq;
using PwnedPasswords.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PwnedPasswords.View
{
    /// <summary>
    /// EmailCheckPage
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailCheckPage : ContentPage
    {
        private readonly ViewModel.ViewModel vm;
        private Entry emailinput;
        private Button passButton;
        private Label totalBreaches;
        private Label totalAccounts;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailCheckPage"/> class.
        /// </summary>
        public EmailCheckPage()
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
                DependencyService.Get<ILog>().SendTracking("Error");
                DependencyService.Get<ILog>().SendTracking(e.Message, e);
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
            this.emailinput = new Entry { AutomationId = "password", Placeholder = "Your Email Address", IsPassword = false };
            this.passButton = new Button { BackgroundColor = Color.LightBlue, Text = "GO" };
            this.totalBreaches = new Label { Text = this.vm.Breach, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.totalAccounts = new Label { Text = this.vm.Accounts, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.emailinput.Completed += this.Passbutton;
            this.passButton.Clicked += this.Passbutton;

            this.vm.Pg.Setup(this.PassStack, height, width - 4);

            this.PassStack.Children.Add(this.emailinput, 0, 1);
            Grid.SetColumnSpan(this.emailinput, width - 2);

            this.PassStack.Children.Add(this.passButton, width - 2, 1);
            Grid.SetColumnSpan(this.passButton, 2);

            this.PassStack.Children.Add(this.totalBreaches, 0, 0);
            Grid.SetColumnSpan(this.totalBreaches, width - 4);

            this.PassStack.Children.Add(this.totalAccounts, width - 4, 0);
            Grid.SetColumnSpan(this.totalAccounts, 4);
            this.emailinput.Text = Cache.LoadLastEmail();
            DependencyService.Get<ILog>().SendTracking("Email Page Setup");
        }

        /// <summary>
        /// OnButtonClicked
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        public void OnButtonClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            this.Navigation.PushAsync(new BreachesPage(btn.AutomationId));
        }

        /// <summary>
        /// Passbutton.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void Passbutton(object sender, EventArgs e)
        {
            int count = 3;
            var email = this.emailinput.Text;
            this.ToolbarItems.Clear();
            this.InitializeComponent();
            if (email != null && email.Length > 0)
            {
                Cache.SaveLastEmail(email.Trim());

                var result = App.GetAPI.GetHIBP("https://pwnedpassapifsi.azurewebsites.net/api/HIBP/CheckEmail?email=" + email.Trim() + "&unverified=true");
                if (result.Contains("Request Blocked"))
                {
                    this.PassStack.Children.Clear();
                    int width = 7;
                    int height = 7;
                    this.Setup(height, width);
                    var info = new Label { AutomationId = "goodbad", Text = "It was not possible to check this email at this time.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    this.PassStack.Children.Add(info);
                    this.PassStack.Children.Add(info, 0, 2);
                    Grid.SetColumnSpan(info, width);
                }
                else if (result != null && result.Length > 0)
                {
                    this.PassStack.Children.Clear();
                    int width = 7;
                    int height = 7;
                    this.Setup(height, width);
                    JArray job = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    var numberOfBreaches = job.Count;
                    var info = new Label { AutomationId = "goodbad", Text = "Your email address has been included in the following " + numberOfBreaches.ToString() + " data breaches:", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };

                    this.PassStack.Children.Add(info, 0, 2);
                    Grid.SetColumnSpan(info, width);

                    foreach (var item in job.Children())
                    {
                        DataBreach db = new DataBreach
                        {
                            Name = item["Name"].ToString(),
                            Title = item["Title"].ToString(),
                        };
                        var breachbutt = new Button { AutomationId = db.Name, BackgroundColor = Color.LightBlue, Text = db.Title, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                        breachbutt.Clicked += this.OnButtonClicked;
                        this.PassStack.Children.Add(breachbutt, 0, count);
                        Grid.SetColumnSpan(breachbutt, width);
                        count++;
                    }
                }
                else
                {
                    this.PassStack.Children.Clear();
                    int width = 7;
                    int height = 7;
                    this.Setup(height, width);
                    var info = new Label { AutomationId = "goodbad", Text = "Your email address has not been included in any data breach.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    this.PassStack.Children.Add(info);
                    this.PassStack.Children.Add(info, 0, 2);
                    Grid.SetColumnSpan(info, width);
                }

                DependencyService.Get<ILog>().SendTracking("HIBP");
            }
        }

        private void AboutClicked(object sender, EventArgs e)
        {
            DependencyService.Get<ILog>().SendTracking("About MenuItem");
            Device.OpenUri(new Uri("https://haveibeenpwned.com/"));
        }

        private void FSiClicked(object sender, EventArgs e)
        {
            DependencyService.Get<ILog>().SendTracking("FSi MenuItem");
            Device.OpenUri(new Uri("https://www.funkysi1701.com/pwned-pass/?pwnedpass"));
        }

        private void RateClicked(object sender, EventArgs e)
        {
            DependencyService.Get<ILog>().SendTracking("Rate MenuItem");
            DependencyService.Get<IStore>().GetStore();
        }
    }
}