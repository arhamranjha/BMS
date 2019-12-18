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
using MahApps.Metro;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for PageSetting.xaml
    /// </summary>
    public partial class Setting : Page
    {
        public Setting()
        {
            InitializeComponent();
        }
        public void ChangeAppStyle(string theme, string color)
        {
            // set the Red accent and dark theme only to the current window
            ThemeManager.ChangeAppStyle(Application.Current,
                                        ThemeManager.GetAccent(color),
                                        ThemeManager.GetAppTheme(theme));
            InvalidateVisual();
        }

        private void backButton_TouchUp(object sender, TouchEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }



        private void theme_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addnew_TouchDown(object sender, TouchEventArgs e)
        {
            ChangeAppStyle(theme.Text, color.Text);
        }
    }
}
