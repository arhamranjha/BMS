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
    /// Interaction logic for RetailCustomer.xaml
    /// </summary>
    public partial class RetailCustomer : Page
    {

        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private CustomerModel customer;
        private CustomerModel selected = new CustomerModel();
        private CustomerModel newcustomer = new CustomerModel();
        private CollectionViewSource CVS;
        private ListCollectionView bindlistview;
        private ItemViewModel _itemViewModel = new ItemViewModel();
        private StatusModel dropDownStatus = new StatusModel();
        private int count;
        private int count1;
        public RetailCustomer()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            customer = (CustomerModel)this.FindResource("pro");
            POS_Customer();
            //Drop_Down_Status();
            fly.DataContext = _itemViewModel;
            flyout.DataContext = newcustomer;
            webHandler.POS_Add_CustomerCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("New Customer added", "New customer was succefully added",
                        MessageDialogStyle.Affirmative);
                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_Update_CustomerCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Customer changes saved", "",
                        MessageDialogStyle.Affirmative);

                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_Delete_CustomerCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Customer removed", "The Customer was removed",
                        MessageDialogStyle.Affirmative);

                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };


        }


        private void POS_Customer()
        {
            webHandler.POS_Get_CustomerAsync();
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
                CVS = ((CollectionViewSource)(this.FindResource("cvs1")));
                bindlistview = CVS.View as ListCollectionView;
            };
        }
        private void SearchBox_KeyUp(object sender, RoutedEventArgs e)
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

        private void backButton_TouchUp(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                update.IsEnabled = false; update.Foreground = Brushes.Gray;
                count++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count--;
                if (count == 0)
                {
                    update.IsEnabled = true; update.Foreground = (Brush)this.FindResource("AccentColorBrush");

                }
            }
        }

        private void TextBox_Error_1(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addnew.IsEnabled = false; addnew.Foreground = Brushes.Gray;
                count1++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count1--;
                if (count1 == 0)
                {
                    addnew.IsEnabled = true;
                    addnew.Foreground = (Brush)this.FindResource("AccentColorBrush");
                }
            }
        }
        private async void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selected = (CustomerModel)productList.SelectedItem;
                if (selected != null)
                {
                    update.IsEnabled = true;
                    del.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                update.IsEnabled = false;
                del.IsEnabled = false;
                update.Foreground = Brushes.Gray;
                del.Foreground = Brushes.Gray;
                var win = (MetroWindow)Application.Current.MainWindow;
                var res = await win.ShowMessageAsync("Something went wrong", "Try again in a little bit",
                    MessageDialogStyle.AffirmativeAndNegative);
            }
        }
        private void add_TouchDown(object sender, RoutedEventArgs e)
        {
            //newproduct = new ProductsModel();
            newcustomer.CustomerName = ""; newcustomer.Customerbalance = ""; newcustomer.Customeraddress = ""; newcustomer.CustomerLocation = ""; newcustomer.CustomerLastTally = ""; newcustomer.Customerphone = "";
            newcustomer.CustomerLastTally = DateTime.Now.ToString();
            _itemViewModel.Open = true;
        }

        private void update_TouchDown(object sender, RoutedEventArgs e)
        {
            webHandler.POS_Update_CustomerAsync(int.Parse(selected.Customerid), selected.CustomerName, Decimal.Parse(selected.Customerbalance), selected.CustomerLocation, selected.CustomerLastTally, (selected.Customerphone), (selected.Customeraddress));

        }

        private async void del_TouchDown(object sender, RoutedEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Are you sure?", "This " + selected.CustomerName + " Customer will be deleted",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                webHandler.POS_Delete_CustomerAsync(int.Parse(selected.Customerid));
                customer.CustomerCollection.Remove(selected);

            }
        }
        private void addnew_TouchDown(object sender, RoutedEventArgs e)
        {
            webHandler.POS_Add_CustomerAsync(newcustomer.CustomerName, Decimal.Parse(newcustomer.Customerbalance), newcustomer.CustomerLocation, newcustomer.CustomerLastTally, (newcustomer.Customerphone), (newcustomer.Customeraddress));
            customer.CustomerCollection.Add(new CustomerModel()
            {
                CustomerName = newcustomer.CustomerName,
                Customerbalance = newcustomer.Customerbalance,
                CustomerLocation = newcustomer.CustomerLocation,
                CustomerLastTally = newcustomer.CustomerLastTally,
                Customerphone = newcustomer.Customerphone,
                Customeraddress = newcustomer.Customeraddress,

            });
            _itemViewModel.Open = false;

        }

        private void cancelnew_TouchDown(object sender, RoutedEventArgs e)
        {
            _itemViewModel.Open = false;
            newcustomer.CustomerName = ""; newcustomer.Customerbalance = ""; newcustomer.Customeraddress = ""; newcustomer.Customerphone = ""; newcustomer.CustomerLocation = ""; newcustomer.CustomerLastTally = "";
        }
    }
}
