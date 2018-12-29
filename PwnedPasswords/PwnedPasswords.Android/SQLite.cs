using SQLite;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(PwnedPasswords.Droid.SQLite))]
namespace PwnedPasswords.Droid
{
    public class SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var sqliteFilename = "pwnedpass.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);

            File.Open(path, FileMode.OpenOrCreate);

            var conn = new SQLiteConnection(path);

            return conn;
        }
    }
}