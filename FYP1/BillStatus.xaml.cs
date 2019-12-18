using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for BillStatus.xaml
    /// </summary>
    public partial class BillStatus : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private GetBillModel bill;
        private BillProductModel billProductModel;
        private int? resultout1 = -1;
        private Decimal BALANCE;
        private GetBillModel SelectedBill;

        public BillStatus()
        {
            InitializeComponent();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (new LoggedInEmployeeModel().Rank > 2)
            {
                dataTable1DataGrid.IsEnabled = false; rightStackPanel.IsEnabled = false; flyout.IsEnabled = false;
                combo.IsEnabled = true;
            }
            bill = new GetBillModel();
            billProductModel = new BillProductModel();
            POS_Get_Bills();
            webHandler.POS_Get_BillCompleted += (x, y) =>
            {
                var result = y.Result;
                bill.GetBillCollection = new ObservableCollection<GetBillModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    bill.GetBillCollection.Add(new GetBillModel()
                    {
                        Billid = int.Parse(arg[0]),
                        Current = (DateTime.ParseExact(arg[1], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)),
                        Total = Decimal.Parse(arg[2]),
                        PaidAmount = arg[3],
                        PaymentMethod = arg[4],
                        CustomerName = arg[5],
                        Customerid = int.Parse(arg[6]),
                        TotalBalance = decimal.Parse(arg[7]),
                        EmployeeId = int.Parse(arg[8]),
                        EmployeeName = arg[9],

                    });
                }
                combo.ItemsSource = bill.GetBillCollection;


            };
            webHandler.POS_Get_Bill_ProductCompleted += (x, y) =>
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
                try
                {
                    total.Content = SelectedBill.Total;
                    var a = billProductModel.BillCollection.SingleOrDefault(x1 => x1.Status == "Cr");
                    if (a != null)
                    {
                        billProductModel.BillCollection.Remove(a);
                        rightcheck.IsChecked = true;
                        paymentamount.Text = a.TotalAmount.ToString();
                        List<RadioButton> radioButtons = new List<RadioButton>();
                        WalkLogicalTree(radioButtons, rootpanel);
                        foreach (RadioButton rb in radioButtons)
                        {
                            if (rb.GroupName == "ready" && (string)rb.Content == a.ProductName)
                            {
                                rb.IsChecked = true;
                            }
                        }
                    }
                    else
                    {
                        rightcheck.IsChecked = false;
                        paymentamount.Text = null;
                        bill.PaymentMethod = null;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                }
                dataTable1DataGrid.ItemsSource = billProductModel.BillCollection;
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
                        InvalidateVisual();
                    }
                }
                if (resultout1 == 0)
                {
                    var res1 = await win.ShowMessageAsync("Something is wrong...", "Please try again " +
                            MessageDialogStyle.Affirmative);
                    NavigationService navigation = NavigationService.GetNavigationService(this);
                    navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                }
            };
            webHandler.POS_delete_billCompleted += async (x, y) =>
             {
                 var win = (MetroWindow)Application.Current.MainWindow;
                 resultout1 = y.Result;
                 if (resultout1 == 1)
                 {
                     var res1 = await win.ShowMessageAsync("Bill Deleted...", " " +
                             MessageDialogStyle.Affirmative);
                     if (res1 == MessageDialogResult.Affirmative)
                     {
                         InvalidateVisual();
                         NavigationService navigation = NavigationService.GetNavigationService(this);
                         navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));

                     }
                 }
                 if (resultout1 == 0)
                 {
                     var res1 = await win.ShowMessageAsync("Something is wrong...", "Please try again " +
                             MessageDialogStyle.Affirmative);
                 }
             };
        }
        private void WalkLogicalTree(List<RadioButton> radioButtons, object parent)
        {
            DependencyObject doParent = parent as DependencyObject;
            if (doParent == null) return;
            foreach (object child in LogicalTreeHelper.GetChildren(doParent))
            {
                if (child is RadioButton)
                {
                    radioButtons.Add(child as RadioButton);
                }
                WalkLogicalTree(radioButtons, child);
            }
        }
        private void POS_Get_Bills()
        {
            webHandler.POS_Get_BillAsync();

        }

        private void backbutton(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
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
                bill.PaymentMethod = null;

            }
            catch (Exception e1)
            {

                Debug.WriteLine(e1.Message);
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
                webHandler.POS_Delete_Bill_ProductAsync(SelectedBill.Billid);
                webHandler.POS_Delete_Bill_ProductCompleted += (x, y) =>
                {
                    var result = y.Result;
                    string data = "";
                    decimal paid = 0; string method = "";
                    if (SelectedBill.Customerid != 0 && billProductModel.BillCollection != null)
                    {
                        foreach (var item in billProductModel.BillCollection)
                        {
                            BALANCE = BALANCE + item.TotalPerItem;
                            data += SelectedBill.Billid + "," + SelectedBill.Customerid + "," + item.Quantity + "," + item.ProductName + "," + item.Rate + "," + item.TotalPerItem + "," + "Dr," + BALANCE + ";";
                        }

                        if ((bool)rightcheck.IsChecked && paymentamount.Text != null && bill.PaymentMethod != null && decimal.Parse(paymentamount.Text) != 0)
                        {
                            paid = Decimal.Parse(paymentamount.Text);
                            BALANCE = BALANCE - paid;
                            method = bill.PaymentMethod;
                            data += SelectedBill.Billid + "," + SelectedBill.Customerid + "," + "0" + "," + bill.PaymentMethod + "," + "0" + "," + paid + "," + "Cr," + BALANCE + ";";
                        }
                        webHandler.POS_Update_Customer_BalanceAsync(SelectedBill.Customerid, BALANCE, 1);
                        webHandler.POS_Update_Bill(SelectedBill.Billid, billProductModel.BillCollection.Sum(x1 => x1.TotalPerItem), SelectedBill.Customerid, paid, method);
                        webHandler.POS_Add_Bill_ProductAsync(resultout1, data);
                    }

                };



            }
            catch (Exception e1)
            {

                Debug.Write(e1.Message);
            }
        }
        private void combo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                SelectedBill = (GetBillModel)combo.SelectedItem;
                BALANCE = SelectedBill.TotalBalance;
                BALANCE = BALANCE - SelectedBill.Total;
                BALANCE = BALANCE + Decimal.Parse(SelectedBill.PaidAmount);

                if (SelectedBill != null)
                {
                    webHandler.POS_Get_Bill_ProductAsync(SelectedBill.Billid);
                }
            }
            catch (Exception)
            {

            }
        }



        private void tenterd_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                webHandler.POS_Delete_Bill_ProductAsync(SelectedBill.Billid);
                webHandler.POS_Delete_Bill_ProductCompleted += (x, y) =>
                {
                    webHandler.POS_delete_billAsync(SelectedBill.Billid);
                };
            }
            catch (Exception)
            {

            }

        }
        private void Button_Click_delete(object sender, RoutedEventArgs e)
        {
            try
            {
                BillProductModel row = (BillProductModel)dataTable1DataGrid.SelectedItems[0];
                ObservableCollection<BillProductModel> data = (ObservableCollection<BillProductModel>)dataTable1DataGrid.ItemsSource;
                data.Remove(row);
            }
            catch (Exception)
            {
                //billProductModel.BillCollection.RemoveAt(dataTable1DataGrid.SelectedIndex);
            }
        }
    }
}
