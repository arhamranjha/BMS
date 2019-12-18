using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace FYP1
{
    /// <summary>
    /// Interaction logic for ReportCustomer.xaml
    /// </summary>
    public partial class ReportCustomer : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private CustomerModel customer;
        private CustomerReportModel bill;
        private ItemViewModel _itemViewModel = new ItemViewModel();

        public ReportCustomer()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            customer = new CustomerModel();
            bill = new CustomerReportModel();
            POS_Customer();
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
                        Customerbalance = arg[2]

                    });
                }
                gridSource.DataContext = customer.CustomerCollection;
            };
        }
        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
        }
        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            //if (search.Text != string.Empty && bindlistview != null)
            //{
            //    bindlistview.Filter = delegate (object cust) { return ((CustomerModel)cust).CustomerName.ToLower().Contains(search.Text.ToLower()); };
            //}
            //else if (search.Text == string.Empty)
            //{
            //    bindlistview.Filter = null;
            //}
        }
        private void customerbill_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void customerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                webHandler.POS_Get_CustomerBillAsync(int.Parse(((CustomerModel)(((ListBox)sender).SelectedItem)).Customerid));
                webHandler.POS_Get_CustomerBillCompleted += (x, y) =>
                {
                    var result = y.Result;
                    bill.CustomerCollection = new ObservableCollection<CustomerReportModel>();
                    string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in arr)
                    {
                        string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                        bill.CustomerCollection.Add(new CustomerReportModel()
                        {
                            Billid = int.Parse(arg[0]),
                            Current = (DateTime.ParseExact(arg[1], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture).Date).ToString("dd/MM/yyyy"),
                            EmployeeId = int.Parse(arg[2]),
                            Total = decimal.Parse(arg[3]),
                          

                        });
                    }
                    lvUsers.ItemsSource = bill.CustomerCollection;
                    CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lvUsers.ItemsSource);
                    PropertyGroupDescription groupDescription = new PropertyGroupDescription("Current");
                    view.GroupDescriptions.Add(groupDescription);
                };

            }
            catch (Exception ex)
            {

                Debug.WriteLine(ex.Message);
            }
        }
    }
}
