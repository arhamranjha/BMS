using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CrystalSaleReport1 : MetroWindow
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private ReportSaleModel product;
        ReportDocument report = new ReportDocument();
        DateTime DFS, DTS;
        public CrystalSaleReport1(DateTime DFS, DateTime DTS)
        {
            InitializeComponent();
            this.DTS = DTS;
            this.DFS = DFS;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            product = new ReportSaleModel();
            this.Cursor = Cursors.Arrow;
            new TaskFactory().StartNew(() =>
            {
                return POS_Sale(DFS, DTS);
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
                report.Load("../../CrystalReport2.rpt");
                report.SetDataSource(from c in product.SaleReportCollection
                                     select new { c.Billid, c.Current_DateTime, c.Employee_Name, c.Product_id, c.Product_Name, c.Product_Quantity, c.Optionid, c.Optionname, c.OptionQty, c.Total, DFS, DTS, new LoggedInEmployeeModel().FullName });
                crystalReportsViewer2.ViewerCore.ReportSource = report;
            }
            catch (Exception e1)
            {

                Debug.WriteLine("Kush hua hai ");
                Debug.WriteLine(e1.Message);
            }
        }

        private async void error()
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Something went wrong", "Try entring some other dates",
                MessageDialogStyle.Affirmative);
        }

        private ObservableCollection<ReportSaleModel> POS_Sale(DateTime dF, DateTime dT)
        {
            var a = String.Format("{0:yyyy/M/d}", dF);
            var b = String.Format("{0:yyyy/M/d}", dT);
            var result = webHandler.POS_Report_Sale(a, b);
            if (result == null || result == "")
            {
                return null;
            }

            product.SaleReportCollection = new ObservableCollection<ReportSaleModel>();
            string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var item in arr)
            {
                string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                product.SaleReportCollection.Add(new ReportSaleModel()
                {
                    Orderid = arg[0],
                    Billid = arg[1],
                    Current_DateTime = arg[2],
                    Employee_id = arg[3],
                    Employee_Name = arg[4],
                    Product_id = arg[5],
                    Product_Name = arg[6],
                    Product_Quantity = arg[7],
                    Optionid = arg[8],
                    Optionname = arg[9],
                    OptionQty = arg[10],
                    Total = Decimal.Parse(arg[11]),
                });
            }
            return product.SaleReportCollection;
        }

        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            this.Close();
        }
    }
}
