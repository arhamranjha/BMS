using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace FYP1.Models
{
    public class ItemViewModel : ValidatableModel
    {
        // Properties  

        private bool _open;
        private double _total;
        private double _balance;
        private static string _time;


        public bool Open
        {
            get { return _open; }

            set { this.SetProperty(ref this._open, value); }
        }

       
        public double Total
        {
            get { return _total; }

            set { this.SetProperty(ref this._total, value); }
        }
        [Required]
        [CustomValidation(typeof(ItemViewModel), "checkbal")]
        public double Balance
        {
            get { return _balance; }
            set { this.SetProperty(ref this._balance, value); }
        }
        public static ValidationResult checkbal(object obj, ValidationContext context)
        {
            var emp = (ItemViewModel)context.ObjectInstance;
            if (emp.Balance < emp.Total)
            {
                return new ValidationResult("Cash is not enough",
                    new List<string> { "Balance" });
            }
            return ValidationResult.Success;
        }

        public string Time
        {
            get
            {
                return _time;
            }

            set { this.SetProperty(ref _time, value); }
        }
    }
}
