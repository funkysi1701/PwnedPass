namespace PwnedPasswords.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();
            PwnedPasswords.App.InitHash(new UWPGetHash());
            PwnedPasswords.App.InitAPI(new UWPGetAPI());
            LoadApplication(new PwnedPasswords.App());
        }
    }
}
