using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSeguridad._0._0_._Entities.General.Response
{
    public class ResponseTokenLogin
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
