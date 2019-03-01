using System;
using System.Globalization;
using System.Windows.Data;

namespace YouTubeComments.ValueConverters
{
    public abstract class BoolConverter<T> : IValueConverter
    {
        public T True { get; set; }
        public T False { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? True : False;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
