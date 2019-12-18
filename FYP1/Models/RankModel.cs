using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FYP1.Models
{
    class RankModel : INotifyPropertyChanged
    {
        private int _rankid;
        private string _rankDescription;
        private ObservableCollection<RankModel> _rankCollection;
        public event PropertyChangedEventHandler PropertyChanged;

        public int Rankid
        {
            get
            {
                return _rankid;
            }

            set { this.SetProperty(ref this._rankid, value); }

        }

        public string RankDescription
        {
            get
            {
                return _rankDescription;
            }

            set { this.SetProperty(ref this._rankDescription, value); }

        }

        internal ObservableCollection<RankModel> RankCollection
        {
            get
            {
                return _rankCollection;
            }

            set { this.SetProperty(ref this._rankCollection, value); }

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
