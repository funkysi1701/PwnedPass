using SQLite;

namespace PwnedPasswords
{
    public class LastEmail
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Email { get; set; }
    }
}
