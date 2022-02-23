using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSeguridad._2._0_._Business.General.Response
{
    public class ResponseAll
    {
        public bool error { get; set; }

        public string message { get; set; }
        public IEnumerable<Object> result { get; set; }
    }
}
