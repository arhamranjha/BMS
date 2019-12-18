using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    class CatagoryModel : ValidatableModel
    {
        private string _catagoryid;
        private string _catagoryName;
        private string _catagoryDesc;

        private ObservableCollection<CatagoryModel> _catagoryCollection;


        public string Catagoryid
        {
            get
            {
                return _catagoryid;
            }

            set
            {
                this.SetProperty(ref this._catagoryid, value);
            }
        }
        [Required]
        public string CatagoryName
        {
            get
            {
                return _catagoryName;
            }

            set
            {
                this.SetProperty(ref this._catagoryName, value);
            }
        }

        [Required]
        public string CatagoryDesc
        {
            get
            {
                return _catagoryDesc;
            }

            set
            {
                this.SetProperty(ref this._catagoryDesc, value);
            }
        }

        public ObservableCollection<CatagoryModel> CatagoryCollection
        {
            get
            {
                return _catagoryCollection;
            }

            set
            {
                this.SetProperty(ref this._catagoryCollection, value);
            }
        }
    }
}
