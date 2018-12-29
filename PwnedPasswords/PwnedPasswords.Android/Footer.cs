using Microsoft.AppCenter.Analytics;
using Plugin.CurrentActivity;
using PwnedPasswords.Interfaces;
using PwnedPasswords.View;
using Xamarin.Forms;

[assembly: Dependency(typeof(PwnedPasswords.Droid.Footer))]
namespace PwnedPasswords.Droid
{
    public class Footer : IFooter
    {
        public void AddFooter(MainPage mainPage, StackLayout stack)
        {
            var context = CrossCurrentActivity.Current.Activity;
            var versioncode = new Label
            {
                Text = " Version: " + context.PackageManager.GetPackageInfo(context.PackageName, 0).VersionName,
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Small, mainPage)
            };
            var about = new Label
            {
                Text = "  ';** Pwned Pass created by Simon Foster.",
                TextColor = Color.White,
                FontSize = Device.GetNamedSize(NamedSize.Small, mainPage)
            };
            stack.Children.Add(about);
            stack.Children.Add(versioncode);
        }
    }
}