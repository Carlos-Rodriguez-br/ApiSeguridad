using ApiSeguridad._0._0_._Entities.Enviroment.Inputs;
using ApiSeguridad._0._0_._Entities.Enviroment.Response;
using ApiSeguridad._0._0_._Entities.General.Response;
using ApiSeguridad._2._0_._Business;
using ApiSeguridad._2._0_._Business.General.Response;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiSeguridad._3._0_._UI
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EnviromentController : ControllerBase
    {
        private readonly Enviroment context;

        public EnviromentController(Enviroment context)
        {
            this.context = context;
        }

        /// <summary>
        /// Get all environments of a program
        /// </summary>
        [HttpGet("getEnviroments/{idProgram}")]
        public ActionResult<ResponseAll> getEnviroments(int idProgram)
        {
            return this.context.GetEnviroments(idProgram);
        }

        /// <summary>
        /// Get string connection of a enviroment
        /// </summary>
        [HttpPost("selectEnviroment")]
        public async Task<ActionResult<ResponseTokenLogin>> selectEnviroment([FromBody] InputSelectAmbiente model)
        {
            List<SelectEnv> data = this.context.SelectEnviroment(model.idEnviroment);
            if (data.Count == 0)
            {
                return NotFound("No se encontro el ambiente indicado");
            }

            return this.context.BuildTokenProgram(data[0], Utilities.Utilities.GetClaim(HttpContext,"id"), Utilities.Utilities.GetClaim(HttpContext, "name"), Utilities.Utilities.GetClaim(HttpContext, "e_mail"));
        }


        }
}
