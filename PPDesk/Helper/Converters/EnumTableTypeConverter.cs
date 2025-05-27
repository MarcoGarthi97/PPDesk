using Microsoft.UI.Xaml.Data;
using PPDesk.Abstraction.Enum;
using System;

namespace PPDesk.Helper.Converters
{
    public class EnumTableTypeConverter : HelperConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is EnumTableType type)
            {
                return type switch
                {
                    EnumTableType.SessioneGdr => "Sessione Gdr",
                    EnumTableType.Multitavolo => "Multi Tavolo",
                    _ => type.ToString()
                };
            }

            return string.Empty;
        }
    }
}
