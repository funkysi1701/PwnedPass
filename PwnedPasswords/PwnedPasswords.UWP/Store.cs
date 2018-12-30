using PwnedPasswords.Interfaces;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(PwnedPasswords.UWP.Store))]
namespace PwnedPasswords.UWP
{
    public class Store : IStore
    {
        public void GetStore()
        {
            Device.OpenUri(new Uri("https://www.microsoft.com/en-gb/p/pwned-pass/9nm2whnztnlt"));
        }
    }
}
