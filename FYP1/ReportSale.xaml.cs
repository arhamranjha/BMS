using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Threading;
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for ReportSale.xaml
    /// </summary>
    public partial class ReportSale : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private ReportSaleModel saleReport;
        private ReportSaleModel saleReport1;
        private ObservableCollection<ReportSaleModel> PortfolioInfo;
        private ObservableCollection<ReportSaleModel> PortfolioInfo1;
        private ReportSaleGraphModel most;
        private ReportSaleGraphModel emp;
        private ReportSaleGraphModel qty;
        private ItemViewModel _itemViewModel = new ItemViewModel();
        private DateModel _dates = new DateModel();
        ICollectionView view;
        private DateTime DFS, DTS;

        private Decimal sum = 0;

        public ReportSale()
        {
            InitializeComponent();
            total.DataContext = _itemViewModel;
            fly.DataContext = _itemViewModel;
            _dates.DateFrom = DateTime.Now;
            _dates.DateTo = DateTime.Now;
            flyout.DataContext = _dates;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            most = new ReportSaleGraphModel();
            emp = new ReportSaleGraphModel();
            qty = new ReportSaleGraphModel();
            saleReport = new ReportSaleModel();
            saleReport1 = new ReportSaleModel();
        }
        private ReportSaleGraphModel POS_qty(DateTime DF, DateTime DT)
        {//"2016/05/10", "2016/06/10"
            qty = new ReportSaleGraphModel();
            var a = String.Format("{0:yyyy/M/d}", DF);
            var b = String.Format("{0:yyyy/M/d}", DT);
            var result = webHandler.POS_Product_QtySold(a, b);
            if (result == null || result == "")
            {
                return null;
            }

            qty.GraphCollection = new ObservableCollection<ReportSaleGraphModel>();
            string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                qty.GraphCollection.Add(new ReportSaleGraphModel()
                {
                    Graphname = arg[0],
                    Graphvalue = arg[1],

                });
            }
            return qty;
        }
        private ReportSaleGraphModel POS_emp(DateTime DF, DateTime DT)
        {//"2016/05/10", "2016/06/10"
            emp = new ReportSaleGraphModel();
            var a = String.Format("{0:yyyy/M/d}", DF);
            var b = String.Format("{0:yyyy/M/d}", DT);
            var result = webHandler.POS_Employeer_selling(a, b);
            if (result == null || result == "")
            {
                return null;
            }

            emp.GraphCollection = new ObservableCollection<ReportSaleGraphModel>();
            string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                emp.GraphCollection.Add(new ReportSaleGraphModel()
                {
                    Graphname = arg[0],
                    Graphvalue = arg[1],

                });
            }
            return emp;
        }
        private ReportSaleGraphModel POS_most(DateTime DF, DateTime DT)
        {//"2016/05/10", "2016/06/10"
            most = new ReportSaleGraphModel();
            var a = String.Format("{0:yyyy/M/d}", DF);
            var b = String.Format("{0:yyyy/M/d}", DT);
            var result = webHandler.POS_Most_sold(a, b);
            if (result == null || result == "")
            {
                return null;
            }

            most.GraphCollection = new ObservableCollection<ReportSaleGraphModel>();
            string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                most.GraphCollection.Add(new ReportSaleGraphModel()
                {
                    Graphname = arg[0],
                    Graphvalue = arg[1],

                });
            }
            return most;
        }
        private ObservableCollection<ReportSaleModel> POS_Report_SaleTest1(DateTime DF, DateTime DT)
        {//"2016/05/10", "2016/06/10"
            var a = String.Format("{0:yyyy/M/d}", DF);
            var b = String.Format("{0:yyyy/M/d}", DT);
            var result = webHandler.POS_Report_Sale_test1(a, b);
            if (result == null || result == "")
            {
                return null;
            }

            saleReport.SaleReportCollection = new ObservableCollection<ReportSaleModel>();
            string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                saleReport.SaleReportCollection.Add(new ReportSaleModel()
                {
                    Orderid = arg[0],
                    Billid = arg[1],
                    Current_DateTime = arg[2],
                    Employee_Name = arg[3],
                    Total = (Decimal.Parse(arg[4])),
                    Product_id = arg[5],
                    Product_Name = arg[6],
                    Product_Quantity = arg[7],
                    Optionid = arg[8],
                    Optionname = arg[9],

                });
            }

            PortfolioInfo = new ObservableCollection<ReportSaleModel>();
            var selectedItem = from p in saleReport.SaleReportCollection group p by p.Billid;
            foreach (var item in selectedItem)
            {
                var key = item.Key;
                ObservableCollection<ReportSaleModel> liss = new ObservableCollection<ReportSaleModel>();
                string name1 = "";
                Decimal name2 = 0;
                string name3 = "";
                foreach (var itemkeys in item)
                {
                    name1 = itemkeys.Current_DateTime;
                    name2 = itemkeys.Total;
                    name3 = itemkeys.Employee_Name;

                    liss.Add(new ReportSaleModel()
                    {
                        Billid = itemkeys.Optionid,
                        Product_id = itemkeys.Product_id,
                        Product_Quantity = itemkeys.Product_Quantity,
                        Product_Name = itemkeys.Product_Name,
                        Optionname = itemkeys.Optionname
                    });
                }
                PortfolioInfo.Add(new ReportSaleModel() { Billid = key, Employee_Name = name3, Current_DateTime = name1, Total = name2, SaleReportCollection = liss });
            }
            return addThemToghter(a, b);
        }

        private ObservableCollection<ReportSaleModel> addThemToghter(string DF, string DT)
        {
            var result = webHandler.POS_Report_Sale_test2(DF, DT);
            saleReport1.SaleReportCollection = new ObservableCollection<ReportSaleModel>();
            string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                saleReport1.SaleReportCollection.Add(new ReportSaleModel()
                {
                    Billid = arg[1],
                    Current_DateTime = arg[2],
                    Product_id = arg[3],
                    Optionid = arg[4],
                    Optionname = arg[5],
                    OptionQty = arg[6],
                    Total = Decimal.Parse(arg[7]),
                });
            }

            var selectedItem = from p in saleReport1.SaleReportCollection group p by p.Billid;
            PortfolioInfo1 = new ObservableCollection<ReportSaleModel>();

            foreach (var item in selectedItem)
            {
                var key = item.Key;
                ObservableCollection<ReportSaleModel> liss = new ObservableCollection<ReportSaleModel>();
                foreach (var itemkeys in item)
                {
                    liss.Add(new ReportSaleModel()
                    {
                        Optionid = itemkeys.Optionid,
                        Product_id = itemkeys.Product_id,
                        Optionname = itemkeys.Optionname,
                        OptionQty = itemkeys.OptionQty,
                        Total = itemkeys.Total
                    });
                }
                PortfolioInfo1.Add(new ReportSaleModel() { Billid = key, SaleReportCollection = liss });
            }
            foreach (var item in PortfolioInfo)
            {
                var a = item.Billid;
                foreach (var item1 in PortfolioInfo1)
                {
                    if (a == item1.Billid)
                    {
                        item.SaleReportCollection1 = item1.SaleReportCollection;
                    }
                }
            }
            return PortfolioInfo;
        }

        private void Sum()
        {
            sum = 0;
            foreach (var item in PortfolioInfo)
            {
                sum += item.Total;
            }
            _itemViewModel.Total = Double.Parse(sum.ToString());
        }

        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Reports.xaml", UriKind.Relative));
        }

        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (productList.SelectedItem != null)
                {
                    var a = (ReportSaleModel)productList.SelectedItem;
                    productList1.ItemsSource = a.SaleReportCollection;
                    productList2.ItemsSource = a.SaleReportCollection1;
                }
                return;
            }
            catch (Exception e1)
            {
                Debug.Write(e1.StackTrace);

            }

        }



        private void clen_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = true;
        }

        private void addnew_TouchDown(object sender, TouchEventArgs e)
        {
            DFS = _dates.DateFrom;
            DTS = _dates.DateTo;
            GetData(_dates.DateFrom, _dates.DateTo);

        }

        private async void error()
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Something went wrong", "Try entring some other dates",
                MessageDialogStyle.Affirmative);
            progressbar.Visibility = Visibility.Hidden;
        }

        private void after()
        {
            gridSource.DataContext = PortfolioInfo;
            Sum();
            progressbar.Visibility = Visibility.Hidden;
            view = CollectionViewSource.GetDefaultView(PortfolioInfo);
        }

        private void aftermost()
        {
            chart.ItemsSource = most.GraphCollection;
        }
        private void afteremp()
        {
            chart1.ItemsSource = emp.GraphCollection;
        }
        private void afterqty()
        {
            chart2.ItemsSource = qty.GraphCollection;
        }


        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (dates.SelectedIndex)
            {
                case 0:
                    DFS = _dates.Today;
                    DTS = DateTime.Now.AddDays(1);
                    GetData(_dates.Today, DateTime.Now.AddDays(1));
                    break;
                case 1:
                    DFS = _dates.Yesterday;
                    DTS = DateTime.Now;
                    GetData(_dates.Yesterday, DateTime.Now);
                    break;
                case 2:
                    DFS = _dates.Lastweek;
                    DTS = DateTime.Now.AddDays(1);
                    GetData(_dates.Lastweek, DateTime.Now.AddDays(1));
                    break;
                case 3:
                    DFS = _dates.Lastmonth;
                    DTS = DateTime.Now.AddDays(1);
                    GetData(_dates.Lastmonth, DateTime.Now.AddDays(1));
                    break;
                default:
                    DFS = _dates.Today;
                    DTS = DateTime.Now.AddDays(1);
                    GetData(_dates.Today, DateTime.Now.AddDays(1));
                    break;
            }
        }
        //Get Aysnc Values
        private void GetData(DateTime DF, DateTime DT)
        {
            progressbar.Visibility = Visibility.Visible;
            new TaskFactory().StartNew(() =>
            {
                return POS_Report_SaleTest1(DF, DT);
            })
            .ContinueWith(x =>
            {
                if (x.Result == null)
                {
                    Dispatcher.Invoke(new Action(() => { error(); }), DispatcherPriority.ContextIdle);
                    return;
                }
                Dispatcher.Invoke(new Action(() => { after(); }), DispatcherPriority.ContextIdle);
            });

        }

        private void GetGraph(DateTime DF, DateTime DT)
        {
            new TaskFactory().StartNew(() =>
            {
                return POS_most(DF, DT);
            })
           .ContinueWith(x =>
           {
               if (x.Result == null)
               {
                   Dispatcher.Invoke(new Action(() => { error(); }), DispatcherPriority.ContextIdle);
                   return;
               }
               Dispatcher.Invoke(new Action(() => { aftermost(); }), DispatcherPriority.ContextIdle);
           });
            new TaskFactory().StartNew(() =>
            {
                return POS_emp(DF, DT);
            })
       .ContinueWith(x =>
       {
           if (x.Result == null)
           {
               Dispatcher.Invoke(new Action(() => { error(); }), DispatcherPriority.ContextIdle);
               return;
           }
           Dispatcher.Invoke(new Action(() => { afteremp(); }), DispatcherPriority.ContextIdle);
       });
            new TaskFactory().StartNew(() =>
            {
                return POS_qty(DF, DT);
            })
       .ContinueWith(x =>
       {
           if (x.Result == null)
           {
               Dispatcher.Invoke(new Action(() => { error(); }), DispatcherPriority.ContextIdle);
               return;
           }
           Dispatcher.Invoke(new Action(() => { afterqty(); }), DispatcherPriority.ContextIdle);
       });
        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && view != null)
            {
                view.Filter += delegate (object cust) { return ((ReportSaleModel)cust).Employee_Name.ToLower().Contains(search.Text.ToLower()); };
            }
            else if (search.Text == string.Empty)
            {
                view.Filter = null;
            }
        }

        private void DatePicker_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void printButton_TouchUp(object sender, TouchEventArgs e)
        {
            if (!(DFS == DateTime.MinValue || DTS == DateTime.MinValue))
            {
                CrystalSaleReport1 win2 = new CrystalSaleReport1(DFS, DTS);
                win2.Show();
            }

        }

        private async void tab_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var a = tab.SelectedIndex;
                if (a == 1)
                {
                    if (PortfolioInfo != null)
                    {
                        GetGraph(DFS, DTS);
                    }
                    else
                    {
                        var win = (MetroWindow)Application.Current.MainWindow;
                        var res = await win.ShowMessageAsync("Something went wrong", "Please select some dates to view graph",
                            MessageDialogStyle.Affirmative);
                    }
                }
            }
            catch (Exception e1)
            {

                Debug.Write(e1.Message);
            }
        }
    }
}
