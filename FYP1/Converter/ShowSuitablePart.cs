using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using FYP1.Models;

namespace FYP1.Converter
{
    public class ShowSuitablePart : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return " ";
                else
                    return value.ToString();
            }
            catch (Exception)
            {

                return " ";
            }

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return value.ToString();

            }
            catch (Exception)
            {
                return DependencyProperty.UnsetValue;

            }
        }


    }
}
