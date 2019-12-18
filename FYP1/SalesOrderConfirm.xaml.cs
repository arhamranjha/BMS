using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Printing;
using System.Windows.Shapes;
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.IO;
using System.Diagnostics;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for ConfirmSalesOrder.xaml
    /// </summary>
    public partial class ConfirmSalesOrder : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private SaleModelProducts productSold = new SaleModelProducts();
        private SaleModelProducts productOptions = new SaleModelProducts();
        private ItemViewModel _ItemViewModel = new ItemViewModel();
        private string finalProducts;
        private string finalProductsOptions;
        private int? resultout = -1;
        private int? resultout1 = -1;
        PrintDialog printDialog = new PrintDialog();
        Table tb = new Table();
        TableRowGroup trg = new TableRowGroup();
        private int count1;
        private int count2;

        public ConfirmSalesOrder()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //webHandler.POS_Order_ProductCompleted += (x, y) => { resultout = y.got; CheckResult(); };
            //webHandler.POS_Order_OptionsCompleted += (x, y) => { resultout1 = y.got; CheckResult(); };
            POS_ProductOptions();
            this.productSold = new SalesOrder().Sold;
            total.DataContext = _ItemViewModel;
            fly.DataContext = _ItemViewModel;

        }

        private void Sum()
        {
            double sum = 0;
            foreach (var item in productSold.ProductCollection)
            {
                double opt = 0;
                foreach (var item1 in item.ProductOptionList)
                {
                    opt += int.Parse(item1.ProductTotalQuantity) * double.Parse(item1.ProductPrice);
                }

                sum += ((double.Parse(item.ProductPrice) * double.Parse(item.ProductTotalQuantity)) + opt);
            }
            _ItemViewModel.Total = sum;
        }






        private void POS_ProductOptions()
        {
            webHandler.POS_Sale_OrderAsync();
            webHandler.POS_Sale_OrderCompleted += (x, y) =>
            {
                var result = y.Result;
                productOptions.ProductOptionList = new List<SaleModelProducts>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    productOptions.ProductOptionList.Add(new SaleModelProducts()
                    {
                        Productid = arg[0],
                        ProductStatus = arg[1],
                        ProductName = arg[2],
                        ProductDesc = arg[3],
                        ProductPrice = arg[4],
                        ProductTotalPrice = "0",
                        ProductTotalQuantity = "0",
                    });
                }
                addProductOptions();
            };
        }
        private void addProductOptions()
        {
            foreach (var item in productOptions.ProductOptionList)
            {
                foreach (var itemsales in productSold.ProductCollection)
                {

                    if (itemsales.ProductOptionList == null)
                    {
                        itemsales.ProductOptionList = new List<SaleModelProducts>();
                    }
                    if (item.Productid.Equals(itemsales.Productid))
                    {
                        item.ProductQuantity = itemsales.ProductTotalQuantity;
                        itemsales.ProductOptionList.Add(item);
                    }
                }
            }
            gridSource.DataContext = productSold;
            Sum();
        }

        private async void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Cancel Order", "Are you sure you want to cancel this order?",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                webHandler.POS_delete_billAsync(new BillModel().Billid);
                webHandler.POS_delete_billCompleted += (x, y) =>
                {
                    NavigationService navigation = NavigationService.GetNavigationService(this);
                    navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                };
            }
        }

        private void listBoxwithoptions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var a = (ListBox)sender;
            //var b = (SaleModelProducts)a.SelectedItem;
            ////var c = b.product_name;
        }



        private void minus_TouchDown(object sender, TouchEventArgs e)
        {
            var _id = ((Button)sender).Tag;
            var _id1 = ((Button)sender).Uid;
            foreach (var item in productSold.ProductCollection)
            {
                foreach (var item1 in item.ProductOptionList)
                {
                    if (item1.ProductStatus.Equals(_id) && item1.Productid.Equals(_id1))
                    {
                        var quantity = int.Parse(item1.ProductTotalQuantity); quantity--;
                        item1.ProductTotalQuantity = quantity.ToString();
                        //item1.ProductQuantity = quantity.ToString();
                    }
                }
            }
            Sum();
        }


        private void NumericUpDown_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            try
            {
                var _id = ((NumericUpDown)sender).Tag;
                var nu1 = ((NumericUpDown)sender);
                foreach (var item in productSold.ProductCollection)
                {
                    foreach (var item1 in item.ProductOptionList)
                    {
                        if (item1.Productid.Equals(_id))
                        {
                            item1.ProductQuantity = nu1.Value.ToString();
                        }
                    }
                }
                Sum();
            }
            catch (Exception e1)
            {
                Debug.WriteLine(e1.Message);
            }

        }
        private void add_TouchDown_1(object sender, TouchEventArgs e)
        {
            var _id1 = ((Button)sender).Uid;
            var _id = ((Button)sender).Tag;
            foreach (var item in productSold.ProductCollection)
            {
                foreach (var item1 in item.ProductOptionList)
                {
                    if (item1.ProductStatus.Equals(_id) && item1.Productid.Equals(_id1))
                    {
                        var quantity = int.Parse(item1.ProductTotalQuantity); quantity++;
                        item1.ProductTotalQuantity = quantity.ToString();
                        //item.ProductQuantity = item1.ProductTotalQuantity;
                    }
                }
            }
            Sum();
        }



        private async void remove_TouchDown(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Remove Item", "Are you sure you want to remove this item from cart?",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                var _id = ((Button)sender).Tag;
                foreach (var item in productSold.ProductCollection)
                {
                    if (item.Productid.Equals(_id))
                    {
                        listBoxwithitems.SelectedItem = item;
                    }
                }
                SaleModelProducts selected = (SaleModelProducts)listBoxwithitems.SelectedItem;
                productSold.ProductCollection.Remove(selected); Sum();
            }

        }

        private async void cancel_TouchDown(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Cancel Order", "Are you sure you want to cancel this order?",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                webHandler.POS_delete_billAsync(new BillModel().Billid);
                webHandler.POS_delete_billCompleted += (x, y) =>
                {
                    NavigationService navigation = NavigationService.GetNavigationService(this);
                    navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                };
            }
        }

        private void confirm_TouchDown(object sender, TouchEventArgs e)
        {
            _ItemViewModel.Open = true;
        }
        private async void CheckResult()
        {
            tb.RowGroups.Add(trg);
            FlowDocument doc = new FlowDocument();
            var win = (MetroWindow)Application.Current.MainWindow;
            if (finalProductsOptions == null)
            {
                if (resultout == 1)
                {
                    var res1 = await win.ShowMessageAsync("Done1...", " " +
                           MessageDialogStyle.Affirmative);
                    if (res1 == MessageDialogResult.Affirmative)
                    {
                        doc.Blocks.Add(tb);
                        doc.Name = "FlowDoc";
                        IDocumentPaginatorSource idpSource = doc;
                        if (printDialog.ShowDialog() == true)
                        {
                            printDialog.PrintDocument(idpSource.DocumentPaginator, "My Printing");
                            NavigationService navigation = NavigationService.GetNavigationService(this);
                            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                        }
                        else
                        {
                            NavigationService navigation = NavigationService.GetNavigationService(this);
                            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                        }

                    }
                }
            }
            else
            {
                if (resultout == 1 && resultout1 == 1)
                {
                    var res1 = await win.ShowMessageAsync("Done2...", " " +
                           MessageDialogStyle.Affirmative);
                    if (res1 == MessageDialogResult.Affirmative)
                    {
                        //foreach (var x in printlis)
                        //{
                        //    p.Inlines.Add(x + "\r\n");
                        //    p.Margin = new Thickness(0);
                        //}
                        doc.Blocks.Add(tb);
                        doc.Name = "FlowDoc";
                        IDocumentPaginatorSource idpSource = doc;
                        if (printDialog.ShowDialog() == true)
                        {
                            printDialog.PrintDocument(idpSource.DocumentPaginator, "My Printing");
                            //printlis = new List<string>();
                            NavigationService navigation = NavigationService.GetNavigationService(this);
                            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                        }
                        else
                        {
                            NavigationService navigation = NavigationService.GetNavigationService(this);
                            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                        }

                    }
                }
            }
            if (resultout == 0 || resultout1 == 0)
            {
                var res1 = await win.ShowMessageAsync("somethings wrong...", " " +
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


        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                confirm.IsEnabled = false; confirm.Foreground = Brushes.Gray;
                count1++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count1--;
                if (count1 == 0)
                {
                    confirm.IsEnabled = true;
                    confirm.Foreground = Brushes.YellowGreen;
                }
            }
        }

        private void tenterd_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        #region buttons
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void Button_TouchUp(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "7";
        }



        private void Button_TouchUp_1(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "5";
        }

        private void Button_TouchUp_2(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "0";
        }

        private void Button_TouchUp_4(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "8";
        }

        private void Button_TouchUp_5(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "9";
        }

        private void Button_TouchUp_6(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "4";
        }

        private void Button_TouchUp_7(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "6";
        }

        private void Button_TouchUp_8(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "1";
        }

        private void Button_TouchUp_9(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "2";
        }

        private void Button_TouchUp_10(object sender, TouchEventArgs e)
        {
            tenterd.Text = tenterd.Text + "3";
        }

        private void Button_TouchUp_11(object sender, TouchEventArgs e)
        {
            tenterd.Text = "";
        }

        private void Button_TouchUp_12(object sender, TouchEventArgs e)
        {
            var count = tenterd.Text.Count();
            if (count >= 1)
            {
                var pa = tenterd.Text.Remove((tenterd.Text.Count() - 1), 1);
                tenterd.Text = pa;
            }

        }

        #endregion
        private void Button_TouchUp_3(object sender, TouchEventArgs e)
        {
            #region reciept
            tb.Columns.Add(new TableColumn());
            tb.Columns.Add(new TableColumn() { Width = GridLength.Auto });
            tb.Columns.Add(new TableColumn());
            tb.CellSpacing = 0; tb.FontSize = 12; tb.FontFamily = new FontFamily("sans serif");
            TableRow tr = new TableRow();
            Image img = new Image();
            img.Source = new BitmapImage(new Uri("C:/Users/owner/documents/visual studio 2015/Projects/FYP1/FYP1/Resources/Counting-Machine-small-task.png", UriKind.RelativeOrAbsolute));
            Paragraph p2 = new Paragraph(); p2.TextAlignment = TextAlignment.Center;
            p2.Inlines.Add(img);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(p2));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);


            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("\tBMS")))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("    Food! you Love"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Address:Sesemey \tStreet,")))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("\tNear SKM,")))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("\tLahore")))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Tel: 0421-458-8741"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("GST ID: 00345874518"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            var day = DateTime.Now.ToShortDateString();
            var time = DateTime.Now.ToShortTimeString();
            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Date:"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + day))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + time))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Invoice No."))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + new BillModel().Billid))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Staff ID:"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + new BillModel().EmployeeId))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Description:"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Unit Price:"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Unit Qty * Unit Price"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Amount"))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            trg.Rows.Add(tr);


            //string filepath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //string path = filepath + @"/test1.bin";
            //using (StreamWriter writer = new StreamWriter(path, true))
            //{
            //string pro = "";
            //string option = "";
            foreach (var item in productSold.ProductCollection)
            {
                tr = new TableRow();
                tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run(" " + item.ProductName)))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
                trg.Rows.Add(tr);

                tr = new TableRow();
                tr.Cells.Add(new TableCell(new Paragraph(new Run("Rs " + Double.Parse(item.ProductPrice)))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + item.ProductTotalQuantity + "*" + Double.Parse(item.ProductPrice)))));
                tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + item.ProductTotalPrice + " Rs"))));
                trg.Rows.Add(tr);

                finalProducts += new BillModel().Billid + "," + item.Productid + "," + item.ProductTotalQuantity + ";";
                //pro += "|||" + "," + item.Productid + "," + item.ProductTotalQuantity + ";";
                foreach (var item1 in item.ProductOptionList)
                {
                    if (int.Parse(item1.ProductTotalQuantity) > 0)
                    {
                        var opt = int.Parse(item1.ProductTotalQuantity) * double.Parse(item1.ProductPrice);

                        tr = new TableRow();
                        tr.Cells.Add(new TableCell(new Paragraph(new Italic(new Run(" --- " + item1.ProductName)))));
                        tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + item1.ProductTotalQuantity + "*" + double.Parse(item1.ProductPrice)))));
                        tr.Cells.Add(new TableCell(new Paragraph(new Run(" " + opt + " Rs"))));
                        trg.Rows.Add(tr);

                        finalProductsOptions += new BillModel().Billid + "," + item.Productid + "," + item1.ProductStatus + "," + item1.ProductTotalQuantity + ";";
                        //option += "|||" + "," + item.Productid + "," + item1.ProductStatus + "," + item1.ProductTotalQuantity + ";";
                    }
                }
            }
            //if (finalProductsOptions != null)
            //{
            //    writer.WriteLine(pro + "===" + option + "\n");
            //}
            //else
            //{
            //    writer.WriteLine(finalProducts);
            //}


            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            trg.Rows.Add(tr);

            var t = _ItemViewModel.Total;
            var t1 = (18 * _ItemViewModel.Total) / 100;

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Subtotal Exc GST"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Rs " + (t - t1)))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("GST"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Rs " + t1))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Service Charges"))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("Rs 0"))));
            trg.Rows.Add(tr);


            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            trg.Rows.Add(tr);


            Bold to = new Bold(); to.FontSize = 18; to.Inlines.Add("Rs " + t);
            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("TOTAL Inc Tax")))));
            tr.Cells.Add(new TableCell(new Paragraph(to)));
            trg.Rows.Add(tr);


            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            trg.Rows.Add(tr);

            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Run("--------------------"))));
            trg.Rows.Add(tr);

            Bold to1 = new Bold(); to1.FontSize = 14; to1.Inlines.Add("Rs " + tenterd.Text);
            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Tendered:")))));
            tr.Cells.Add(new TableCell(new Paragraph(to1)));
            trg.Rows.Add(tr);

            Bold to2 = new Bold(); to2.FontSize = 14; to2.Inlines.Add("Rs " + rem.Text);
            tr = new TableRow();
            tr.Cells.Add(new TableCell(new Paragraph(new Run(""))));
            tr.Cells.Add(new TableCell(new Paragraph(new Bold(new Run("Change:")))));
            tr.Cells.Add(new TableCell(new Paragraph(to2)));
            trg.Rows.Add(tr);


            #endregion reciept
            webHandler.POS_Order_Product(ref resultout, finalProducts);
            if (finalProductsOptions != null)
            {
                webHandler.POS_Order_Options(ref resultout1, finalProductsOptions);
                finalProductsOptions = "";

            }
            //webHandler.POS_update_billAsync(new BillModel().Billid, Decimal.Parse(_ItemViewModel.Total.ToString()));
            finalProducts = "";
            CheckResult();

        }

     

        private void tenterd_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                Confirm_Sale.IsEnabled = false; Confirm_Sale.Background = Brushes.Gray;
                count2++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count2--;
                if (count2 == 0)
                {
                    Confirm_Sale.IsEnabled = true;
                    Confirm_Sale.Background = (Brush)this.FindResource("salecolor");
                }
            }
        }

        private void tenterd_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (! tenterd.Text.Equals(string.Empty))
                {
                    rem.Text =(-(int.Parse(total1.Text) - int.Parse(tenterd.Text))).ToString();
                }
            }
            catch (Exception e1)
            {
                Debug.WriteLine(e1.Message);
            }

        }
    }
}
