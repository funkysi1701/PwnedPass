using System;
using System.Collections.Generic;
using System.Linq;

namespace PwnedPasswords.ViewModel
{
    /// <summary>
    /// TempViewModel
    /// </summary>
    public class TempViewModel
    {
        public readonly List<DataBreach> collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="TempViewModel"/> class.
        /// </summary>
        public TempViewModel()
        {
            this.collection = new List<DataBreach>();
            var table = App.Database.GetAll();
            table = table.OrderByDescending(s => s.AddedDate);
            foreach (var s in table)
            {
                if (s.AddedDate > DateTime.Today.AddYears(-1))
                {
                    this.collection.Add(s);
                }
            }
        }
    }
}
