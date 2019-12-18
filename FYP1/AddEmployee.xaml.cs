using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using FYP1.Models;
using FYP1.POSServiceReference;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace FYP1
{

    /// <summary>
    /// Interaction logic for AddEmployee.xaml
    /// </summary>
    public partial class AddEmployee : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private StatusModel dropDownStatus = new StatusModel();
        private RankModel dropDownRank = new RankModel();
        private EmployeeModel empAdd = new EmployeeModel();
        private int count1;

        public AddEmployee()
        {
            InitializeComponent();
        }

        private async void backButton_TouchUp(object sender, RoutedEventArgs e)
        {
            var win = (MetroWindow)Application.Current.MainWindow;
            var res = await win.ShowMessageAsync("Cancel Registration", "Are you sure you want to cancel this ?",
                MessageDialogStyle.AffirmativeAndNegative);
            if (res == MessageDialogResult.Affirmative)
            {
                NavigationService navigation = NavigationService.GetNavigationService(this);
                navigation.Navigate(new Uri("Employee.xaml", UriKind.Relative));
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            gridSource.DataContext = empAdd;
            Drop_Down_Rank();
            Drop_Down_Status();
            add_eventListener();

        }

        private void add_eventListener()
        {
            webHandler.POS_add_EmployeeCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Employee registered", "Done registring employees?",
                        MessageDialogStyle.AffirmativeAndNegative);
                    if (res == MessageDialogResult.Affirmative)
                    {
                        NavigationService navigation = NavigationService.GetNavigationService(this);
                        navigation.Navigate(new Uri("Employee.xaml", UriKind.Relative));
                    }
                }
                else
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Employee registration failed", "One or more incorect fields",
                        MessageDialogStyle.Affirmative);
                }
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

            };
        }
        private void Drop_Down_Rank()
        {
            webHandler.POS_Get_RankAsync();
            webHandler.POS_Get_RankCompleted += (x, y) =>
            {
                var result = y.Result;
                dropDownRank.RankCollection = new ObservableCollection<RankModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    dropDownRank.RankCollection.Add(new RankModel()
                    {
                        Rankid = int.Parse(arg[0]),
                        RankDescription = arg[1],
                    });
                }
                rank.ItemsSource = dropDownRank.RankCollection;
            };
        }

        private void add_TouchDown(object sender, RoutedEventArgs e)
        {
            if (pass.Password != string.Empty)
            {
                webHandler.POS_add_EmployeeAsync(empAdd.FirstName, empAdd.LastName, empAdd.Status.ToString(), empAdd.Phone, empAdd.Rank.ToString(), String.Format("{0:M/d/yyyy}", empAdd.Birthdate), empAdd.Email, pass.Password, empAdd.PIN, String.Format("{0:M/d/yyyy}", empAdd.JoinDate), empAdd.JobDescription, empAdd.Absent.ToString(), empAdd.Salary.ToString(), empAdd.DueNet.ToString());

            }

        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added )
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

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
    }
}
