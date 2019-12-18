using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for ReportStock.xaml
    /// </summary>
    public partial class ReportStock : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private ProductsModel product;
        private CollectionViewSource CVS;
        private ListCollectionView bindlistview;
        public ReportStock()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            product = (ProductsModel)this.FindResource("pro");
            POS_Product();
        }
        private void POS_Product()
        {
            webHandler.POS_Get_ProductAsync();
            webHandler.POS_Get_ProductCompleted += (x, y) =>
            {
                var result = y.Result;
                product.ProductsCollection = new ObservableCollection<ProductsModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    product.ProductsCollection.Add(new ProductsModel()
                    {
                        Productid = arg[0],
                        ProductName = arg[1],
                        ProductCatagoryid = arg[2],
                        ProductCatagoryDesc = arg[3],
                        ProductPrice = arg[4],
                        ProductStatusid = arg[5],
                        ProductStatusDesc = arg[6],
                        ProductQuantity = arg[7],
                        ProductDesc = arg[8],
                        ProductBuyingPrice = arg[9],
                        ProductProfit = arg[10],

                    });
                }
                CVS = ((CollectionViewSource)(this.FindResource("cvs")));
                bindlistview = CVS.View as ListCollectionView;
                chart.ItemsSource = product.ProductsCollection;
            };
        }
        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Reports.xaml", UriKind.Relative));
        }

        private void printButton_TouchUp(object sender, TouchEventArgs e)
        {
            CrystalStockReport1 win2 = new CrystalStockReport1();
            win2.Show();
   
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && bindlistview != null)
            {
                bindlistview.Filter = delegate (object cust) { return ((ProductsModel)cust).ProductName.ToLower().Contains(search.Text.ToLower()); };
            }
            else if (search.Text == string.Empty)
            {
                bindlistview.Filter = null;
            }
        }

        private void ToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                var a = (ToggleSwitch)sender;
                if ((bool)a.IsChecked)
                {
                    bindlistview.Filter = delegate (object cust) { return ((ProductsModel)cust).ProductStatusDesc == "Active"; };
                }
                else { bindlistview.Filter = null; }
            }
            catch (Exception e1)
            {
                Debug.Write(e1.Message);
            }

        }

        private async void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var a = tab.SelectedIndex;
                if (a == 1)
                {
                    chart.ItemsSource = product.ProductsCollection;
                }
            }

            catch (Exception e1)
            {
                Debug.Write(e1.Message);
                var win = (MetroWindow)Application.Current.MainWindow;
                var res = await win.ShowMessageAsync("Something went wrong", "Please select some dates to view graph",
                    MessageDialogStyle.Affirmative);
            }
        }

    }
}
