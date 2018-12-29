using PwnedPasswords.Interfaces;
using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(PwnedPasswords.Droid.Store))]
namespace PwnedPasswords.Droid
{
    public class Store : IStore
    {
        public void GetStore()
        {
            Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=pwnedpasswords.pwnedpasswords"));
        }
    }
}
