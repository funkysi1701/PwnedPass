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
            Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=pwnedpasswords.pwnedpasswords"));
        }
    }
}
