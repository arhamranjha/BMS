using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    public class ProductsModel : ValidatableModel
    {
        private string _productid;
        private string _productName;
        private string _productCatagoryid;
        private string _productStatusid;
        private string _productCatagoryDesc;
        private string _productStatusDesc;
        private string _productPrice;
        private string _productQuantity;
        private string _productDesc;
        private string _productBuyingPrice;
        private string _productProfit;

        private ObservableCollection<ProductsModel> _productsCollection;

        [Required]
        public string Productid
        {
            get
            {
                return _productid;
            }

            set { this.SetProperty(ref this._productid, value); }
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
        public string ProductCatagoryid
        {
            get
            {
                return _productCatagoryid;
            }

            set { this.SetProperty(ref this._productCatagoryid, value); }
        }

        [Required]
        public string ProductStatusid
        {
            get
            {
                return _productStatusid;
            }

            set { this.SetProperty(ref this._productStatusid, value); }
        }

        public string ProductCatagoryDesc
        {
            get
            {
                return _productCatagoryDesc;
            }

            set { this.SetProperty(ref this._productCatagoryDesc, value); }
        }

        public string ProductStatusDesc
        {
            get
            {
                return _productStatusDesc;
            }

            set { this.SetProperty(ref this._productStatusDesc, value); }
        }
        [Required]
        public string ProductPrice
        {
            get
            {
                return _productPrice;
            }

            set { this.SetProperty(ref this._productPrice, value); }
        }

        [Required]
        public string ProductQuantity
        {
            get
            {
                return _productQuantity;
            }

            set { this.SetProperty(ref this._productQuantity, value); }
        }

        [Required]
        public string ProductDesc
        {
            get
            {
                return _productDesc;
            }

            set { this.SetProperty(ref this._productDesc, value); }
        }

        [Required]
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

        public ObservableCollection<ProductsModel> ProductsCollection
        {
            get
            {
                return _productsCollection;
            }

            set { this.SetProperty(ref this._productsCollection, value); }

        }
    }
}
