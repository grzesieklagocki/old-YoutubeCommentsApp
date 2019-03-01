using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace YouTubeComments.ValueConverters
{
    public class BoolToVisibilityConverter : BoolConverter<Visibility>
    {
        public BoolToVisibilityConverter()
        {
            True = Visibility.Visible;
            False = Visibility.Collapsed;
        }
    }
}
