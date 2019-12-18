using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    class ReportSaleModel : ValidatableModel
    {
        private string _Orderid;
        private string _Billid;
        private string _Current_DateTime;
        private string _Employee_id;
        private Decimal _total;
        private string _Employee_Name;
        private string _Product_id;
        private string _Product_Name;
        private string _Product_Quantity;
        private string _Optionid;
        private string _Optionname;
        private string _OptionQty;

        private ObservableCollection<ReportSaleModel> _saleReportCollection;
        private ObservableCollection<ReportSaleModel> _saleReportCollection1;

        public string Orderid
        {
            get
            {
                return _Orderid;
            }

            set { this.SetProperty(ref this._Orderid, value); }
        }

        public string Billid
        {
            get
            {
                return _Billid;
            }

            set { this.SetProperty(ref this._Billid, value); }
        }

        public string Current_DateTime
        {
            get
            {
                return _Current_DateTime;
            }

            set { this.SetProperty(ref this._Current_DateTime, value); }
        }

        public Decimal Total
        {
            get
            {
                return _total;
            }

            set { this.SetProperty(ref this._total, value); }
        }

        public string Employee_Name
        {
            get
            {
                return _Employee_Name;
            }

            set { this.SetProperty(ref this._Employee_Name, value); }
        }
        public string Employee_id
        {
            get
            {
                return _Employee_id;
            }

            set { this.SetProperty(ref this._Employee_id, value); }
        }
        public string Product_id
        {
            get
            {
                return _Product_id;
            }

            set { this.SetProperty(ref this._Product_id, value); }
        }

        public string Product_Name
        {
            get
            {
                return _Product_Name;
            }

            set { this.SetProperty(ref this._Product_Name, value); }
        }

        public string Product_Quantity
        {
            get
            {
                return _Product_Quantity;
            }

            set { this.SetProperty(ref this._Product_Quantity, value); }
        }

        public string Optionid
        {
            get
            {
                return _Optionid;
            }

            set { this.SetProperty(ref this._Optionid, value); }
        }

        public string Optionname
        {
            get
            {
                return _Optionname;
            }

            set { this.SetProperty(ref this._Optionname, value); }
        }

        public string OptionQty
        {
            get
            {
                return _OptionQty;
            }

            set { this.SetProperty(ref this._OptionQty, value); }
        }

        internal ObservableCollection<ReportSaleModel> SaleReportCollection
        {
            get
            {
                return _saleReportCollection;
            }

            set { this.SetProperty(ref this._saleReportCollection, value); }
        }

        internal ObservableCollection<ReportSaleModel> SaleReportCollection1
        {
            get
            {
                return _saleReportCollection1;
            }

            set { this.SetProperty(ref this._saleReportCollection1, value); }
        }


    }

}
