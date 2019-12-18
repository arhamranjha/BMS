using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FYP1.Models
{
    class BillModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private static int _billid;
        private static int _employeeId;
        private static DateTime _current;
        private static Decimal _total;
        private static Decimal _customerid;
        private string _paidAmount;
        private string _paymentMethod;


        public decimal Total
        {
            get
            {
                return _total;
            }

            set { this.SetProperty(ref _total, value); }

        }
        public int Billid
        {
            get
            {
                return _billid;
            }

            set { this.SetProperty(ref _billid, value); }

        }

        public int EmployeeId
        {
            get
            {
                return _employeeId;
            }

            set { this.SetProperty(ref _employeeId, value); }

        }

        public DateTime Current
        {
            get
            {
                return _current;
            }

            set { this.SetProperty(ref _current, value); }

        }

        
        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

  
        public decimal Customerid
        {
            get
            {
                return _customerid;
            }

            set
            {
                _customerid = value;
            }
        }

        public string PaidAmount
        {
            get
            {
                return _paidAmount;
            }

            set
            {
                _paidAmount = value;
            }
        }

        public string PaymentMethod
        {
            get
            {
                return _paymentMethod;
            }

            set
            {
                _paymentMethod = value;
            }
        }

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName);
            }
        }

        private string GetErrorForProperty(string propertyName)
        {
            return string.Empty;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.OnPropertyChaned(propertyName);
            return true;
        }

        private void OnPropertyChaned(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
