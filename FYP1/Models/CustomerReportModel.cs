using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    class CustomerReportModel : ValidatableModel
    {
        private int _billid;
        private int _employeeId;
        private string _current;
        private Decimal _total;      
        private Decimal _customerid;

        private ObservableCollection<CustomerReportModel> _customerBillCollection;

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

        public string Current
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

      

        public decimal Customerid
        {
            get
            {
                return _customerid;
            }
            set { this.SetProperty(ref this._customerid, value); }
        }

        public ObservableCollection<CustomerReportModel> CustomerCollection
        {
            get
            {
                return _customerBillCollection;
            }

            set { this.SetProperty(ref this._customerBillCollection, value); }
        }
    }
}
