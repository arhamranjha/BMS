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
    /// Interaction logic for testchartpage.xaml
    /// </summary>
    public partial class testchartpage : Page
    {
        public testchartpage()
        {
            InitializeComponent();
            this.DataContext =new TestPageViewModel();
        }
    }
}
