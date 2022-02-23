using ApiSeguridad._0._0_._Entities.General.Response;
using ApiSeguridad._2._0_._Business;
using ApiSeguridad._2._0_._Business.General.Response;
using ApiSeguridad._2._0_._Entities.Auth.Inputs;
using ApiSeguridad._2._0_._Entities.Auth.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiSeguridad._3._0_._UI
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly Auth context;

        public AuthController(Auth context)
        {
            this.context = context;
        }

        /// <summary>
        /// Validate user credentials
        /// </summary>
        /// <response code="404">If user not found</response>
        /// <response code="500">If there is a server error</response>
        [HttpPost("login")]
        public async Task<ActionResult<ResponseTokenLogin>> login([FromBody] Credentials model)
        {
            List<ResponseLogin> data = this.context.GetUser(model);
            if (data.Count==0) {
                return NotFound("El usuario o la contraseña son incorrectos");
            }
            return this.context.BuildToken(data[0]);
        }

    }
}
