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
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for Retail_Product_ProductOption.xaml
    /// </summary>
    public partial class Retail_Product_ProductOption : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private ProductOptionRelationModel product;
        private ItemViewModel _itemViewModel = new ItemViewModel();
        private ProductsModel dropDownpro = new ProductsModel();
        private ProductOptionModel dropDownOpt = new ProductOptionModel();
        ICollectionView view;
        private int count;

        public ObservableCollection<Portfolio> PortfolioInfo { get; set; }
        public Retail_Product_ProductOption()
        {
            InitializeComponent();

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            product = new ProductOptionRelationModel();
            POS_Rel();
            Drop_Down_pro();
            Drop_Down_Opt();
            fly.DataContext = _itemViewModel;
            webHandler.POS_add_ProductOptionRelationCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("New product option added", "All good",
                        MessageDialogStyle.Affirmative);
                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "This option may already exists!",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_delete_ProductOptionRelationCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Option Deleted", "",
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

        private void Drop_Down_Opt()
        {
            webHandler.POS_Get_ProductOptionsAsync();
            webHandler.POS_Get_ProductOptionsCompleted += (x, y) =>
            {
                var result = y.Result;
                dropDownOpt.OptionCollection = new ObservableCollection<ProductOptionModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    dropDownOpt.OptionCollection.Add(new ProductOptionModel()
                    {
                        Optionid = int.Parse(arg[0]),
                        Optionname = arg[1],
                    });
                }
                catagory2.ItemsSource = dropDownOpt.OptionCollection;

            };
        }

        private void Drop_Down_pro()
        {
            webHandler.POS_Get_ProductAsync();
            webHandler.POS_Get_ProductCompleted += (x, y) =>
            {
                var result = y.Result;
                dropDownpro.ProductsCollection = new ObservableCollection<ProductsModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    dropDownpro.ProductsCollection.Add(new ProductsModel()
                    {
                        Productid = arg[0],
                        ProductName = arg[1],
                    });
                }
                product1.ItemsSource = dropDownpro.ProductsCollection;
            };
        }

        private void POS_Rel()
        {
            webHandler.POS_Get_Products_Options_RelAsync();
            webHandler.POS_Get_Products_Options_RelCompleted += (x, y) =>
            {
                var result = y.Result;
                product.ProductsCollection = new ObservableCollection<ProductOptionRelationModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    product.ProductsCollection.Add(new ProductOptionRelationModel()
                    {
                        Productid = arg[0],
                        ProductName = arg[1],
                        Optionid = arg[2],
                        OptionName = arg[3],

                    });
                }

                PortfolioInfo = new ObservableCollection<Portfolio>();
                var selectedItem = from p in product.ProductsCollection group p by p.ProductName;
                foreach (var item in selectedItem)
                {
                    var key = item.Key;
                    ObservableCollection<Portfolio> liss = new ObservableCollection<Portfolio>();
                    string namei = "";
                    foreach (var itemkeys in item)
                    {
                        namei = itemkeys.Productid;
                        liss.Add(new Portfolio() { name = itemkeys.Optionid, Optname = itemkeys.OptionName });
                        Console.WriteLine("\n\t" + itemkeys.Optionid + "---" + itemkeys.OptionName);
                    }
                    PortfolioInfo.Add(new Portfolio() { name = key, nameid = namei, ItemList = liss });
                }
                gridSource.DataContext = PortfolioInfo;
                view = CollectionViewSource.GetDefaultView(PortfolioInfo);

                //itee.ItemsSource = PortfolioInfo;
            };
        }

        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Retail.xaml", UriKind.Relative));
        }
        private void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //var a = (Portfolio)productList.SelectedItem;
            //productList1.ItemsSource = a.ItemList;
        }

        private async void delbutton_TouchUp(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Are you sure?", "This option will be deleted",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                try
                {
                    var a = ((Portfolio)productList.SelectedItem).nameid;
                    var b = ((Button)sender).Tag.ToString();
                    webHandler.POS_delete_ProductOptionRelationAsync(int.Parse(a), int.Parse(b));
                }
                catch (Exception e1)
                {
                    Debug.Write(e1.Message);

                }

            }
        }
        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && view != null)
            {
                view.Filter += delegate (object cust) { return ((Portfolio)cust).name.ToLower().Contains(search.Text.ToLower()); };
            }
            else if (search.Text == string.Empty)
            {
                view.Filter = null;
            }
        }


        private void add_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = true;

        }

        private void cancelnew_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = false;
        }

        private void addnew_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (product1.SelectedItem != null && catagory2.SelectedItem != null)
                {
                    var a = product1.SelectedValue.ToString();
                    var b = catagory2.SelectedValue.ToString();
                    webHandler.POS_add_ProductOptionRelationAsync(int.Parse(a), int.Parse(b));
                    _itemViewModel.Open = false;
                }
            }
            catch (Exception e1)
            {
                Debug.Write(e1.Message);
            }


        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added)
            {
                addnew.IsEnabled = false; addnew.Foreground = Brushes.Gray;
                count++;
            }
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                count--;
                if (count == 0)
                {
                    addnew.IsEnabled = true; addnew.Foreground = (Brush)this.FindResource("AccentColorBrush");

                }
            }
        }
    }
}





