using SQLite;

namespace PwnedPasswords
{
    public class HIBP
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public long TotalAccounts { get; set; }
        public int TotalBreaches { get; set; }
    }
}
