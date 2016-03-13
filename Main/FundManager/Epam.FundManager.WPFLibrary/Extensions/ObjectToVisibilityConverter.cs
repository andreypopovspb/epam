using System;
using System.Windows;
using System.Windows.Data;

namespace Epam.FundManager.WPFLibrary.Extensions
{
    public class ObjectToVisibilityConverter : IValueConverter
    {
        #region IValueConverter

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool visible = (value is bool) ? (bool)value : !ReferenceEquals(value, null);
            return (visible) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException("The method or operation is not supported.");
        }

        #endregion
    }
}