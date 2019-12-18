using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for Employee.xaml
    /// </summary>
    public partial class Employee : Page
    {
        private ServiceSoapClient webHandler = new ServiceSoapClient("ServiceSoap");
        private EmployeeModel employee;
        private EmployeeModel selected = new EmployeeModel();
        private CollectionViewSource CVS;
        private ListCollectionView bindlistview;
        private StatusModel dropDownStatus = new StatusModel();
        private RankModel dropDownRank = new RankModel();
        private int count;

        public Employee()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            employee = (EmployeeModel)this.FindResource("emp");
            POS_Employee();
            Drop_Down_Status();
            Drop_Down_Rank();
            webHandler.POS_update_EmployeeCompleted += async (x, y) =>
            {
                if (y.Result == 1)
                {
                    var win = (MetroWindow)Application.Current.MainWindow;
                    var res = await win.ShowMessageAsync("Employee changes saved", "",
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

        private void POS_Employee()
        {
            webHandler.POS_Get_EmployeeAsync();
            webHandler.POS_Get_EmployeeCompleted += (x, y) =>
            {
                var result = y.Result;
                employee.EmployeesCollection = new ObservableCollection<EmployeeModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    employee.EmployeesCollection.Add(new EmployeeModel()
                    {
                        ID = int.Parse(arg[0]),
                        FirstName = arg[1],
                        LastName = arg[2],
                        FullName = arg[1] + " " + arg[2],
                        StatusDesc = arg[3],
                        Phone = arg[4],
                        RankDesc = arg[5],
                        Birthdate = DateTime.ParseExact(arg[6], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) ,
                        Email = arg[7],
                        Password = arg[8],
                        PIN = arg[9],
                        JoinDate = DateTime.ParseExact(arg[10], "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                        StringJoinDate = arg[10],
                        JobDescription = arg[11],
                        Absent = int.Parse(arg[12]),
                        Salary = double.Parse(arg[13]),
                        DueNet = double.Parse(arg[14]),
                        Rank = int.Parse(arg[15]),
                        Status = int.Parse(arg[16])
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
            };
        }
        private void Drop_Down_Rank()
        {
            webHandler.POS_Get_RankAsync();
            webHandler.POS_Get_RankCompleted+= (x, y) =>
            {
                var result = y.Result;
                dropDownRank.RankCollection = new ObservableCollection<RankModel>();
                string[] arr = result.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in arr)
                {
                    string[] arg = item.Split(new string[] { "---" }, StringSplitOptions.None);
                    dropDownRank.RankCollection.Add(new RankModel()
                    {
                        Rankid =int.Parse(arg[0]),
                        RankDescription = arg[1],
                    });
                }
                rank.ItemsSource = dropDownRank.RankCollection;
            };
        }
        private void backButton_TouchUp(object sender, RoutedEventArgs e)
        {

            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("Dashboard.xaml", UriKind.Relative));

        }

     

        private void SearchBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (search.Text != string.Empty && bindlistview != null)
            {
                bindlistview.Filter = delegate (object cust) { return ((EmployeeModel)cust).FullName.ToLower().Contains(search.Text.ToLower()); };
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
                bindlistview.Filter = delegate (object cust) { return ((EmployeeModel)cust).StatusDesc == "Active"; };
            }
            else { bindlistview.Filter = null; }
        }

        private async void productList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                selected = (EmployeeModel)productList.SelectedItem;
                if (selected != null)
                {
                    update.IsEnabled = true;
                }
            }
            catch (Exception)
            {
                update.IsEnabled = false;
                update.Foreground = Brushes.Gray;
                var win = (MetroWindow)Application.Current.MainWindow;
                var res = await win.ShowMessageAsync("Something went wrong", "Try again in a little bit",
                    MessageDialogStyle.AffirmativeAndNegative);
            }
        }

        private void DatePicker_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void add_TouchDown(object sender, RoutedEventArgs e)
        {
            NavigationService navigation = NavigationService.GetNavigationService(this);
            navigation.Navigate(new Uri("AddEmployee.xaml", UriKind.Relative));
        }

        private void update_TouchDown(object sender, RoutedEventArgs e)
        {
            webHandler.POS_update_EmployeeAsync(selected.ID, selected.FirstName, selected.LastName, selected.Status.ToString(), selected.Phone, selected.Rank.ToString(), String.Format("{0:M/d/yyyy}", selected.Birthdate), selected.Email, selected.Password, selected.PIN, String.Format("{0:M/d/yyyy}", selected.JoinDate), selected.JobDescription, selected.Absent.ToString(), selected.Salary.ToString(), selected.DueNet.ToString());

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
    }
}
