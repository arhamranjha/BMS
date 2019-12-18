using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace FYP1.Models
{
    class BillProductModel : ValidatableModel, INotifyPropertyChanged
    {
        private int _billid;
        private int _customerid;
        private int _quantity;
        private string _productName;
        private Decimal _rate;
        private Decimal _totalPerItem;
        private Decimal _totalAmount;
        private string _status;
        private Decimal _balance;
        private DateTime _dateTime;

        private ObservableCollection<BillProductModel> _billCollection;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public int Billid
        {
            get
            {
                return _billid;
            }

            set { this.SetProperty(ref this._billid, value); }
        }

        public int Customerid
        {
            get
            {
                return _customerid;
            }

            set { this.SetProperty(ref this._customerid, value); }
        }
        [Required]
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value != this.Quantity)
                {
                    _quantity = value;
                    OnPropertyChanged();
                    OnPropertyChanged("TotalPerItem");
                }
            }
        }
        [Required]
        public string ProductName
        {
            get
            {
                return _productName;
            }

            set { this.SetProperty(ref this._productName, value); }
        }
        [Required]
        public Decimal Rate
        {
            get
            {
                return _rate;
            }

            set
            {
                if (value != this.Rate)
                {
                    _rate = value;
                    OnPropertyChanged();
                    OnPropertyChanged("TotalPerItem");
                }
            }
        }

        public Decimal TotalPerItem
        {
            get
            {
                return Quantity * Rate;
            }

        }

        public string Status
        {
            get
            {
                return _status;
            }
            set { this.SetProperty(ref this._status, value); }
        }

        public Decimal Balance
        {
            get
            {
                return _balance;
            }

            set { this.SetProperty(ref this._balance, value); }
        }

        public ObservableCollection<BillProductModel> BillCollection
        {
            get
            {
                return _billCollection;
            }
            set
            {
                if (value != this.BillCollection)
                {
                    _billCollection = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime DateTime
        {
            get
            {
                return _dateTime;
            }

            set { this.SetProperty(ref this._dateTime, value); }
        }

        public decimal TotalAmount
        {
            get
            {
                return _totalAmount;
            }

            set { this.SetProperty(ref this._totalAmount, value); }
        }
    }
}

