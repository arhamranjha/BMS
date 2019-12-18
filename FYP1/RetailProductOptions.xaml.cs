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
    /// Interaction logic for ProductOptions.xaml
    /// </summary>
    public partial class ProductOptions : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private ProductOptionModel option;
        private ProductOptionModel selected = new ProductOptionModel();
        private ProductOptionModel newoption = new ProductOptionModel();
        private CollectionViewSource CVS;
        private ItemViewModel _itemViewModel = new ItemViewModel();
        private ListCollectionView bindlistview;
        private int count;
        private int count1;

        public ProductOptions()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            option = (ProductOptionModel)this.FindResource("opt");
            POS_Option();
            fly.DataContext = _itemViewModel;
            flyout.DataContext = newoption;
            webHandler.POS_add_ProductOptionCompleted += async (x, y) =>
             {
                 if (y.Result == 1)
                 {
                     var win = (MetroWindow)Application.Current.MainWindow;
                     var res = await win.ShowMessageAsync("New option added", "new option was succefully added",
                         MessageDialogStyle.Affirmative);
                 }
                 else
                 {
                     var win = (MetroWindow)Application.Current.MainWindow;
                     var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                         MessageDialogStyle.Affirmative);
                 }
             };
            webHandler.POS_update_ProductOptionCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("option changes saved", "",
                        MessageDialogStyle.Affirmative);

                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_delete_ProductOptionCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Option removed", "The option was removed",
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

        private void POS_Option()
        {
            webHandler.POS_Get_ProductOptionsAsync();
            webHandler.POS_Get_ProductOptionsCompleted += (x, y) =>
            {
                var result = y.Result;
                option.OptionCollection = new ObservableCollection<ProductOptionModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    option.OptionCollection.Add(new ProductOptionModel()
                    {
                        Optionid = int.Parse(arg[0]),
                        Optionname = arg[1],
                        Optionsprice = Decimal.Parse(arg[2]),
                        OptionDesc = arg[3],
                    });
                }
                CVS = ((CollectionViewSource)(this.FindResource("cvs")));
                bindlistview = CVS.View as ListCollectionView;
                //catagoryList.ItemsSource = catagory.CatagoryCollection;
            };
        }

        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Retail.xaml", UriKind.Relative));
        }

        private void add_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = true;
            newoption.Optionname = ""; newoption.Optionsprice = 0; newoption.OptionDesc = "";


        }

        private void update_TouchDown(object sender, TouchEventArgs e)
        {
            webHandler.POS_update_ProductOptionAsync(selected.Optionid, selected.Optionname, selected.Optionsprice, selected.OptionDesc);

        }

        private async void del_TouchDown(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Are you sure?", "if you delete this option " + selected.Optionname + " you wont be able to serve it",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                webHandler.POS_delete_ProductOptionAsync(selected.Optionid);
                option.OptionCollection.Remove(selected);

            }
        }

        private async void catagorytList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selected = (ProductOptionModel)optionList.SelectedItem;
                if (selected != null)
                {
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
        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && bindlistview != null)
            {
                bindlistview.Filter = delegate (object cust) { return ((ProductOptionModel)cust).Optionname.ToLower().Contains(search.Text.ToLower()); };
            }
            else if (search.Text == string.Empty)
            {
                bindlistview.Filter = null;
            }
        }


        private void cancelnew_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = false;
            newoption.Optionname = ""; newoption.Optionsprice = 0; newoption.OptionDesc = "";
        }

        private void addnew_TouchDown(object sender, TouchEventArgs e)
        {
            webHandler.POS_add_ProductOptionAsync(newoption.Optionname, newoption.Optionsprice, newoption.OptionDesc);
            _itemViewModel.Open = false;
            newoption.Optionname = ""; newoption.Optionsprice = 0; newoption.OptionDesc = "";

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
