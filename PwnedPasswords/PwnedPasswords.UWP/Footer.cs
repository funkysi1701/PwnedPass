using PwnedPasswords.Interfaces;
using Xamarin.Forms;
using Windows.ApplicationModel;

[assembly: Dependency(typeof(PwnedPasswords.UWP.Footer))]
namespace PwnedPasswords.UWP
{
    public class Footer : IFooter
    {
        public void AddFooter(View.MainPage mainPage, StackLayout stack)
        {
            Package package = Package.Current;
            PackageId packageId = package.Id;
            PackageVersion version = packageId.Version;
            var versioncode = new Label
            {
                Text = "  Version: " + string.Format("{0}.{1}", version.Major, version.Minor),
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