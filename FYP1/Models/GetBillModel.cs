using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    class GetBillModel : ValidatableModel
    {
        private int _billid;
        private int _employeeId;
        private string _employeeName;
        private DateTime _current;
        private Decimal _total;
        private Decimal _totalBalance;
        private int _customerid;
        private string _customerName;
        private string _paidAmount;
        private string _paymentMethod;

        private ObservableCollection<GetBillModel> _getBillCollection;

        public int Billid
        {
            get
            {
                return _billid;
            }

            set { this.SetProperty(ref this._billid, value); }
        }

        public int EmployeeId
        {
            get
            {
                return _employeeId;
            }

            set { this.SetProperty(ref this._employeeId, value); }
        }

        public string EmployeeName
        {
            get
            {
                return _employeeName;
            }

            set { this.SetProperty(ref this._employeeName, value); }
        }

        public DateTime Current
        {
            get
            {
                return _current;
            }

            set { this.SetProperty(ref this._current, value); }
        }

        public decimal Total
        {
            get
            {
                return _total;
            }

            set { this.SetProperty(ref this._total, value); }
        }

        public int Customerid
        {
            get
            {
                return _customerid;
            }

            set { this.SetProperty(ref this._customerid, value); }
        }

        public string PaidAmount
        {
            get
            {
                return _paidAmount;
            }

            set { this.SetProperty(ref this._paidAmount, value); }
        }

        public string PaymentMethod
        {
            get
            {
                return _paymentMethod;
            }

            set { this.SetProperty(ref this._paymentMethod, value); }
        }

        public string CustomerName
        {
            get
            {
                return _customerName;
            }

            set { this.SetProperty(ref this._customerName, value); }
        }

        public ObservableCollection<GetBillModel> GetBillCollection
        {
            get
            {
                return _getBillCollection;
            }

            set { this.SetProperty(ref this._getBillCollection, value); }
        }

        public decimal TotalBalance
        {
            get
            {
                return _totalBalance;
            }

            set
            {
                _totalBalance = value;
            }
        }
    }
}
