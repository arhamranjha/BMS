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
    /// Interaction logic for ProductCatagory.xaml
    /// </summary>
    public partial class ProductCatagory : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private CatagoryModel catagory;
        private CatagoryModel selected = new CatagoryModel();
        private CatagoryModel newcatagory = new CatagoryModel();
        private ItemViewModel _itemViewModel = new ItemViewModel();
        private CollectionViewSource CVS;
        private ListCollectionView bindlistview;
        private int count1;
        private int count;

        public ProductCatagory()
        {
            InitializeComponent();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            catagory = (CatagoryModel)this.FindResource("cat");
            POS_Catagory();
            fly.DataContext = _itemViewModel;
            flyout.DataContext = newcatagory;
            webHandler.POS_add_catagoryCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("New catagory added", "new catagory was succefully added",
                        MessageDialogStyle.Affirmative);
                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_update_catagoryCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Catagory changes saved", "",
                        MessageDialogStyle.Affirmative);

                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Something went wrong", "try again in a little bit",
                        MessageDialogStyle.Affirmative);
                }
            };
            webHandler.POS_delete_catagoryCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Catgory removed", "The Catgory was removed",
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

        private void POS_Catagory()
        {
            webHandler.POS_Get_CatagoryAsync();
            webHandler.POS_Get_CatagoryCompleted += (x, y) =>
             {
                 var result = y.Result;
                 catagory.CatagoryCollection = new ObservableCollection<CatagoryModel>();
                 string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                 foreach (var item in arr)
                 {
                     string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                     catagory.CatagoryCollection.Add(new CatagoryModel()
                     {
                         Catagoryid = arg[0],
                         CatagoryName = arg[1],
                         CatagoryDesc = arg[2],
                     });
                 }
                 CVS = ((CollectionViewSource)(this.FindResource("cvs")));
                 bindlistview = CVS.View as ListCollectionView;
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
            newcatagory.CatagoryDesc = ""; newcatagory.CatagoryName = "";

        }

        private void update_TouchDown(object sender, TouchEventArgs e)
        {
            webHandler.POS_update_catagoryAsync(int.Parse(selected.Catagoryid), selected.CatagoryName, selected.CatagoryDesc);

        }

        private async void del_TouchDown(object sender, TouchEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Are you sure?", "if you delete this catagory "+selected.CatagoryName+" all the corrsponding products will be deleted",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                webHandler.POS_delete_catagoryAsync(int.Parse(selected.Catagoryid));
                catagory.CatagoryCollection.Remove(selected);

            }
        }

        private async void catagorytList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selected = (CatagoryModel)catagoryList.SelectedItem;
                if (selected != null)
                {
                    del.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                del.IsEnabled = false;
                update.Foreground = Brushes.Gray;
                del.Foreground = Brushes.Gray;
                var win = (MetroWindow)Application.Current.MainWindow;
                var res = await win.ShowMessageAsync("Something went wrong", "Try again in a little bit",
                    MessageDialogStyle.AffirmativeAndNegative);
            }

        }

        private void addnew_TouchDown(object sender, TouchEventArgs e)
        {
            webHandler.POS_add_catagoryAsync(newcatagory.CatagoryName, newcatagory.CatagoryDesc);
            _itemViewModel.Open = false;
            newcatagory.CatagoryDesc = ""; newcatagory.CatagoryName = "";


        }
        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && bindlistview != null)
            {
                bindlistview.Filter = delegate (object cust) { return ((CatagoryModel)cust).CatagoryName.ToLower().Contains(search.Text.ToLower()); };
            }
            else if (search.Text == string.Empty)
            {
                bindlistview.Filter = null;
            }
        }

        private void cancelnew_TouchDown(object sender, TouchEventArgs e)
        {
            _itemViewModel.Open = false;
            newcatagory.CatagoryDesc = ""; newcatagory.CatagoryName = "";
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
