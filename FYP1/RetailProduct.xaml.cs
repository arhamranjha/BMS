using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for RetailProduct.xaml
    /// </summary>
    public partial class RetailProduct : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private ProductsModel product;
        private ProductsModel selected = new ProductsModel();
        private ProductsModel newproduct = new ProductsModel();
        private CollectionViewSource CVS;
        private ListCollectionView bindlistview;
        private ItemViewModel _itemViewModel = new ItemViewModel();
        private StatusModel dropDownStatus = new StatusModel();
        private CatagoryModel dropDownCat = new CatagoryModel();
        private int count;
        private int count1;

        public RetailProduct()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            product = (ProductsModel)this.FindResource("pro");
            POS_Product();
            Drop_Down_Status();
            Drop_Down_Cat();
            fly.DataContext = _itemViewModel;
            flyout.DataContext = newproduct;
            webHandler.POS_add_ProductCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("New product added", "new product was succefully added",
                        MessageDialogStyle.Affirmative);
                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_Update_ProductCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Product changes saved", "",
                        MessageDialogStyle.Affirmative);

                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_Delete_ProductCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Product removed", "The product was removed",
                        MessageDialogStyle.Affirmative);

                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
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
                CVS = ((CollectionViewSource)(this.FindResource("cvs")));
                bindlistview = CVS.View as ListCollectionView;
            };
        }
        private void Drop_Down_Status()
        {
            webHandler.POS_Get_StatusAsync();
            webHandler.POS_Get_StatusCompleted += (x, y) =>
            {
                var result = y.Result;
                dropDownStatus.StatusCollection = new ObservableCollection<StatusModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    dropDownStatus.StatusCollection.Add(new StatusModel()
                    {
                        Statusid = arg[0],
                        StatusDescription = arg[1],
                    });
                }
                status.ItemsSource = dropDownStatus.StatusCollection;
                status1.ItemsSource = dropDownStatus.StatusCollection;
            };
        }
        private void Drop_Down_Cat()
        {
            webHandler.POS_Get_CatagoryAsync();
            webHandler.POS_Get_CatagoryCompleted += (x, y) =>
            {
                var result = y.Result;
                dropDownCat.CatagoryCollection = new ObservableCollection<CatagoryModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    dropDownCat.CatagoryCollection.Add(new CatagoryModel()
                    {
                        Catagoryid = arg[0],
                        CatagoryName = arg[1],
                    });
                }
                catagory.ItemsSource = dropDownCat.CatagoryCollection;
                catagory1.ItemsSource = dropDownCat.CatagoryCollection;
            };
        }
        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Retail.xaml", UriKind.Relative));
        }

        private void add_TouchDown(object sender, TouchEventArgs e)
        {
            //newproduct = new ProductsModel();
            newproduct.ProductName = ""; newproduct.ProductDesc = ""; newproduct.ProductCatagoryid = ""; newproduct.ProductStatusid = ""; newproduct.ProductBuyingPrice = ""; newproduct.ProductPrice = "";
           
            _itemViewModel.Open = true;
        }

        private void update_TouchDown(object sender, TouchEventArgs e)
        {
            webHandler.POS_Update_ProductAsync(int.Parse(selected.Productid), selected.ProductName, int.Parse(selected.ProductCatagoryid), Decimal.Parse(selected.ProductPrice), int.Parse(selected.ProductStatusid), int.Parse(selected.ProductQuantity), selected.ProductDesc, Decimal.Parse(selected.ProductBuyingPrice));

        }

        private async void del_TouchDown(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Are you sure?", "This " + selected.ProductName + " products will be deleted",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                webHandler.POS_Delete_ProductAsync(int.Parse(selected.Productid));
                product.ProductsCollection.Remove(selected);

            }
        }

        private async void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selected = (ProductsModel)productList.SelectedItem;
                if (selected != null)
                {
                    update.IsEnabled = true;
                    del.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                update.IsEnabled = false;
                del.IsEnabled = false;
                update.Foreground = Brushes.Gray;
                del.Foreground = Brushes.Gray;
                var win = (MetroWindow)Application.Current.MainWindow;
                var res = await win.ShowMessageAsync("Something went wrong", "Try again in a little bit",
                    MessageDialogStyle.AffirmativeAndNegative);
            }
        }
        //int.Parse(newproduct.ProductStatusid)
        private void addnew_TouchDown(object sender, TouchEventArgs e)
        {
            webHandler.POS_add_ProductAsync(newproduct.ProductName, int.Parse(newproduct.ProductCatagoryid), Decimal.Parse(newproduct.ProductPrice), int.Parse(newproduct.ProductStatusid), int.Parse(newproduct.ProductQuantity), newproduct.ProductDesc, Decimal.Parse(newproduct.ProductBuyingPrice));
            product.ProductsCollection.Add(new ProductsModel()
            {                
                ProductName = newproduct.ProductName,
                ProductCatagoryid = newproduct.ProductCatagoryid,
                ProductPrice = newproduct.ProductPrice,
                ProductStatusid = newproduct.ProductStatusid,                
                ProductQuantity = newproduct.ProductQuantity,
                ProductDesc = newproduct.ProductDesc,
                ProductBuyingPrice = newproduct.ProductBuyingPrice,

            });
            _itemViewModel.Open = false;

        }

        private void cancelnew_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = false;
            newproduct.ProductName = ""; newproduct.ProductDesc = ""; newproduct.ProductCatagoryid = ""; newproduct.ProductStatusid = ""; newproduct.ProductBuyingPrice = ""; newproduct.ProductPrice = "";

        }

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && bindlistview != null)
            {
                bindlistview.Filter = delegate (object cust) { return ((ProductsModel)cust).ProductName.ToLower().Contains(search.Text.ToLower()); };
            }
            else if (search.Text == string.Empty)
            {
                bindlistview.Filter = null;
            }
        }

        private void ToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            var a = (ToggleSwitch)sender;
            if ((bool)a.IsChecked)
            {
                bindlistview.Filter = delegate (object cust) { return ((ProductsModel)cust).ProductStatusDesc == "Active"; };
            }
            else { bindlistview.Filter = null; }
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                update.IsEnabled = false; update.Foreground = Brushes.Gray;
                count++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count--;
                if (count == 0)
                {
                    update.IsEnabled = true; update.Foreground = (Brush)this.FindResource("AccentColorBrush");

                }
            }
        }

        private void TextBox_Error_1(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addnew.IsEnabled = false; addnew.Foreground = Brushes.Gray;
                count1++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count1--;
                if (count1 == 0)
                {
                    addnew.IsEnabled = true;
                    addnew.Foreground = (Brush)this.FindResource("AccentColorBrush");
                }
            }
        }
    }
}
