using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ST10091324_PROG7312_Part1.Converter
{
    //  The following class was taken from StackOverflow and modified
    //  Posted by: Berty
    //  Available at: https://stackoverflow.com/questions/7000819/binding-a-buttons-visibility-to-a-bool-value-in-viewmodel
    public class StatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value as string;
            if (string.IsNullOrEmpty(status))
                return Brushes.Gray; // Default color if status is null or empty

            return status switch
            {
                "Completed" => Brushes.Green,  // Green for Completed
                "In Progress" => Brushes.Orange, // Orange for In Progress
                "Pending" => Brushes.Red, // Red for Pending
                _ => Brushes.Gray // Default gray color for other cases
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
