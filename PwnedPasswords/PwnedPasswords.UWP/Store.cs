// <copyright file="Store.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using PwnedPasswords.Interfaces;
using Xamarin.Forms;

[assembly: Dependency(typeof(PwnedPasswords.UWP.Store))]

namespace PwnedPasswords.UWP
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
            Device.OpenUri(new Uri("https://www.microsoft.com/en-gb/p/pwned-pass/9nm2whnztnlt"));
        }
    }
}
