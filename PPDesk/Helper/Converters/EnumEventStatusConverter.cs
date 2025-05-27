using Microsoft.UI.Xaml.Data;
using PPDesk.Abstraction.Enum;
using PPDesk.Abstraction.Helper;
using System;

namespace PPDesk.Helper.Converters
{
    public class EnumEventStatusConverter : HelperConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is EnumEventStatus status)
            {
                return status switch
                {
                    EnumEventStatus.Draft => "Bozza",
                    EnumEventStatus.Live => "Live",
                    EnumEventStatus.Started => "Partito",
                    EnumEventStatus.Ended => "Finito",
                    EnumEventStatus.Completed => "Completato",
                    EnumEventStatus.Canceled => "Cancellato",
                    _ => status.ToString()
                };
            }

            return string.Empty;
        }
    }
}
