using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    public class ProductOptionModel : ValidatableModel
    {
        private int _optionid;
        private string _optionname;
        private Decimal _optionsprice;
        private string _optionDesc;

        private ObservableCollection<ProductOptionModel> _optionCollection;

        public int Optionid
        {
            get
            {
                return _optionid;
            }

            set
            {
                this.SetProperty(ref this._optionid, value);
            }
        }
        [Required]
        public string Optionname
        {
            get
            {
                return _optionname;
            }
            set
            {
                this.SetProperty(ref this._optionname, value);
            }
        }
        [Required]
        public decimal Optionsprice
        {
            get
            {
                return _optionsprice;
            }

            set
            {
                this.SetProperty(ref this._optionsprice, value);
            }
        }
        [Required]
        public string OptionDesc
        {
            get
            {
                return _optionDesc;
            }

            set
            {
                this.SetProperty(ref this._optionDesc, value);
            }
        }

        public ObservableCollection<ProductOptionModel> OptionCollection
        {
            get
            {
                return _optionCollection;
            }

            set
            {
                this.SetProperty(ref this._optionCollection, value);
            }
        }
    }
}

