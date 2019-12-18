using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FYP1.Converter
{
    class BalanceToStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (decimal.Parse(value.ToString()) > 0 || decimal.Parse(value.ToString()) == 0)
                {
                    return "Dr";
                }
                if (decimal.Parse(value.ToString()) < 0)
                {
                    return "Cr";
                }
                return "";

            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
