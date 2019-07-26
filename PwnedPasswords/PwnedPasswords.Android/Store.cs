// <copyright file="Store.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using System;
using PwnedPasswords.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(PwnedPasswords.Droid.Store))]

namespace PwnedPasswords.Droid
{
    /// <summary>
    /// Store.
    /// </summary>
    public class Store : IStore
    {
        /// <summary>
        /// GetStore.
        /// </summary>
        public void GetStore()
        {
            Device.OpenUri(new Uri("https://play.google.com/store/apps/details?id=pwnedpasswords.pwnedpasswords"));
        }
    }
}
