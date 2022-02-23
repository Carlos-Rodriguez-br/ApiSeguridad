using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSeguridad._0._0_._Entities.Enviroment.Response
{
    public class SelectEnviroment
    {
        public int id { get; set; }
        public string nameEnviroment { get; set; }
        public int idProgram { get; set; }
        public string nameProgram { get; set; }
    }
}
