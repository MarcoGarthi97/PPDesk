using PPDesk.Abstraction.DTO.Response.Eventbride;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PPDesk.Service.Storages.Eventbride
{
    public static class SrvETokenStorage
    {
        public static string Bearer { get; private set; }
        public static string Code { get; private set; }

        public static void SetBearer(string bearer)
        {
            Bearer = bearer;
        }

        public static void SetCode(string code)
        {
            Code = code;
        }
    }
}
