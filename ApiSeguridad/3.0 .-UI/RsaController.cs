using ApiSeguridad._2._0_._Business;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSeguridad._3._0_._UI
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RsaController : ControllerBase
    {
        private readonly Rsa context;

        public RsaController(Rsa context)
        {
            this.context = context;
        }

        /// <summary>
        /// Generate public key and private key
        /// </summary>
        [HttpPost("generateKey")]
        public async Task<ActionResult<string>> generateKey()
        {
            bool result=this.context.GenerateKeys();
            if (result) {
                return "Se crearon correctamente los archivos";
            }
            return "Los archivos ya existen";
            //return this.context.BuildTokenProgram(data[0], Utilities.Utilities.GetClaim(HttpContext, "id"), Utilities.Utilities.GetClaim(HttpContext, "name"), Utilities.Utilities.GetClaim(HttpContext, "e_mail"));
        }

    }
}
