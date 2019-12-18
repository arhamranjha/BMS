using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FYP1.Converter
{
    class DrCrMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                int par = int.Parse(parameter.ToString());
                var val = Decimal.Parse(values[0].ToString());
                string sat = values[1] as string;
                if (par == 0)
                {
                    if (sat == "Dr")
                    {
                        return val.ToString();
                    }
                    if (sat == "Cr")
                    {
                        return "0";

                    }
                }
                if (par == 1)
                {
                    if (sat == "Dr")
                    {
                        return "0";
                    }
                    if (sat == "Cr")
                    {
                        return val.ToString();
                    }
                }

                return "";
            }
            catch (Exception)
            {

                return "";
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return new object[] { DependencyProperty.UnsetValue };

        }


    }
}
