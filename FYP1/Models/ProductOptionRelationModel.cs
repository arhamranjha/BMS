using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    class ProductOptionRelationModel : ValidatableModel
    {
        private string _productid;
        private string _productName;
        private string _optionid;
        private string _optionName;

        private ObservableCollection<ProductOptionRelationModel> _productsCollection;

        [Required]
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
        [Required]
        public string Optionid
        {
            get
            {
                return _optionid;
            }

            set { this.SetProperty(ref this._optionid, value); }
        }

        public string OptionName
        {
            get
            {
                return _optionName;
            }

            set { this.SetProperty(ref this._optionName, value); }
        }

        public ObservableCollection<ProductOptionRelationModel> ProductsCollection
        {
            get
            {
                return _productsCollection;
            }

            set { this.SetProperty(ref this._productsCollection, value); }
        }
    }
    public class Portfolio : ValidatableModel
    {
        private string _name;
        private string _optname;
        private string _nameid;
        private ObservableCollection<Portfolio> itemList;
        public string name
        {
            get
            {
                return _name;
            }

            set { this.SetProperty(ref this._name, value); }
        }
        public string nameid
        {
            get
            {
                return _nameid;
            }

            set { this.SetProperty(ref this._nameid, value); }
        }
        public string Optname
        {
            get
            {
                return _optname;
            }

            set { this.SetProperty(ref this._optname, value); }
        }
        public ObservableCollection<Portfolio> ItemList
        {
            get
            {
                return itemList;
            }

            set { this.SetProperty(ref this.itemList, value); }
        }
    }
}
