using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{

    public class CustomerModel: ValidatableModel
    {
        private string _customerid;
        private string _customerName;
        private string _customerbalance; 
        private string _customerphone;
        private string _customeraddress;
        private string _customerLocation;
        private string _customerLastTally;
        private string _customerStatusDesc;

        private ObservableCollection<CustomerModel> _customerCollection;
        [Required]
        public string Customerid
        {
            get
            {
                return _customerid;
            }

            set { this.SetProperty(ref this._customerid, value); }
        }
        [Required]

        public string CustomerName
        {
            get
            {
                return _customerName;
            }

            set { this.SetProperty(ref this._customerName, value); }
        }
        [Required]
        
        public string Customerbalance
        {
            get
            {
                return _customerbalance;
            }

            set { this.SetProperty(ref this._customerbalance, value); }
        }
        [Required]
        [Phone]
        public string Customerphone
        {
            get
            {
                return _customerphone;
            }

            set { this.SetProperty(ref this._customerphone, value); }
        }
        [Required]
        
        public string Customeraddress
        {
            get
            {
                return _customeraddress;
            }

            set { this.SetProperty(ref this._customeraddress, value); }
        }
        public string CustomerStatusDesc
        {
            get
            {
                return _customerStatusDesc;
            }

            set { this.SetProperty(ref this._customerStatusDesc, value); }
        }

        public ObservableCollection<CustomerModel> CustomerCollection
        {
            get
            {
                return _customerCollection;
            }

            set { this.SetProperty(ref this._customerCollection, value); }
        }

        public string CustomerLocation
        {
            get
            {
                return _customerLocation;
            }

            set { this.SetProperty(ref this._customerLocation, value); }
        }
        public string CustomerLastTally
        {
            get
            {
                return _customerLastTally;
            }

            set { this.SetProperty(ref this._customerLastTally, value); }
        }

     
    }
}
