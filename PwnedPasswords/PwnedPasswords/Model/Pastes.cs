using SQLite;
using System;

namespace PwnedPasswords
{
    public class Pastes
    {
        [PrimaryKey]
        public string Id { get; set; }

        public string Title { get; set; }

        public DateTime? Date { get; set; }

        public int EmailCount { get; set; }
    }
}
