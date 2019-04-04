using SQLite;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns>IEnumerable</returns>
        public IEnumerable<HIBP> GetHIBP()
        {
            lock (Locker)
            {
                return (from c in this.database.Table<HIBP>()
                        select c).ToList();
            }
        }

        /// <summary>
        /// SaveHIBP
        /// </summary>
        /// <param name="hibp">hibp</param>
        /// <returns>int</returns>
        public int SaveHIBP(HIBP hibp)
        {
            lock (Locker)
            {
                if (hibp.Id != 0)
                {
                    this.database.Delete(hibp);
                    this.database.Insert(hibp);
                    return hibp.Id;
                }
                else
                {
                    return this.database.Insert(hibp);
                }
            }
        }

        /// <summary>
        /// SaveLastEmail
        /// </summary>
        /// <param name="lastemail">lastemail</param>
        /// <returns>int</returns>
        public int SaveLastEmail(LastEmail lastemail)
        {
            lock (Locker)
            {
                if (lastemail.Id != 0)
                {
                    this.database.Update(lastemail);
                    return lastemail.Id;
                }
                else
                {
                    return this.database.Insert(lastemail);
                }
            }
        }

        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>IEnumerable</returns>
        public IEnumerable<DataBreach> Get(int id)
        {
            lock (Locker)
            {
                return (from c in this.database.Table<DataBreach>().Take(id)
                        select c).ToList();
            }
        }

        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns>IEnumerable</returns>
        public IEnumerable<DataBreach> GetAll()
        {
            lock (Locker)
            {
                return (from c in this.database.Table<DataBreach>()
                        select c).ToList();
            }
        }

        /// <summary>
        /// GetLastEmail
        /// </summary>
        /// <returns>IEnumerable</returns>
        public IEnumerable<LastEmail> GetLastEmail()
        {
            lock (Locker)
            {
                return (from c in this.database.Table<LastEmail>()
                        select c).ToList();
            }
        }
    }
}
