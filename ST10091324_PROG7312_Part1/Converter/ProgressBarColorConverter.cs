using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ST10091324_PROG7312_Part1.Converter
{
    // Modification from Blog post
    // Titled: Change the Color of ProgressBar in C#
    // Posted by: Marc
    // Posted in: 2017
    // Available at: https://www.csharp-console-examples.com/winform/change-the-color-of-progressbar-in-c/
    public class ProgressBarColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            switch (status)
            {
                case "Completed":
                    return new SolidColorBrush(Colors.Green);
                case "In Progress":
                    return new SolidColorBrush(Colors.Orange);
                case "Pending":
                    return new SolidColorBrush(Colors.Red);
                default:
                    return new SolidColorBrush(Colors.Gray);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
