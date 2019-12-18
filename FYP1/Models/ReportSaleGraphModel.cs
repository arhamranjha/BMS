using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FYP1.Models
{
    class ReportSaleGraphModel : ValidatableModel
    {
        private string _graphname;
        private string _graphvalue;
        private ObservableCollection<ReportSaleGraphModel> _graphCollection;

        public string Graphname
        {
            get
            {
                return _graphname;
            }

            set
            {
                this.SetProperty(ref this._graphname, value);
            }
        }

        public string Graphvalue
        {
            get
            {
                return _graphvalue;
            }

            set
            {
                this.SetProperty(ref this._graphvalue, value);
            }
        }

        internal ObservableCollection<ReportSaleGraphModel> GraphCollection
        {
            get
            {
                return _graphCollection;
            }

            set
            {
                this.SetProperty(ref this._graphCollection, value);
            }
        }
    }
}
