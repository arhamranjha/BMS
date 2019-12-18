using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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
    /// Interaction logic for SalesOrder.xaml
    /// </summary>
    public partial class SalesOrder : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private SaleModelProducts catagories = new SaleModelProducts();
        private SaleModelProducts allProducts = new SaleModelProducts();
        private static SaleModelProducts sold = new SaleModelProducts();
        private ItemViewModel _itemViewModel = new ItemViewModel();
        private ObservableCollection<SaleModelProducts> orginalProduct = new ObservableCollection<SaleModelProducts>();
        private int? resultout = -1;
        private BillModel bill;
        private int Flag=0;

        public SaleModelProducts Sold
        {
            get
            {
                return sold;
            }

            set
            {
                sold = value;
            }
        }

        public SalesOrder()
        {
            InitializeComponent();
            webHandler.POS_Start_BillCompleted += (x, y) =>
            {
                var result = y.Result;
                resultout = y.got;
                if (resultout == 1)
                {
                    string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in arr)
                    {
                        string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                        bill = new BillModel()
                        {
                            Billid = int.Parse(arg[0]),
                            Current = DateTime.ParseExact(arg[1], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                            EmployeeId = int.Parse(arg[2]),
                            Total = Decimal.Parse(arg[3]),
                        };
                    }
                }

            };
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            POS_Product();
            POS_catagory();
            POS_Bill();

        }

        private void POS_Bill()
        {
            {
                webHandler.POS_Start_BillAsync(resultout, new LoggedInEmployeeModel().EmployeeId);

            }
        }

        private void setDataContext()
        {
            ///call this when last call to aysync method/////////
            fly.DataContext = _itemViewModel;
            catagory.DataContext = catagories;
            gridSource.DataContext = allProducts;
            flyoutlistbox.DataContext = Sold;
        }

        private void selectedProducts_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = true;
            updateRecipt();

        }

        private void POS_catagory()
        {
            webHandler.POS_Get_CatagoryAsync();
            webHandler.POS_Get_CatagoryCompleted += (x, y) =>
            {
                var result = y.Result;
                catagories.ProductCollection = new ObservableCollection<SaleModelProducts>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    catagories.ProductCollection.Add(new SaleModelProducts()
                    {
                        Productid = arg[0],
                        ProductName = arg[1],
                    });
                }
            };

        }

        private void POS_Product()
        {
            webHandler.POS_Get_ProductAsync();
            webHandler.POS_Get_ProductCompleted += (x, y) =>
            {
                var result = y.Result;
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    orginalProduct.Add(new SaleModelProducts()
                    {
                        Productid = arg[0],
                        ProductName = arg[1],
                        ProductCatagory = arg[2],
                        ProductPrice = arg[4],
                        ProductStatus = arg[5],
                        ProductQuantity = arg[7],
                        ProductDesc = arg[8],
                        ProductBuyingPrice = arg[9],
                        ProductProfit = arg[10],
                        ProductTotalPrice = "0",
                        ProductTotalQuantity = "0",
                    });
                }
                allProducts.ProductCollection = orginalProduct;
                setDataContext();
            };
        }

        private void catagory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var a = new ObservableCollection<SaleModelProducts>();
            var selectedCatagory = (SaleModelProducts)catagory.SelectedItem;
            foreach (var item in orginalProduct)
            {
                if (item.ProductCatagory.Equals((string)selectedCatagory.Productid))
                {
                    a.Add(item);
                }
            }
            allProducts.ProductCollection = a;

        }

        private void Tile_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                var _id = ((Button)sender).Tag;
                foreach (var item in allProducts.ProductCollection)
                {
                    if (item.Productid.Equals(_id))
                    {
                        listBox.SelectedItem = item;
                    }
                }
                SaleModelProducts selected = (SaleModelProducts)listBox.SelectedItem;
                var totalQuantity = int.Parse(selected.ProductTotalQuantity); totalQuantity++;
                var totalPrice = double.Parse(selected.ProductPrice) * totalQuantity;
                selected.ProductTotalQuantity = totalQuantity.ToString();
                selected.ProductTotalPrice = totalPrice.ToString();
            }
            catch (Exception e1)
            {

                Debug.WriteLine(e1.Message);
            }

        }

        private void updateRecipt()
        {
            Sold.ProductCollection = new ObservableCollection<SaleModelProducts>();
            foreach (var item in orginalProduct)
            {
                if (int.Parse(item.ProductTotalQuantity) > 0)
                {
                    Flag=1;
                    Sold.ProductCollection.Add(item);
                }
            }
        }

        private void confirmOrder_TouchUp(object sender, TouchEventArgs e)
        {
            updateRecipt();
            ConfirmSalesOrder obj = new ConfirmSalesOrder();
            if (Flag==1)
            {
                NavigationService navigation = NavigationService.GetNavigationService(this);
                navigation.Navigate(new Uri("SalesOrderConfirm.xaml", UriKind.Relative));
            }
        }

        private async void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Cancel Order", "Are you sure you want to cancel this order?",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                webHandler.POS_delete_billAsync(new BillModel().Billid);
                webHandler.POS_delete_billCompleted += (x, y) =>
                {
                    NavigationService navigation = NavigationService.GetNavigationService(this);
                    navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                };

            }
        }

        private void flyoutlistbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var a = (ListBox)sender;
            //var b = (SaleModelProducts)a.SelectedItem;
            //if (!(b == null))
            //{
            //    var c = b.ProductName;
            //}
        }
    }
}

