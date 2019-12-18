using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using FYP1.Models;
using FYP1.POSServiceReference;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for CustomerLedger.xaml
    /// </summary>
    public partial class CustomerLedger : Page
    {
        private CustomerModel customer = new CustomerModel();
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private BillProductModel billProductModel;
        private DateModel dateModel;
        private CustomerModel selectedCustomer;
        private int count;


        public CustomerLedger()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            billProductModel = new BillProductModel();
            dateModel = new DateModel();
            POS_GET_CUSTOMER();
            flyout.DataContext = dateModel;

            ///---///
            #region Completed
            webHandler.POS_Get_Customer_LedgerCompleted += (x, y) =>
            {
                var result = y.Result;
                billProductModel.BillCollection = new ObservableCollection<BillProductModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    billProductModel.BillCollection.Add(new BillProductModel()
                    {
                        DateTime = (DateTime.ParseExact(arg[0], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)),
                        Billid = int.Parse(arg[1]),
                        Quantity = int.Parse(arg[2]),
                        ProductName = arg[3],
                        Rate = Decimal.Parse(arg[4]),
                        TotalAmount = Decimal.Parse(arg[5]),
                        Status = arg[6],
                        Balance = Decimal.Parse(arg[7]),
                    });
                }
                dataTable1DataGrid.ItemsSource = billProductModel.BillCollection;

            };
            webHandler.POS_Get_Customer_Ledger_FilterDateCompleted += (x, y) =>
            {
                var result = y.Result;
                billProductModel.BillCollection.Clear();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    billProductModel.BillCollection.Add(new BillProductModel()
                    {
                        DateTime = (DateTime.ParseExact(arg[0], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)),
                        Billid = int.Parse(arg[1]),
                        Quantity = int.Parse(arg[2]),
                        ProductName = arg[3],
                        Rate = Decimal.Parse(arg[4]),
                        TotalAmount = Decimal.Parse(arg[5]),
                        Status = arg[6],
                        Balance = Decimal.Parse(arg[7]),
                    });
                }
            };
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
                combo.ItemsSource = customer.CustomerCollection;
            };
            #endregion End
        }

        private void POS_GET_CUSTOMER()
        {
            webHandler.POS_Get_CustomerAsync();
        }
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selectedCustomer = (CustomerModel)((sender as ComboBox).SelectedItem);
                webHandler.POS_Get_Customer_LedgerAsync(int.Parse(selectedCustomer.Customerid));
            }
            catch (Exception e1)
            {
                Debug.Write(e1.Message);
            }
        }
        private void backbutton(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
        }


        private void print_Click(object sender, RoutedEventArgs e)
        {
            if (selectedCustomer != null)
            {
                if (dateModel.DateFrom > DateTime.MinValue && dateModel.DateTo > DateTime.MinValue)
                {
                    CrystalCustomerLedgerReport win2 = new CrystalCustomerLedgerReport(selectedCustomer, dateModel.DateFrom, dateModel.DateTo);
                    win2.Show();
                }
                else
                {
                    CrystalCustomerLedgerReport win2 = new CrystalCustomerLedgerReport(selectedCustomer);
                    win2.Show();
                }
            }
        }


        private void filter_click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dateModel.DateFrom > DateTime.MinValue && dateModel.DateTo > DateTime.MinValue && selectedCustomer != null)
                {
                    webHandler.POS_Get_Customer_Ledger_FilterDateAsync(int.Parse(selectedCustomer.Customerid), dateModel.DateFrom.ToString("M/d/yyyy"), dateModel.DateTo.ToString("M/d/yyyy"));
                }
            }
            catch (Exception)
            {


            }

        }

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                filter.IsEnabled = false; filter.Background = Brushes.Gray;
                count++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count--;
                if (count == 0)
                {
                    filter.IsEnabled = true; filter.Background = Brushes.Gray;
                }
            }
        }

        private void DatePicker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            if (selectedCustomer != null)
            {
                webHandler.POS_Update_CustomerAsync(int.Parse(selectedCustomer.Customerid), selectedCustomer.CustomerName, Decimal.Parse(selectedCustomer.Customerbalance), selectedCustomer.CustomerLocation, selectedCustomer.CustomerLastTally, (selectedCustomer.Customerphone), (selectedCustomer.Customeraddress));

            }
        }
    }
}
