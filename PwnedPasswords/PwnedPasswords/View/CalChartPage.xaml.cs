using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using PwnedPasswords.Interfaces;
using PwnedPasswords.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PwnedPasswords.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalChartPage : ContentPage
    {
        private TempViewModel vm;

        public CalChartPage()
        {
            try
            {
                this.InitializeComponent();
                this.Setup();
            }
            catch (Exception e)
            {
                Analytics.TrackEvent("Error");
                Analytics.TrackEvent(e.Message);
                Crashes.TrackError(e);
            }
        }

        public void Setup()
        {
            this.BindingContext = this.vm = new TempViewModel();

            this.Chart(this.vm.collection);
        }

        public void Chart(List<DataBreach> collection)
        {
            var grid = new Grid();

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            List<DateTime> collectionDate = new List<DateTime>();
            foreach (var item in collection)
            {
                collectionDate.Add(item.AddedDate.Date);
            }

            var twobreaches = collectionDate.GroupBy(x => x)
                        .Where(group => group.Count() == 2)
                        .Select(group => group.Key).ToList();
            var threebreaches = collectionDate.GroupBy(x => x)
                        .Where(group => group.Count() == 3)
                        .Select(group => group.Key).ToList();
            DateTime end = DateTime.Now.Date;
            DateTime start = DateTime.Now.AddYears(-1).Date;

            var mon = new Label { Text = " M ", HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
            var tue = new Label { Text = " T ", HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
            var wed = new Label { Text = " W ", HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
            var thu = new Label { Text = " T ", HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
            var fri = new Label { Text = " F ", HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
            var sat = new Label { Text = " S ", HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };
            var sun = new Label { Text = " S ", HorizontalOptions = LayoutOptions.Center, TextColor = Color.Black };

            grid.Children.Add(mon, 3, 0);
            grid.Children.Add(tue, 4, 0);
            grid.Children.Add(wed, 5, 0);
            grid.Children.Add(thu, 6, 0);
            grid.Children.Add(fri, 7, 0);
            grid.Children.Add(sat, 8, 0);
            grid.Children.Add(sun, 2, 0);
            bool firstmonth = false;
            for (int j = 1; j < 67; j++)
            {
                for (int i = 2; i < 9; i++)
                {
                    if (j == 1 && start.DayOfWeek != DayOfWeek.Sunday)
                    {
                        i = (int)start.DayOfWeek + 2;
                        if (!firstmonth && start.Day != 1)
                        {
                            var month = new Label { Text = start.ToString("MMM yy"), VerticalOptions = LayoutOptions.Center, TextColor = Color.Black };
                            grid.Children.Add(month, 0, j);
                            Grid.SetColumnSpan(month, 2);
                            firstmonth = true;
                        }
                    }

                    var text = new Label { Text = "   ", BackgroundColor = Color.Gray };
                    foreach (var item in collection)
                    {
                        if (item.AddedDate.Date == start)
                        {
                            text = new Label { Text = string.Empty, BackgroundColor = Color.LightBlue, AutomationId = item.Name };
                            text.GestureRecognizers.Add(new TapGestureRecognizer
                            {
                                Command = new Command(() => this.ChartClicked(text.AutomationId)),
                            });
                            if (twobreaches.Contains(item.AddedDate.Date))
                            {
                                text = new Label { Text = string.Empty, BackgroundColor = Color.Blue, AutomationId = item.Name };
                                text.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => this.ChartClicked(text.AutomationId)),
                                });
                            }

                            if (threebreaches.Contains(item.AddedDate.Date))
                            {
                                text = new Label { Text = string.Empty, BackgroundColor = Color.DarkBlue, AutomationId = item.Name };
                                text.GestureRecognizers.Add(new TapGestureRecognizer
                                {
                                    Command = new Command(() => this.ChartClicked(text.AutomationId)),
                                });
                            }
                        }
                    }

                    if (start.Day == 1)
                    {
                        j++;
                        var month = new Label { Text = start.ToString("MMM yy"), VerticalOptions = LayoutOptions.Center, TextColor = Color.Black };
                        grid.Children.Add(month, 0, j);
                        Grid.SetColumnSpan(month, 2);
                    }

                    grid.Children.Add(text, i, j);
                    start = start.AddDays(1);
                    if (start > end)
                    {
                        break;
                    }
                }

                if (start > end)
                {
                    break;
                }
            }

            this.PassStack.Children.Add(grid);
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

        private void ChartClicked(string id)
        {
            this.Navigation.PushAsync(new BreachesPage(id));
        }
    }
}