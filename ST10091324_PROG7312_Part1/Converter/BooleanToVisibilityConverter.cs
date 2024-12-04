using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace ST10091324_PROG7312_Part1.Converter
{
    // Modified from Blog post
    // Titled: WPF TextBlock
    // Posted by: Chand, M
    // Posted on: 14 November 2018
    // Available at: https://www.c-sharpcorner.com/uploadfile/mahesh/wpf-textblock/
    public class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            // In case the value is not a boolean, return Collapsed as default behavior
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // This method might not be needed unless you need to handle two-way binding
            return (value is Visibility visibility) && visibility == Visibility.Visible;
        }
    }
}
