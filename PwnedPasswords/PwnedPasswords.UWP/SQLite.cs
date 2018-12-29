using SQLite;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(PwnedPasswords.UWP.SQLite))]
namespace PwnedPasswords.UWP
{
    public class SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "pwnedpass.db3";
            string documentsPath = Windows.Storage.ApplicationData.Current.LocalFolder.Path; // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            var conn = new SQLiteConnection(path);

            return conn;
        }
    }
}
