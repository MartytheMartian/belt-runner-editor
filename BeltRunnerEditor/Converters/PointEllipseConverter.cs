using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BeltRunnerEditor.Converters
{
    /// <summary>
    /// Converts point values for ellipses
    /// </summary>
    public class PointEllipseConverter : DependencyObject, IValueConverter
    {
        /// <summary>
        /// Converts the point for use in ellipses
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter to use</param>
        /// <param name="culture">Culture at runtime</param>
        /// <returns>Converted value</returns>
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Get the value
            double point = (double)value;

            // Adjust for the ellipse
            point -= 5;

            return point;
        }

        /// <summary>
        /// Converts the point value back
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="targetType">Target type</param>
        /// <param name="parameter">Parameter to use</param>
        /// <param name="culture">Culture at runtime</param>
        /// <returns>Converted value</returns>
        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            // Get the value
            double point = (double)value;

            // Adjust for the ellipse
            point += 5;

            return point;
        }
    }
}
