using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;

namespace PPDesk.Helper.Converters
{
    public class PresenceStatusColorConverter : HelperConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool presence)
            {
                if (presence)
                {
                    return new SolidColorBrush(Colors.LightGreen);
                }
            }

            return new SolidColorBrush(Colors.Transparent);
        }
    }
}
