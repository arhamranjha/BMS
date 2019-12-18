using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    class DateModel : ValidatableModel
    {
        private DateTime _dateFrom;
        private DateTime _dateTo;
        private DateTime _today;
        private DateTime _yesterday;
        private DateTime _lastweek;
        private DateTime _lastmonth;

        private ObservableCollection<DateModel> _dates;

        public DateTime DateFrom
        {
            get
            {
                return _dateFrom;
            }

            set
            {
                this.SetProperty(ref this._dateFrom, value);
            }
        }

        public DateTime DateTo
        {
            get
            {
                return _dateTo;
            }
            set
            {
                this.SetProperty(ref this._dateTo, value);
            }
        }

        public DateTime Today
        {
            get
            {
                return DateTime.Today;
            }

           
        }

        public DateTime Yesterday
        {
            get
            {
                return DateTime.Now.AddDays(-1);
            }

          
        }

        public DateTime Lastweek
        {
            get
            {
                return DateTime.Now.AddDays(-7);
            }

            
        }

        public DateTime Lastmonth
        {
            get
            {
                return DateTime.Now.AddDays(-30);
            }

           
        }

        internal ObservableCollection<DateModel> Dates
        {
            get
            {
                return _dates;
            }

            set
            {
                this.SetProperty(ref this._dates, value);
            }
        }
    }
}
