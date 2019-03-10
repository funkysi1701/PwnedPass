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
        private readonly StackLayout stack;
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
                this.stack = new StackLayout();
                this.scroll.Content = this.stack;
                this.Setup();
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
        public void Setup()
        {
            this.emailinput = new Entry { AutomationId = "password", Placeholder = "Your Email Address", IsPassword = false };
            this.passButton = new Button { BackgroundColor = Color.LightBlue, Text = "GO" };
            this.totalBreaches = new Label { Text = this.vm.Breach, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.totalAccounts = new Label { Text = this.vm.Accounts, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.emailinput.Completed += this.Passbutton;
            this.passButton.Clicked += this.Passbutton;

            this.stack.Children.Add(this.emailinput);
            this.stack.Children.Add(this.passButton);
            this.stack.Children.Add(this.totalBreaches);
            this.stack.Children.Add(this.totalAccounts);
            this.emailinput.Text = Cache.LoadLastEmail();
            Analytics.TrackEvent("Email Page Setup");
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
        /// Passbutton
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Passbutton(object sender, EventArgs e)
        {
            int count = 3;
            var email = this.emailinput.Text;

            if (email != null && email.Length > 0)
            {
                Cache.SaveLastEmail(email.Trim());
                string result = App.GetAPI.GetHIBP("https://haveibeenpwned.com/api/v2/breachedaccount/" + email.Trim() + "?includeUnverified=true");
                if (result.Contains("invalid email"))
                {
                    var info = new Label { AutomationId = "goodbad", Text = "This is not a valid email address", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    this.stack.Children.Add(info);
                }
                else if (result.Contains("Request Blocked"))
                {
                    var info = new Label { AutomationId = "goodbad", Text = "It was not possible to check this email at this time.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    this.PassStack.Children.Add(info);
                }
                else if (result != null && result.Length > 0)
                {
                    JArray job = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
                    var numberOfBreaches = job.Count;
                    var info = new Label { AutomationId = "goodbad", Text = "A breach is an incident where data has been unintentionally exposed to the public. Your email address has been included in the following " + numberOfBreaches.ToString() + " data breaches:", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };

                    this.PassStack.Children.Add(info);

                    foreach (var item in job.Children())
                    {
                        DataBreach db = new DataBreach
                        {
                            Name = item["Name"].ToString(),
                            Title = item["Title"].ToString()
                        };
                        var breachbutt = new Button { AutomationId = db.Name, BackgroundColor = Color.LightBlue, Text = db.Title, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                        breachbutt.Clicked += this.OnButtonClicked;
                        this.stack.Children.Add(breachbutt);
                        count++;
                    }

                    string pastes = App.GetAPI.GetHIBP("https://haveibeenpwned.com/api/v2/pasteaccount/" + email.Trim());
                    if (pastes.Contains("invalid email"))
                    {
                        info = new Label { AutomationId = "goodbad", Text = "This is not a valid email address", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                        this.stack.Children.Add(info);
                    }
                    else if (pastes.Contains("Request Blocked"))
                    {
                        info = new Label { AutomationId = "goodbad", Text = "It was not possible to check this email for pastes at this time.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                        this.stack.Children.Add(info);
                    }
                    else if (pastes != null && pastes.Length > 0)
                    {
                        job = (JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(pastes);
                        var numberOfPastes = job.Count;
                        info = new Label { AutomationId = "goodbad", Text = "A paste is information that has been published to a publicly facing website designed to share content and is often an early indicator of a data breach. Pastes are automatically imported and often removed shortly after having been posted. Your email address has been included in the following " + numberOfPastes.ToString() + " Pastes:", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };

                        this.stack.Children.Add(info);
                        count++;

                        foreach (var item in job.Children())
                        {
                            Pastes db = new Pastes
                            {
                                Title = item["Title"].ToString(),
                                Date = (DateTime?)item["Date"],
                                EmailCount = (int)item["EmailCount"]
                            };
                            var description = string.IsNullOrEmpty(db.Title) ? "No Name" : db.Title;
                            var date = "Date: " + (string.IsNullOrEmpty(db.Date.ToString()) ? "No Date" : ((DateTime)db.Date).ToString("dd MMM yy HH:mm"));
                            var emaillb = "Emails: " + db.EmailCount.ToString();

                            var pastetext = new Label { Text = description, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                            var pastetext2 = new Label { Text = date, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };

                            var pastetext3 = new Label { Text = emaillb, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };

                            this.stack.Children.Add(pastetext);
                            this.stack.Children.Add(pastetext2);
                            this.stack.Children.Add(pastetext3);
                            count++;
                        }
                    }
                }
                else
                {
                    this.PassStack.Children.Clear();
                    this.Setup();
                    var info = new Label { AutomationId = "goodbad", Text = "Your email address has not been included in any data breach.", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    this.stack.Children.Add(info);
                }

                Analytics.TrackEvent("HIBP");
            }
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