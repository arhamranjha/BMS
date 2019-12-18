using System;
using System.Collections.Generic;
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
using FYP1.Models;
using MahApps.Metro.Controls;

namespace FYP1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            //this.Cursor = Cursors.None;

        }

        private void ManipulationBoundaryFeedbackHandler(object sender, ManipulationBoundaryFeedbackEventArgs e)
        {
            e.Handled = true;
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ItemViewModel _itemviewmodel = new ItemViewModel();
            clock.DataContext = _itemviewmodel;
            setTime(_itemviewmodel);
        }

        private void setTime(ItemViewModel _itemviewmodel)
        {
            DispatcherTimer timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                _itemviewmodel.Time = DateTime.Now.ToString("HH:mm:ss");
            }, this.Dispatcher);
        }
    }
}
