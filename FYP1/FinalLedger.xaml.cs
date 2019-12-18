using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for FinalLedger.xaml
    /// </summary>
    public partial class FinalLedger : Page
    {
        private CustomerModel customer;
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private CollectionViewSource CVS;
        private ListCollectionView bindlistview;
        public FinalLedger()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            customer = (CustomerModel)this.FindResource("pro");
            POS_GET_CUSTOMER();
            webHandler.POS_Get_CustomerCompleted += (x, y) =>
            {
                var result = y.Result;
                customer.CustomerCollection = new ObservableCollection<CustomerModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "|||" }, StringSplitOptions.None);
                    customer.CustomerCollection.Add(new CustomerModel()
                    {
                        Customerid = arg[0],
                        CustomerName = arg[1],
                        Customerbalance = arg[2],
                        Customerphone = arg[3],
                        Customeraddress = arg[4],
                        CustomerLocation = arg[5],
                        CustomerLastTally = arg[6],
                    });
                }
                CVS = ((CollectionViewSource)(this.FindResource("cvs")));
                bindlistview = CVS.View as ListCollectionView;
                bindlistview.SortDescriptions.Add(new System.ComponentModel.SortDescription("Customerbalance", System.ComponentModel.ListSortDirection.Descending));
            };
        }
        private void POS_GET_CUSTOMER()
        {
            webHandler.POS_Get_CustomerAsync();
        }
        private void backbutton(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
        }
 
        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && bindlistview != null)
            {
                bindlistview.Filter = delegate (object cust) { return ((CustomerModel)cust).CustomerName.ToLower().Contains(search.Text.ToLower()); };
            }
            else if (search.Text == string.Empty)
            {
                bindlistview.Filter = null;
            }
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            CrystalFinalLedgerReport win2 = new CrystalFinalLedgerReport();
            win2.Show();

        }
    }
}
