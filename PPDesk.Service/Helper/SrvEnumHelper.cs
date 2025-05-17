using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Helper
{
    public static class SrvEnumHelper
    {
        public static T ConvertStringToEnum<T>(string value) where  T : struct,  Enum
        {
            value = value[0].ToString().ToUpper() + value.Substring(1);

            if(Enum.TryParse<T>(value, false, out var result))
            {
                return result;
            }

            return default(T);
        }
    }
}
