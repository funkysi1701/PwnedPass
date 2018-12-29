using SQLite;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace PwnedPasswords
{
    public class Database
    {
        readonly SQLiteConnection database;
        static object locker = new object();

        public Database()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();

            database.CreateTable<DataBreach>();
            database.CreateTable<HIBP>();
            database.CreateTable<LastEmail>();
        }

        public int SaveDataBreach(DataBreach databreach)
        {
            lock (locker)
            {
                if (databreach.Id != 0)
                {
                    database.Delete(databreach);
                    database.Insert(databreach);
                    return databreach.Id;
                }
                else
                {
                    database.Delete(databreach);
                    return database.Insert(databreach);
                }
            }
        }

        public void EmptyDataBreach()
        {
            database.DropTable<DataBreach>();
            database.CreateTable<DataBreach>();
        }

        public IEnumerable<HIBP> GetHIBP()
        {
            lock (locker)
            {
                return (from c in database.Table<HIBP>()
                        select c).ToList();
            }
        }

        public int SaveHIBP(HIBP hibp)
        {
            lock (locker)
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
            lock (locker)
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
            lock (locker)
            {
                return (from c in database.Table<DataBreach>().Take(id)
                        select c).ToList();
            }
        }

        public IEnumerable<DataBreach> GetAll()
        {
            lock (locker)
            {
                return (from c in database.Table<DataBreach>()
                        select c).ToList();
            }
        }

        public IEnumerable<LastEmail> GetLastEmail()
        {
            lock (locker)
            {
                return (from c in database.Table<LastEmail>()
                        select c).ToList();
            }
        }
    }
}
