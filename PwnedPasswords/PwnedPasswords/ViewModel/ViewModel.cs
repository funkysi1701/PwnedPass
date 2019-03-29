using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace PwnedPasswords.ViewModel
{
    /// <summary>
    /// ViewModel
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        private string breach;
        private string accounts;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/> class.
        /// </summary>
        public ViewModel()
        {
            this.Pg = new Page();
            this.Breach = this.Pg.GetBreach();
            this.Accounts = this.Pg.GetAccounts();
        }

        /// <summary>
        /// PropertyChanged
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets pg
        /// </summary>
        public Page Pg { get; set; }

        /// <summary>
        /// Gets breach
        /// </summary>
        public string Breach
        {
            get
            {
                return this.breach;
            }

            private set
            {
                if (this.breach != value)
                {
                    this.breach = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Breach"));
                }
            }
        }

        /// <summary>
        /// Gets breach
        /// </summary>
        public string Accounts
        {
            get
            {
                return this.accounts;
            }

            private set
            {
                if (this.accounts != value)
                {
                    this.accounts = value;
                    this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Accounts"));
                }
            }
        }
    }
}
