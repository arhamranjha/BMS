using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }



        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                this.clock.Text = DateTime.Now.ToString("HH:mm:ss");
            }, this.Dispatcher);
            //this.clock.Text = new LoggedInEmployeeModel().FullName;
        }


        private void tile_TouchDown(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("BIllBook.xaml", UriKind.Relative));
        }

        private void Tile_TouchUp(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Employee.xaml", UriKind.Relative));
        }

        private async void Tile_TouchDown_1(object sender, RoutedEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res1 = await win.ShowMessageAsync("Are you sure?...", "ShutDown Application",
                  MessageDialogStyle.AffirmativeAndNegative);
            if (res1 == MessageDialogResult.Affirmative)
            {
                Application.Current.Shutdown();
            }

        }

        private void Tile_TouchDown_2(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Retail.xaml", UriKind.Relative));
        }

        private void Tile_TouchUp_1(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Reports.xaml", UriKind.Relative));
        }
        private void Setting_tile_touch_down(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Setting.xaml", UriKind.Relative));
        }
        private async void Logout_tile_touch_up(object sender, RoutedEventArgs e)
        {
            var winn = (MetroWindow)Application.Current.MainWindow;
            var res11 = await winn.ShowMessageAsync("Are you sure?...", "You Want to Logout?",
                  MessageDialogStyle.AffirmativeAndNegative);
            if (res11 == MessageDialogResult.Affirmative)
            {
                NavigationService navigation = NavigationService.GetNavigationService(this);
                navigation.Navigate(new Uri("LoginPage.xaml", UriKind.Relative));
            }
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("RetailCustomer.xaml", UriKind.Relative));
        }

        private void Tile_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("ReportCustomer.xaml", UriKind.Relative));
        }

        private void Tile_TouchUp_2(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("RetailCustomer.xaml", UriKind.Relative));
        }

        private void Tile_TouchUp_3(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("CustomerLedger.xaml", UriKind.Relative));
        }


        private void Tile_TouchDown_3(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("FinalLedger.xaml", UriKind.Relative));
        }

        private void Tile_TouchDown_4(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("BillStatus.xaml", UriKind.Relative));
        }

        private void Tile_TouchUp_4(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("BillStatusShipment.xaml", UriKind.Relative));
        }
    }
}
