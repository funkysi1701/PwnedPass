// <copyright file="BreachesPage.xaml.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

namespace PwnedPasswords.View
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AppCenter.Analytics;
    using Microsoft.AppCenter.Crashes;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using PwnedPasswords.Interfaces;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    /// <summary>
    /// Breaches Page.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BreachesPage : ContentPage
    {
        private readonly StackLayout stack;
        private readonly ViewModel.ViewModel vm;
        private Label totalBreaches;
        private Label totalAccounts;
        private int sortId = 0;
        private bool sortDirection = false;
        private string search = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="BreachesPage"/> class.
        /// </summary>
        /// <param name="breach">name of breach.</param>
        public BreachesPage(string breach)
        {
            this.InitializeComponent();
            this.PassStack.Children.Clear();
            this.stack = new StackLayout();
            this.scroll.Content = this.stack;
            string result = this.CallAPI(breach).Result;
            if (result != null && result.Length > 0)
            {
                JObject job = (JObject)JsonConvert.DeserializeObject(result);

                DataBreach db = new DataBreach
                {
                    Title = job["Title"].ToString(),
                };
                var title = new Label { Text = db.Title, TextColor = Color.DarkBlue, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
                this.stack.Children.Add(title);
                this.Title = db.Title;
                db.Domain = job["Domain"].ToString();
                var domain = new Label { Text = db.Domain, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                this.stack.Children.Add(domain);
                db.PwnCount = (int)job["PwnCount"];
                var count = new Label { Text = string.Format("{0:n0}", db.PwnCount) + " pwned accounts", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                this.stack.Children.Add(count);
                PwnedPasswords.Page pg = new PwnedPasswords.Page();
                long total = pg.GetAccountsRaw();
                if (Math.Ceiling(100 * ((double)db.PwnCount / total)) > 1)
                {
                    var percentage = new Label { Text = Math.Ceiling(100 * ((double)db.PwnCount / total)) + "% of HIBP", FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                    this.stack.Children.Add(percentage);
                }

                db.BreachDate = (DateTime)job["BreachDate"];
                var bdate = new Label { Text = "Breach Date " + db.BreachDate.ToShortDateString(), FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                this.stack.Children.Add(bdate);
                db.AddedDate = (DateTime)job["AddedDate"];
                var adate = new Label { Text = "Added Date " + db.AddedDate.ToShortDateString(), FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                this.stack.Children.Add(adate);
                db.IsVerified = (bool)job["IsVerified"];
                Label verified;
                if (db.IsVerified)
                {
                    verified = new Label { Text = "Verified: Y", TextColor = Color.Green, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }
                else
                {
                    verified = new Label { Text = "Verified: N", TextColor = Color.Red, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                }

                this.stack.Children.Add(verified);
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

                this.stack.Children.Add(sense);
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

                this.stack.Children.Add(retire);
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

                this.stack.Children.Add(spam);
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

                this.stack.Children.Add(fab);
                db.Description = Regex.Replace(job["Description"].ToString().Replace("&quot;", "'"), "<.*?>", string.Empty);
                var desc = new Label { Text = db.Description, FontSize = Device.GetNamedSize(NamedSize.Medium, this) };
                this.stack.Children.Add(desc);
                Analytics.TrackEvent("Breaches");
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BreachesPage"/> class.
        /// </summary>
        /// <param name="sortId">Sort Type.</param>
        /// <param name="sortDirection">Sort Direction.</param>
        public BreachesPage(int sortId, bool sortDirection)
        {
            try
            {
                this.sortId = sortId;
                this.sortDirection = sortDirection;
                this.InitializeComponent();
                this.BindingContext = this.vm = new ViewModel.ViewModel();
                this.PassStack.Children.Clear();
                this.stack = new StackLayout();
                this.scroll.Content = this.stack;
                this.SetupPage(string.Empty);
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Crashes.TrackError(e);
            }
        }

        /// <summary>
        /// CallAPI.
        /// </summary>
        /// <param name="breach">breach.</param>
        /// <returns>string.</returns>
        public async Task<string> CallAPI(string breach)
        {
            return await App.GetAPI.GetHIBP("https://pwnedpassapifsi.azurewebsites.net/api/HIBP/GetBreach?breach=" + breach);
        }

        /// <summary>
        /// On Button Click.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">args.</param>
        public void OnButtonClicked(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            this.Navigation.PushAsync(new BreachesPage(btn.AutomationId));
        }

        private void SetupPage(string search)
        {
            string datedirection = string.Empty;
            string numdirection = string.Empty;
            string namedirection = string.Empty;
            if (this.sortDirection)
            {
                this.sortDirection = false;
                if (this.sortId == 0)
                {
                    datedirection = "^";
                }

                if (this.sortId == 1)
                {
                    numdirection = "^";
                }

                if (this.sortId == 2)
                {
                    namedirection = "^";
                }
            }
            else
            {
                this.sortDirection = true;
                if (this.sortId == 0)
                {
                    datedirection = "v";
                }

                if (this.sortId == 1)
                {
                    numdirection = "v";
                }

                if (this.sortId == 2)
                {
                    namedirection = "v";
                }
            }

            this.PassStack.Children.Clear();
            this.stack.Children.Clear();
            StackLayout horizstack = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
            };
            Grid searchgrid = new Grid();
            searchgrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            searchgrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            Button adate = new Button { BackgroundColor = Color.LightGreen, Text = datedirection + " Date", HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, this) };
            Button num = new Button { BackgroundColor = Color.LightGreen, Text = numdirection + " pwned accounts", HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, this) };
            Button name = new Button
            {
                BackgroundColor = Color.LightGreen,
                Text = namedirection + " Name",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Micro, this),
            };

            Entry searchvalue = new Entry { Placeholder = "search", HorizontalOptions = LayoutOptions.FillAndExpand, FontSize = Device.GetNamedSize(NamedSize.Micro, this) };
            Button cancel = new Button
            {
                Text = "X",
                HorizontalOptions = LayoutOptions.FillAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Micro, this),
            };
            adate.Clicked += this.DateClicked;
            num.Clicked += this.NumClicked;
            name.Clicked += this.NameClicked;
            searchvalue.Completed += this.SearchCompleted;
            cancel.Clicked += this.CancelClicked;

            this.totalBreaches = new Label { Text = this.vm.Breach, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.totalAccounts = new Label { Text = this.vm.Accounts, FontAttributes = FontAttributes.Bold, TextColor = Color.Black, FontSize = Device.GetNamedSize(NamedSize.Large, this) };
            this.stack.Children.Add(this.totalBreaches);
            this.stack.Children.Add(this.totalAccounts);
            horizstack.Children.Add(adate);
            horizstack.Children.Add(num);
            horizstack.Children.Add(name);
            searchgrid.Children.Add(searchvalue, 0, 0);
            Grid.SetColumnSpan(searchvalue, 7);
            searchgrid.Children.Add(cancel, 7, 0);
            this.stack.Children.Add(searchgrid);
            this.stack.Children.Add(horizstack);
            this.DisplayData(search);
            Analytics.TrackEvent("Sorted");
        }

        private void CancelClicked(object sender, EventArgs e)
        {
            this.search = string.Empty;
            this.SetupPage(this.search);
        }

        private void SearchCompleted(object sender, EventArgs e)
        {
            this.search = ((Entry)sender).Text;
            if (this.search == null)
            {
                this.search = string.Empty;
            }

            this.SetupPage(this.search);
        }

        private void DisplayData(string search)
        {
            var table = App.Database.GetAll();
            if (search != string.Empty)
            {
                if (!this.sortDirection)
                {
                    switch (this.sortId)
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
                    switch (this.sortId)
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

                string breach = table.Count().ToString() + " data breaches";
                this.totalBreaches.Text = breach;
                string accounts = string.Format("{0:n0}", table.Sum(x => x.PwnCount)) + " pwned accounts";
                this.totalAccounts.Text = accounts;
            }
            else
            {
                if (!this.sortDirection)
                {
                    switch (this.sortId)
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
                    switch (this.sortId)
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
                    BackgroundColor = Color.LightBlue,
                };
                breaches.Clicked += this.OnButtonClicked;
                this.stack.Children.Add(breaches);
            }

            table = null;
        }

        private void DateClicked(object sender, EventArgs e)
        {
            this.sortId = 0;
            this.SetupPage(this.search);
        }

        private void NumClicked(object sender, EventArgs e)
        {
            this.sortId = 1;
            this.SetupPage(this.search);
        }

        private void NameClicked(object sender, EventArgs e)
        {
            this.sortId = 2;
            this.SetupPage(this.search);
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