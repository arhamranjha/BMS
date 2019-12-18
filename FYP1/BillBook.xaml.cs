using FYP1.Models;
using FYP1.POSServiceReference;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Globalization;
using System.Diagnostics;
using MahApps.Metro.Controls.Dialogs;
using MahApps.Metro.Controls;
using System.Printing;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for BillBook.xaml
    /// </summary>
    public partial class BillBook : Page
    {
        private CustomerModel customer = new CustomerModel();
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private int? resultout = -1;
        private int? resultout1 = -1;
        private BillModel bill;
        private Decimal BALANCE;
        private BillProductModel billProductModel;



        public BillBook()
        {
            InitializeComponent();
        }

        private async void backbutton(object sender, RoutedEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res1 = await win.ShowMessageAsync("Are you sure?...", " " +
                          MessageDialogStyle.Affirmative);
            if (res1 == MessageDialogResult.Affirmative)
            {
                webHandler.POS_delete_billAsync(new BillModel().Billid);
                webHandler.POS_delete_billCompleted += (x, y) =>
                {
                    NavigationService navigation = NavigationService.GetNavigationService(this);
                    navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                };
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bill = new BillModel();
            billProductModel = new BillProductModel();
            billProductModel.BillCollection = new ObservableCollection<BillProductModel>();
            POS_GET_CUSTOMER();
            POS_Start_Bill();
            gridSource.DataContext = billProductModel;
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
                invoiceno.Content = new BillModel().Billid.ToString();
                Date.Content = new BillModel().Current;
                billProductModel.Billid = new BillModel().Billid;
            };
            webHandler.POS_Add_Bill_ProductCompleted += async (x, y) =>
            {
                var win = (MetroWindow)Application.Current.MainWindow;
                resultout1 = y.result1;
                if (resultout1 == 1)
                {
                    var res1 = await win.ShowMessageAsync("All done...", " " +
                            MessageDialogStyle.Affirmative);
                    if (res1 == MessageDialogResult.Affirmative)
                    {
                        //Grid DynamicGrid = new Grid();

                        //ColumnDefinition gridCol1 = new ColumnDefinition();
                        //gridCol1.Width = new GridLength(7, GridUnitType.Star);
                        //ColumnDefinition gridCol2 = new ColumnDefinition();
                        //gridCol2.Width = new GridLength(30, GridUnitType.Star);
                        //ColumnDefinition gridCol3 = new ColumnDefinition();
                        //gridCol3.Width = new GridLength(60, GridUnitType.Star);
                        //DynamicGrid.ColumnDefinitions.Add(gridCol1);
                        //DynamicGrid.ColumnDefinitions.Add(gridCol2);
                        //DynamicGrid.ColumnDefinitions.Add(gridCol3);

                        //// Create Rows
                        //RowDefinition gridRow1 = new RowDefinition();
                        //gridRow1.Height = new GridLength(8, GridUnitType.Star);
                        //RowDefinition gridRow2 = new RowDefinition();
                        //gridRow2.Height = new GridLength(50, GridUnitType.Star);
                        //RowDefinition gridRow3 = new RowDefinition();
                        //gridRow3.Height = new GridLength(25, GridUnitType.Star);
                        //DynamicGrid.RowDefinitions.Add(gridRow1);
                        //DynamicGrid.RowDefinitions.Add(gridRow2);
                        //DynamicGrid.RowDefinitions.Add(gridRow3);

                        //Grid fields = fieldGrid;
                        //Grid.SetRow(fields, 0);
                        //Grid.SetColumn(fields, 0);
                        //Grid.SetColumnSpan(fields, 3);

                        //DynamicGrid.Children.Add(fields);

                        //Border border = griddataTable1DataGrid;
                        //Grid.SetRow(border, 1);
                        //Grid.SetColumn(border, 0);
                        //Grid.SetColumnSpan(border, 3);

                        //DynamicGrid.Children.Add(border);

                        //Grid cash = gridCash;
                        //Grid.SetRow(cash, 2);
                        //Grid.SetColumn(cash, 0);
                        //Grid.SetColumnSpan(cash, 3);

                        //DynamicGrid.Children.Add(cash);

                        //StackPanel stack = new StackPanel();
                        //stack.Children.Add(gridPrint);
                        //stack.Children.Add(gridPrint1);
                        gridPrint.SetValue(Grid.RowSpanProperty, 3);
                        gridPrint1.Visibility = Visibility.Visible;
                        PrintCharts(finalprint);
                        NavigationService navigation = NavigationService.GetNavigationService(this);
                        navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));

                    }
                }
                if (resultout1 == 0)
                {
                    var res1 = await win.ShowMessageAsync("Something is wrong...", " " +
                            MessageDialogStyle.Affirmative);
                    webHandler.POS_delete_billAsync(new BillModel().Billid);
                    webHandler.POS_delete_billCompleted += (x1, y1) =>
                    {
                        NavigationService navigation = NavigationService.GetNavigationService(this);
                        navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                    };
                }
            };
            webHandler.POS_Add_Bill_Product_ShipmentCompleted += async (x, y) =>
            {
                var win = (MetroWindow)Application.Current.MainWindow;
                resultout1 = y.result1;
                if (resultout1 == 1)
                {
                    var res1 = await win.ShowMessageAsync("All done...", " " +
                            MessageDialogStyle.Affirmative);
                    if (res1 == MessageDialogResult.Affirmative)
                    {
                        NavigationService navigation = NavigationService.GetNavigationService(this);
                        navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));

                    }
                }
                if (resultout1 == 0)
                {
                    var res1 = await win.ShowMessageAsync("Something is wrong...", " " +
                            MessageDialogStyle.Affirmative);
                    webHandler.POS_delete_billAsync(new BillModel().Billid);
                    webHandler.POS_delete_billCompleted += (x1, y1) =>
                    {
                        NavigationService navigation = NavigationService.GetNavigationService(this);
                        navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                    };
                }
            };
        }

        private void PrintCharts(Grid grid)
        {
            PrintDialog print = new PrintDialog();
            if (print.ShowDialog() == true)
            {
                //print.PrintTicket.PageOrientation = PageOrientation.Landscape;
                PrintCapabilities capabilities = print.PrintQueue.GetPrintCapabilities(print.PrintTicket);
                print.PrintTicket.OutputColor = OutputColor.Grayscale;

                double scale = Math.Min((capabilities.PageImageableArea.ExtentWidth / grid.ActualWidth),
                                        (capabilities.PageImageableArea.ExtentHeight / grid.ActualHeight));

                Transform oldTransform = grid.LayoutTransform;

                grid.LayoutTransform = new ScaleTransform(scale, scale);

                Size oldSize = new Size(grid.ActualWidth, grid.ActualHeight);
                Size sz = new Size((capabilities.PageImageableArea.ExtentWidth), (capabilities.PageImageableArea.ExtentHeight));
                grid.Measure(sz);
                ((UIElement)grid).Arrange(new Rect(new Point(capabilities.PageImageableArea.OriginWidth, capabilities.PageImageableArea.OriginHeight),
                    sz));

                print.PrintVisual(grid, "Print Results");
                grid.LayoutTransform = oldTransform;
                grid.Measure(oldSize);

                ((UIElement)grid).Arrange(new Rect(new Point(0, 0),
                    oldSize));
                gridPrint.SetValue(Grid.RowSpanProperty, 3);
                gridPrint1.Visibility = Visibility.Visible;
            }
        }

        private void POS_Start_Bill()
        {
            webHandler.POS_Start_BillAsync(resultout, new LoggedInEmployeeModel().EmployeeId);
        }

        private void POS_GET_CUSTOMER()
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
                combo.ItemsSource = customer.CustomerCollection;

            };
        }

        private void DataGrid_GotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);


            }
        }
        void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {

                bill.PaymentMethod = (sender as RadioButton).Content.ToString();

            }
            catch (Exception e1)
            {

                Debug.WriteLine(e1.Message);
            }
        }
        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
        {
            try
            {
                bill.PaymentMethod = "";

            }
            catch (Exception e1)
            {

                Debug.WriteLine(e1.Message);
            }
        }

        private void tenterd_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                billProductModel.Customerid = int.Parse(((CustomerModel)((sender as ComboBox).SelectedItem)).Customerid);
                BALANCE = Decimal.Parse(((CustomerModel)((sender as ComboBox).SelectedItem)).Customerbalance);
            }
            catch (Exception e1)
            {
                Debug.Write(e1.Message);
            }
        }

        private void dataTable1DataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            bill.Total = billProductModel.BillCollection.Sum(x => x.TotalPerItem);
            total.Content = bill.Total + " Rs";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Focus();
            try
            {
                string data = "";
                decimal paid = 0; string method = "";
                if (billProductModel.Customerid != 0 && billProductModel.BillCollection != null && bill != null)
                {
                    foreach (var item in billProductModel.BillCollection)
                    {
                        BALANCE = BALANCE + item.TotalPerItem;
                        data += billProductModel.Billid + "," + billProductModel.Customerid + "," + item.Quantity + "," + item.ProductName + "," + item.Rate + "," + item.TotalPerItem + "," + "Dr," + BALANCE + ";";
                    }
                }
                if ((bool)rightcheck.IsChecked)
                {

                    if (paymentamount.Text != null && bill.PaymentMethod != null && decimal.Parse(paymentamount.Text) != 0)
                    {
                        paid = Decimal.Parse(paymentamount.Text);
                        BALANCE = BALANCE - paid;
                        method = bill.PaymentMethod;
                        data += billProductModel.Billid + "," + billProductModel.Customerid + "," + "0" + "," + bill.PaymentMethod + "," + "0" + "," + paid + "," + "Cr," + BALANCE + ";";
                    }
                    webHandler.POS_Update_Customer_BalanceAsync(billProductModel.Customerid, BALANCE, 1);
                    webHandler.POS_Update_Bill(billProductModel.Billid, bill.Total, billProductModel.Customerid, paid, method);
                    webHandler.POS_Add_Bill_ProductAsync(resultout1, data);
                }

                else if ((bool)leftcheck.IsChecked)
                {

                    if (paymentamountship.Text != null && bill.PaymentMethod != null && int.Parse(paymentamountship.Text) != 0)
                    {
                        paid = Decimal.Parse(paymentamountship.Text);
                        BALANCE = BALANCE - paid;
                        method = bill.PaymentMethod;
                        data += billProductModel.Billid + "," + billProductModel.Customerid + "," + "0" + "," + bill.PaymentMethod + "," + "0" + "," + paid + "," + "Cr," + BALANCE + ";";
                    }
                    webHandler.POS_Update_Bill(billProductModel.Billid, bill.Total, billProductModel.Customerid, paid, method);
                    webHandler.POS_Add_Bill_Product_ShipmentAsync(resultout1, data);
                }
                else
                {
                    webHandler.POS_Update_Customer_BalanceAsync(billProductModel.Customerid, BALANCE, 1);
                    webHandler.POS_Update_Bill(billProductModel.Billid, bill.Total, billProductModel.Customerid, 0, "");
                    webHandler.POS_Add_Bill_ProductAsync(resultout1, data);
                }
            }
            catch (Exception e1)
            {
                Debug.Write(e1.Message);
            }
        }



        private void Button_Click_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                billProductModel.BillCollection.RemoveAt(dataTable1DataGrid.SelectedIndex);
            }
            catch (Exception)
            {

            }
        }
    }
}
