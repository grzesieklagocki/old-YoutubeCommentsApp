﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YouTubeComments.ValueConverters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public Visibility Null { get; set; }
        public Visibility NotNull { get; set; }


        public NullToVisibilityConverter()
        {
            Null = Visibility.Collapsed;
            NotNull = Visibility.Visible;
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Null : NotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
