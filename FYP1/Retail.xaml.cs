using System;
using System.Collections.Generic;
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

namespace FYP1
{
    /// <summary>
    /// Interaction logic for Retail.xaml
    /// </summary>
    public partial class Retail : Page
    {
        public Retail()
        {
            InitializeComponent();
        }

        private void tile_TouchDown(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("RetailProduct.xaml", UriKind.Relative));
        }

        private void Tile_TouchDown_1(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("RetailProductOptions.xaml", UriKind.Relative));
        }

        private void Tile_TouchDown_2(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("RetailProductCatagory.xaml", UriKind.Relative));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
        }

        private void rel_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Retail_Product_ProductOption.xaml", UriKind.Relative));
        }
    }
}
