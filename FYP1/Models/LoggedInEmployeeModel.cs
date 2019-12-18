using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FYP1.Models
{
    class LoggedInEmployeeModel : INotifyPropertyChanged, IDataErrorInfo
    {
        private static int _rank;
        private static int _employeeId;
        private static int _status;
        private static int _absent;
        private static string _fullName;
        private static string _email;

        public int Rank
        {
            get
            {
                return _rank;
            }

            set { this.SetProperty(ref _rank, value); }

        }

        public int EmployeeId
        {
            get
            {
                return _employeeId;
            }

            set { this.SetProperty(ref _employeeId, value); }

        }

        public int Status
        {
            get
            {
                return _status;
            }

            set { this.SetProperty(ref _status, value); }

        }

        public int Absent
        {
            get
            {
                return _absent;
            }

            set { this.SetProperty(ref _absent, value); }

        }

        public string FullName
        {
            get
            {
                return _fullName;
            }

            set { this.SetProperty(ref _fullName, value); }

        }

        public string Email
        {
            get
            {
                return _email;
            }

            set { this.SetProperty(ref _email, value); }

        }
        public string Error
        {
            get
            {
                return string.Empty;
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
