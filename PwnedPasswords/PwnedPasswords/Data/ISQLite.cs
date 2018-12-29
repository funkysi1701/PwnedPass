using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PwnedPasswords
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
