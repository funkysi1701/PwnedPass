// <copyright file="Database.cs" company="FunkySi1701">
// Copyright (c) FunkySi1701. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using SQLite;
using Xamarin.Forms;

namespace PwnedPasswords
{
    /// <summary>
    /// Database
    /// </summary>
    public class Database
    {
        private static readonly object Locker = new object();
        private readonly SQLiteConnection database;

        /// <summary>
        /// Initializes a new instance of the <see cref="Database"/> class.
        /// Database
        /// </summary>
        public Database()
        {
            this.database = DependencyService.Get<ISQLite>().GetConnection();
            this.database.CreateTable<DataBreach>();
            this.database.CreateTable<HIBP>();
            this.database.CreateTable<LastEmail>();
        }

        /// <summary>
        /// SaveDataBreach
        /// </summary>
        /// <param name="databreach">databreach</param>
        /// <returns>int</returns>
        public int SaveDataBreach(DataBreach databreach)
        {
            lock (Locker)
            {
                if (databreach.Id != 0)
                {
                    this.database.Delete(databreach);
                    this.database.Insert(databreach);
                    return databreach.Id;
                }
                else
                {
                    this.database.Delete(databreach);
                    return this.database.Insert(databreach);
                }
            }
        }

        /// <summary>
        /// EmptyDataBreach
        /// </summary>
        public void EmptyDataBreach()
        {
            this.database.DropTable<DataBreach>();
            this.database.CreateTable<DataBreach>();
        }

        /// <summary>
        /// GetHIBP
        /// </summary>
        /// <returns>Collection of HIBP</returns>
        public IEnumerable<HIBP> GetHIBP()
        {
            lock (Locker)
            {
                return (from c in database.Table<HIBP>()
                        select c).ToList();
            }
        }

        public int SaveHIBP(HIBP hibp)
        {
            lock (Locker)
            {
                if (hibp.Id != 0)
                {
                    database.Delete(hibp);
                    database.Insert(hibp);
                    return hibp.Id;
                }
                else
                {
                    return database.Insert(hibp);
                }
            }
        }

        public int SaveLastEmail(LastEmail lastemail)
        {
            lock (Locker)
            {
                if (lastemail.Id != 0)
                {
                    database.Update(lastemail);
                    return lastemail.Id;
                }
                else
                {
                    return database.Insert(lastemail);
                }
            }
        }

        public IEnumerable<DataBreach> Get(int id)
        {
            lock (Locker)
            {
                return (from c in database.Table<DataBreach>().Take(id)
                        select c).ToList();
            }
        }

        public IEnumerable<DataBreach> GetAll()
        {
            lock (Locker)
            {
                return (from c in database.Table<DataBreach>()
                        select c).ToList();
            }
        }

        public IEnumerable<LastEmail> GetLastEmail()
        {
            lock (Locker)
            {
                return (from c in database.Table<LastEmail>()
                        select c).ToList();
            }
        }
    }
}
