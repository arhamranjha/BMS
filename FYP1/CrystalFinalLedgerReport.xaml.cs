using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
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
    /// Interaction logic for CrystalFinalLedgerReport.xaml
    /// </summary>
    public partial class CrystalFinalLedgerReport : MetroWindow
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private CustomerModel customer;
        private ReportDocument report = new ReportDocument();


        public CrystalFinalLedgerReport()
        {
            InitializeComponent();
        }
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            customer = new CustomerModel();
            new TaskFactory().StartNew(() =>
            {
                return POS_Get_Customer();

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



        private void aftermost()
        {
            try
            {
                ICLRRuntimeInfo rtInfo = (ICLRRuntimeInfo)RuntimeEnvironment.GetRuntimeInterfaceAsObject(Guid.Empty, typeof(ICLRRuntimeInfo).GUID);
                rtInfo.BindAsLegacyV2Runtime();
                string path = Directory.GetCurrentDirectory();
                var a = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                ReportDocument report = new ReportDocument();
                report.Load(a + @"/CrystalReport4.rpt");
                CustomerLedgerDataSet ds = new CustomerLedgerDataSet();
                DataTable dt1 = ds.BillProduct;
                DataTable dt2 = ds.Customer;
                foreach (var item in customer.CustomerCollection)
                {
                    dt2.Rows.Add(item.Customerid, item.CustomerName, item.CustomerLocation, item.Customerbalance, item.CustomerLastTally, item.Customerphone);
                }
                report.SetDataSource(ds);
                crystalReportsViewer2.ViewerCore.ReportSource = report;
            }
            catch (Exception e1)
            {
                Debug.WriteLine(e1.Message);
            }
        }

        private async void error()
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Something went wrong", "Try entring some other dates",
                MessageDialogStyle.Affirmative);
        }
        private ObservableCollection<CustomerModel> POS_Get_Customer()
        {
            var result = webHandler.POS_Get_Customer();
            if (result == null || result == "")
            {
                return null;
            }
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
            return customer.CustomerCollection;
        }


        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

        }
    }
}