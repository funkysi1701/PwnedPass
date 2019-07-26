// <copyright file="ISQLite.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using SQLite;

namespace PwnedPasswords
{
    /// <summary>
    /// ISQLite.
    /// </summary>
    public interface ISQLite
    {
        /// <summary>
        /// GetConnection.
        /// </summary>
        /// <returns>SQLiteConnection.</returns>
        SQLiteConnection GetConnection();
    }
}