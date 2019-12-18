using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using CrystalDecisions.CrystalReports.Engine;
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for CrystalCustomerLedgerReport.xaml
    /// </summary>
    public partial class CrystalCustomerLedgerReport : MetroWindow
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private CustomerModel customer = new CustomerModel();
        private ReportDocument report = new ReportDocument();
        private BillProductModel billProductModel;
        private DateTime DFS, DTS;
        private CustomerModel selectedCustomer;


        public CrystalCustomerLedgerReport(CustomerModel selectedCustomer, DateTime DFS, DateTime DTS)
        {
            InitializeComponent();
            this.selectedCustomer = selectedCustomer;
            this.DTS = DTS;
            this.DFS = DFS;
        }
        public CrystalCustomerLedgerReport(CustomerModel selectedCustomer)
        {
            InitializeComponent();
            this.selectedCustomer = selectedCustomer;

        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            billProductModel = new BillProductModel();
            new TaskFactory().StartNew(() =>
            {
                if (selectedCustomer != null)
                {
                    if (DFS > DateTime.MinValue && DTS > DateTime.MinValue)
                    {
                        return POS_Customer_Ledger(selectedCustomer, DFS, DTS);
                    }
                    else
                    {
                        return POS_Customer_Ledger(selectedCustomer);
                    }
                }
                return null;
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
        }



        private async void aftermost()
        {
            try
            {
                ICLRRuntimeInfo rtInfo = (ICLRRuntimeInfo)RuntimeEnvironment.GetRuntimeInterfaceAsObject(Guid.Empty, typeof(ICLRRuntimeInfo).GUID);
                rtInfo.BindAsLegacyV2Runtime();

                var a = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                ReportDocument report = new ReportDocument();
                report.Load(a + @"/CrystalReport3.rpt");


                CustomerLedgerDataSet ds = new CustomerLedgerDataSet();
                DataTable dt1 = ds.BillProduct;
                DataTable dt2 = ds.Customer;

                // assume columns have been created
                foreach (var c in billProductModel.BillCollection)
                {
                    dt1.Rows.Add(c.DateTime, c.Billid, c.Quantity, c.ProductName, c.Rate, c.TotalAmount, c.Status, c.Balance);
                }

                dt2.Rows.Add(selectedCustomer.Customerid, selectedCustomer.CustomerName, selectedCustomer.CustomerLocation, selectedCustomer.Customerbalance, selectedCustomer.CustomerLastTally, selectedCustomer.Customerphone);

                report.SetDataSource(ds);

                crystalReportsViewer2.ViewerCore.ReportSource = report;
            }
            catch (Exception e1)
            {
                var win = (MetroWindow)Application.Current.MainWindow;
                var res1 = await win.ShowMessageAsync(" source: " + e1.Source, e1.Message + "  ||   " + e1.StackTrace +
                        MessageDialogStyle.Affirmative);
                Debug.WriteLine(e1.Message);
            }
        }

        private async void error()
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Something went wrong", "Try entring some other dates",
                MessageDialogStyle.Affirmative);
        }
        private ObservableCollection<BillProductModel> POS_Customer_Ledger(CustomerModel selectedCustomer)
        {
            var result = webHandler.POS_Get_Customer_Ledger(int.Parse(selectedCustomer.Customerid));
            if (result == null || result == "")
            {
                return null;
            }
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
            return billProductModel.BillCollection;

        }

        private ObservableCollection<BillProductModel> POS_Customer_Ledger(CustomerModel selectedCustomer, DateTime dFS, DateTime dTS)
        {
            var a = String.Format("{0:yyyy/M/d}", dFS);
            var b = String.Format("{0:yyyy/M/d}", dTS);
            var result = webHandler.POS_Get_Customer_Ledger_FilterDate(int.Parse(selectedCustomer.Customerid), a, b);
            if (result == null || result == "")
            {
                return null;
            }
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
            return billProductModel.BillCollection;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
