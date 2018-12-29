using System;
using System.Collections.Generic;
using System.Linq;

namespace PwnedPasswords.ViewModel
{
    public class TempViewModel
    {
        public List<DataBreach> Collection;
        public TempViewModel()
        {
            Collection = new List<DataBreach>();
            var table = App.Database.GetAll();
            table = table.OrderByDescending(s => s.AddedDate);
            foreach (var s in table)
            {
                if (s.AddedDate > DateTime.Today.AddYears(-1))
                {
                    Collection.Add(s);
                }
            }
        }
    }
}
