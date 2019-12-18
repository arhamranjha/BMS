using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FYP1.Models
{
    class StatusModel : INotifyPropertyChanged
    {
        private string _statusid;
        private string _statusDescription;
        private ObservableCollection<StatusModel> _statusCollection;
        public event PropertyChangedEventHandler PropertyChanged;

        public string Statusid
        {
            get
            {
                return _statusid;
            }

            set { this.SetProperty(ref this._statusid, value); }

        }

        public string StatusDescription 
        {
            get
            {
                return _statusDescription;
            }

            set { this.SetProperty(ref this._statusDescription, value); }

        }

        internal ObservableCollection<StatusModel> StatusCollection
        {
            get
            {
                return _statusCollection;
            }

            set { this.SetProperty(ref this._statusCollection, value); }

        }

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
