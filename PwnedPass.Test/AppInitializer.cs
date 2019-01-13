using System;
using System.IO;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace PwnedPass.Test
{
    public class AppInitializer
    {
        public static IApp StartApp(Platform platform)
        {
            if (platform == Platform.Android)
            {
                string dir1 = Directory.GetCurrentDirectory();
                string dir2 = Directory.GetParent(dir1).ToString();
                string dir3 = Directory.GetParent(dir2).ToString();
                string dir4 = Directory.GetParent(dir3).ToString();
                string dir5 = Directory.GetParent(dir4).ToString();
#if DEBUG
                string mode = "Debug";
#else
                string mode = "Release";
#endif
                return ConfigureApp.Android.Debug().ApkFile(dir5 + "\\PwnedPasswords\\PwnedPasswords\\PwnedPasswords.Android\\bin\\" + mode + "\\pwnedpasswords.pwnedpasswords.apk").StartApp();
            }

            return ConfigureApp.iOS.StartApp();
        }
    }
}