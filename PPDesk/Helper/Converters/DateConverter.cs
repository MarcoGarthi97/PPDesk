using Microsoft.UI.Xaml.Data;
using PPDesk.Abstraction.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Helper.Converters
{
    public class DateConverter : HelperConverter, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                if (date == DateTime.MinValue || date == new DateTime(1, 1, 1))
                {
                    return "-";
                }

                return date.ToString("dd/MM/yyyy HH:mm");
            }

            return "-";
        }
    }
}
