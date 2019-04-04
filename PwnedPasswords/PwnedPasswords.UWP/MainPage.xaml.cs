namespace PwnedPasswords.UWP
{
    public sealed partial class MainPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainPage"/> class.
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();
            PwnedPasswords.App.InitHash(new UWPGetHash());
            PwnedPasswords.App.InitAPI(new UWPGetAPI());
            this.LoadApplication(new PwnedPasswords.App());
        }
    }
}
