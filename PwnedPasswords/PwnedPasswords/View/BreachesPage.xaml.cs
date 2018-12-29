using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PwnedPasswords.Interfaces;

namespace PwnedPasswords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BreachesPage : ContentPage
    {
        public BreachesPage(int _SortId, bool _SortDirection)
        {
            try
            {
                SortId = _SortId;
                SortDirection = _SortDirection;
                InitializeComponent();
                PassStack.Children.Clear();
                stack = new StackLayout();
                scroll.Content = stack;
                SetupPage("");
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Crashes.TrackError(e);
            }
        }
        
        private void SetupPage(string search)
        {
            string datedirection = "";
            string numdirection = "";
            string namedirection = "";
            if (SortDirection)
            {
                SortDirection = false;
                if (SortId == 0) { datedirection = "^"; }
                if (SortId == 1) { numdirection = "^"; }
                if (SortId == 2) { namedirection = "^"; }
            }
            else {
                SortDirection = true;
                if (SortId == 0) { datedirection = "v"; }
                if (SortId == 1) { numdirection = "v"; }
                if (SortId == 2) { namedirection = "v"; }
            }
            PassStack.Children.Clear();
            stack.Children.Clear();
            StackLayout horizstack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            StackLayout searchstack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };
            Page pg = new Page();
            string breach = pg.GetBreach();
            string accounts = pg.GetAccounts();
            Button adate = new Button { BackgroundColor = Color.LightGreen, Text = datedirection + " Date",HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, this) };
            Button num = new Button { BackgroundColor = Color.LightGreen, Text = numdirection + " pwned accounts", HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, this) };
            Button name = new Button {
                BackgroundColor = Color.LightGreen,
                Text = namedirection + " Name",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Micro, this)
            };

            Entry searchvalue = new Entry { Placeholder = "search", HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, this) };
            Button cancel = new Button {
                Text = "Cancel",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Micro, this)
            };
            adate.Clicked += DateClicked;
            num.Clicked += NumClicked;
            name.Clicked += NameClicked;
            searchvalue.Completed += SearchCompleted;
            cancel.Clicked += CancelClicked;

            Label TotalBreaches = new Label { Text = breach, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            Label TotalAccounts = new Label { Text = accounts, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            stack.Children.Add(TotalBreaches);
            stack.Children.Add(TotalAccounts);
            horizstack.Children.Add(adate);
            horizstack.Children.Add(num);
            horizstack.Children.Add(name);
            searchstack.Children.Add(searchvalue);
            searchstack.Children.Add(cancel);
            stack.Children.Add(searchstack);
            stack.Children.Add(horizstack);
            DisplayData(search);
            Analytics.TrackEvent("Sorted");
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            Search = "";
            SetupPage(Search);
        }

        private void SearchCompleted(object sender, EventArgs e)
        {
            Search = ((Entry)sender).Text;
            if (Search == null) { Search = ""; }
            SetupPage(Search);
        }

        private void DisplayData(string search)
        {
            var table = App.Database.GetAll();
            if(search != "")
            {
                if (!SortDirection)
                {
                    switch (SortId)
                    {
                        case 1:
                            table = table
                                .OrderBy(s => s.PwnCount)
                                .Where(x => x.Name.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Domain.Contains(search))
                                .Take(50);
                            break;
                        case 2:
                            table = table
                                .OrderBy(s => s.Title)
                                .Where(x => x.Name.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Domain.Contains(search))
                                .Take(50);
                            break;
                        default:
                            table = table
                                .OrderBy(s => s.AddedDate)
                                .Where(x => x.Name.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Domain.Contains(search))
                                .Take(50);
                            break;
                    }
                }
                else
                {
                    switch (SortId)
                    {
                        case 1:
                            table = table
                                .OrderByDescending(s => s.PwnCount)
                                .Where(x => x.Name.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Domain.Contains(search))
                                .Take(50);
                            break;
                        case 2:
                            table = table
                                .OrderByDescending(s => s.Title)
                                .Where(x => x.Name.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Domain.Contains(search))
                                .Take(50);
                            break;
                        default:
                            table = table
                                .OrderByDescending(s => s.AddedDate)
                                .Where(x => x.Name.Contains(search) || x.Title.Contains(search) || x.Description.Contains(search) || x.Domain.Contains(search))
                                .Take(50);
                            break;
                    }
                }
            }
            else
            {
                if (!SortDirection)
                {
                    switch (SortId)
                    {
                        case 1:
                            table = table.OrderBy(s => s.PwnCount).Take(50);
                            break;
                        case 2:
                            table = table.OrderBy(s => s.Title).Take(50);
                            break;
                        default:
                            table = table.OrderBy(s => s.AddedDate).Take(50);
                            break;
                    }
                }
                else
                {
                    switch (SortId)
                    {
                        case 1:
                            table = table.OrderByDescending(s => s.PwnCount).Take(50);
                            break;
                        case 2:
                            table = table.OrderByDescending(s => s.Title).Take(50);
                            break;
                        default:
                            table = table.OrderByDescending(s => s.AddedDate).Take(50);
                            break;
                    }
                }
            }
            
            foreach (var s in table)
            {
                Button breaches = new Button
                {
                    Text = s.Title,
                    AutomationId = s.Name,
                    BackgroundColor = Color.LightBlue
                };
                breaches.Clicked += OnButtonClicked;
                stack.Children.Add(breaches);
            }
            table = null;
        }

        private void DateClicked(object sender, EventArgs e)
        {
            SortId = 0;
            SetupPage(Search);
        }
        private void NumClicked(object sender, EventArgs e)
        {
            SortId =1;
            SetupPage(Search);
        }
        private void NameClicked(object sender, EventArgs e)
        {
            SortId = 2;
            SetupPage(Search);
        }

        StackLayout stack;
        int SortId = 0;
        bool SortDirection = false;
        string Search = "";

        public BreachesPage(string breach)
        {
            InitializeComponent();
            PassStack.Children.Clear();
            stack = new StackLayout();
            scroll.Content = stack;
            string result = App.GetAPI.GetHIBP("https://haveibeenpwned.com/api/v2/breach/" + breach);
            if (result != null && result.Length > 0)
            {
                JObject job = (JObject)JsonConvert.DeserializeObject(result);

                DataBreach db = new DataBreach
                {
                    Title = job["Title"].ToString()
                };
                var title = new Label { Text = db.Title, TextColor = Color.DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
                stack.Children.Add(title);
                db.Domain = job["Domain"].ToString();
                var domain = new Label { Text = db.Domain, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                stack.Children.Add(domain);
                db.PwnCount = (int)job["PwnCount"];
                var count = new Label { Text = string.Format("{0:n0}", db.PwnCount) + " pwned accounts", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                stack.Children.Add(count);
                db.BreachDate = (DateTime)job["BreachDate"];
                var bdate = new Label { Text = "Breach Date " + db.BreachDate.ToShortDateString(), FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                stack.Children.Add(bdate);
                db.AddedDate = (DateTime)job["AddedDate"];
                var adate = new Label { Text = "Added Date " + db.AddedDate.ToShortDateString(), FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                stack.Children.Add(adate);
                db.IsVerified = (bool)job["IsVerified"];
                Label Verified;
                if (db.IsVerified)
                {
                    Verified = new Label { Text = "Verified: Y", TextColor = Color.Green, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                else
                {
                    Verified = new Label { Text = "Verified: N", TextColor = Color.Red, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                stack.Children.Add(Verified);
                db.IsSensitive = (bool)job["IsSensitive"];
                Label sense;
                if (db.IsSensitive)
                {
                    sense = new Label { Text = "Sensitive: Y", TextColor = Color.Green, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                else
                {
                    sense = new Label { Text = "Sensitive: N", TextColor = Color.Red, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                stack.Children.Add(sense);
                db.IsRetired = (bool)job["IsRetired"];
                Label retire;
                if (db.IsRetired)
                {
                    retire = new Label { Text = "Retired: Y", TextColor = Color.Green, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                else
                {
                    retire = new Label { Text = "Retired: N", TextColor = Color.Red, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                stack.Children.Add(retire);
                db.IsSpamList = (bool)job["IsSpamList"];
                Label spam;
                if (db.IsSpamList)
                {
                    spam = new Label { Text = "Spam: Y", TextColor = Color.Green, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                else
                {
                    spam = new Label { Text = "Spam: N", TextColor = Color.Red, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                stack.Children.Add(spam);
                db.IsFabricated = (bool)job["IsFabricated"];
                Label fab;
                if (db.IsFabricated)
                {
                    fab = new Label { Text = "Fabricated: Y", TextColor = Color.Green, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                else
                {
                    fab = new Label { Text = "Fabricated: N", TextColor = Color.Red, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                stack.Children.Add(fab);
                db.Description = Regex.Replace(job["Description"].ToString().Replace("&quot;", "'"), "<.*?>", String.Empty);
                var desc = new Label { Text = db.Description, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                stack.Children.Add(desc);
                Analytics.TrackEvent("Breaches");
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