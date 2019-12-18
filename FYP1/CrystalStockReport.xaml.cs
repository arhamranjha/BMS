using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using CrystalDecisions.CrystalReports.Engine;
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CrystalStockReport1 : MetroWindow
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private ProductsModel product;
        ReportDocument report = new ReportDocument();
        public CrystalStockReport1()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            product = new ProductsModel();
            POS_Product();
            this.Cursor = Cursors.Arrow;
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
                try
                {
                    ICLRRuntimeInfo rtInfo = (ICLRRuntimeInfo)RuntimeEnvironment.GetRuntimeInterfaceAsObject(Guid.Empty, typeof(ICLRRuntimeInfo).GUID);
                    rtInfo.BindAsLegacyV2Runtime();
                    report.Load("../../CrystalReport1.rpt");
                    report.SetDataSource(from c in product.ProductsCollection
                                         select new { c.Productid, c.ProductName, c.ProductCatagoryDesc, c.ProductPrice, c.ProductStatusDesc, c.ProductBuyingPrice, c.ProductQuantity });
                    crystalReportsViewer1.ViewerCore.ReportSource = report;
                }
                catch (Exception e1)
                {

                    Debug.WriteLine("Kush hua hai ");
                    Debug.WriteLine(e1.Message);
                }


                //ReportDocument report2 = new ReportDocument();
                //report2.Load("../../CrystalReport2.rpt");

                //report2.SetDataSource(from c in product.ProductsCollection
                //                     select new { c.Productid, c.ProductName, c.ProductCatagoryDesc, c.ProductPrice, c.ProductStatusDesc, c.ProductBuyingPrice, c.ProductQuantity });
                //crystalReportsViewer2.ViewerCore.ReportSource = report2;

            };
        }

        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            this.Close();
        }
    }
}
