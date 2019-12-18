using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
using System.Windows.Threading;
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for Loginpage.xaml
    /// </summary>
    public partial class Loginpage : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private LoggedInEmployeeModel loginEmployee;
        private int? resultout = -1;
        public Loginpage()
        {
            InitializeComponent();
        }
        #region buttons
        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {

            
        }

        private void Button_TouchUp(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "7";
            SystemSounds.Asterisk.Play();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_TouchUp_1(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "5";
        }

        private void Button_TouchUp_2(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "0";
        }

        private void Button_TouchUp_4(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "8";
        }

        private void Button_TouchUp_5(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "9";
        }

        private void Button_TouchUp_6(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "4";
        }

        private void Button_TouchUp_7(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "6";
        }

        private void Button_TouchUp_8(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "1";
        }

        private void Button_TouchUp_9(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "2";
        }

        private void Button_TouchUp_10(object sender, TouchEventArgs e)
        {
            pass.Password = pass.Password + "3";
        }

        private void Button_TouchUp_11(object sender, TouchEventArgs e)
        {
            pass.Password = "";
        }

        private void Button_TouchUp_12(object sender, TouchEventArgs e)
        {
            var count = pass.Password.Count();
            if (count >= 1)
            {
                var pa = pass.Password.Remove((pass.Password.Count() - 1), 1);
                pass.Password = pa;
            }

        }

        #endregion
 

        private async void after()
        {
            if (resultout == 1)
            {
                var win = (MetroWindow)Application.Current.MainWindow;
                var res1 = await win.ShowMessageAsync("Welcome...", " " + new LoggedInEmployeeModel().FullName,
                      MessageDialogStyle.Affirmative);
                if (res1 == MessageDialogResult.Affirmative)
                {
                    NavigationService navigation = NavigationService.GetNavigationService(this);
                    navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));
                }
            }
            if (resultout == 0)
            {
                var win = (MetroWindow)Application.Current.MainWindow;
                var res1 = await win.ShowMessageAsync("Wrong PIN", " ",
                      MessageDialogStyle.Affirmative);
                pass.Password = "";
            }
            progressbar.Visibility = Visibility.Collapsed;

        }

        private async void error()
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Something went wrong", "Try entring some other dates",
                MessageDialogStyle.Affirmative);
            progressbar.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            progressbar.Visibility = Visibility.Visible;
            new TaskFactory().StartNew(() =>
            {
                var result = webHandler.POS_Login(pass.Password, ref resultout);
                if (resultout == 1)
                {
                    string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var item in arr)
                    {
                        string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                        loginEmployee = new LoggedInEmployeeModel()
                        {
                            EmployeeId = int.Parse(arg[0]),
                            FullName = arg[1],
                            Status = int.Parse(arg[2]),
                            Rank = int.Parse(arg[3]),
                            Email = arg[4],
                            Absent = int.Parse(arg[6]),
                        };
                    }
                }
                return result;

            })
           .ContinueWith(x =>
           {
               if (x.Result == null)
               {
                   Dispatcher.Invoke(new Action(() => { error(); }), DispatcherPriority.ContextIdle);
                   return;
               }
               Dispatcher.Invoke(new Action(() => { after(); }), DispatcherPriority.ContextIdle);
           });

        }
    }
}
