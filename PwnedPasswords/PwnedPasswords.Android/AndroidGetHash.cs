// <copyright file="AndroidGetHash.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using System.Security.Cryptography;
using System.Text;

namespace PwnedPasswords.Droid
{
    /// <summary>
    /// AndroidGetHash.
    /// </summary>
    public class AndroidGetHash : IHash
    {
        /// <summary>
        /// GetHash.
        /// </summary>
        /// <param name="input">input.</param>
        /// <returns>string.</returns>
        public string GetHash(string input)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hash = sha1.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}