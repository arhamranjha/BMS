using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FYP1.Models
{
    public class SaleModelProducts : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _productid;
        private string _productName;
        private string _productCatagory;
        private string _productPrice;
        private string _productStatus;
        private string _productQuantity;
        private string _productDesc;
        private string _productBuyingPrice;
        private string _productProfit;
        private string _productTotalQuantity;
        private string _productTotalPrice;

        private List<SaleModelProducts> _productOptionList;

        private ObservableCollection<SaleModelProducts> _productCollection;

        public string Productid
        {
            get
            {
                return _productid;
            }

            set { this.SetProperty(ref this._productid, value); }
        }

        public string ProductName
        {
            get
            {
                return _productName;
            }

            set { this.SetProperty(ref this._productName, value); }
        }

        public string ProductCatagory
        {
            get
            {
                return _productCatagory;
            }

            set { this.SetProperty(ref this._productCatagory, value); }
        }

        public string ProductPrice
        {
            get
            {
                return _productPrice;
            }

            set { this.SetProperty(ref this._productPrice, value); }
        }



        public string ProductQuantity
        {
            get
            {
                return _productQuantity;
            }

            set { this.SetProperty(ref this._productQuantity, value); }
        }

        public string ProductDesc
        {
            get
            {
                return _productDesc;
            }

            set { this.SetProperty(ref this._productDesc, value); }
        }

        public string ProductBuyingPrice
        {
            get
            {
                return _productBuyingPrice;
            }

            set { this.SetProperty(ref this._productBuyingPrice, value); }
        }

        public string ProductProfit
        {
            get
            {
                return _productProfit;
            }

            set { this.SetProperty(ref this._productProfit, value); }
        }

        public List<SaleModelProducts> ProductOptionList
        {
            get
            {
                return _productOptionList;
            }

            set { this.SetProperty(ref this._productOptionList, value); }
        }

        public ObservableCollection<SaleModelProducts> ProductCollection
        {
            get
            {
                return _productCollection;
            }

            set { this.SetProperty(ref this._productCollection, value); }
        }



        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public string ProductStatus
        {
            get
            {
                return _productStatus;
            }

            set { this.SetProperty(ref this._productStatus, value); }
        }

        public string ProductTotalQuantity
        {
            get
            {
                return _productTotalQuantity;
            }

            set { this.SetProperty(ref this._productTotalQuantity, value); }
        }

        public string ProductTotalPrice
        {
            get
            {
                return _productTotalPrice;
            }

            set { this.SetProperty(ref this._productTotalPrice, value); }
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
            switch (propertyName)
            {
                case "ProductTotalQuantity":

                    if (int.Parse(ProductTotalQuantity) >= 0 && int.Parse(ProductTotalQuantity) <= int.Parse(ProductQuantity))
                    {
                        return string.Empty;
                    }

                    return "Cant be more than product quatity";
                default:
                    return string.Empty;
            }
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
