using System;

namespace PPDesk.Helper.Converters
{
    public abstract class HelperConverter
    {
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
