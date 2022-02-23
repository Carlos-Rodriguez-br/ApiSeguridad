using ApiSeguridad._0._0_._Data;
using ApiSeguridad._0._0_._Entities.Enviroment.Response;
using ApiSeguridad._0._0_._Entities.General.Response;
using ApiSeguridad._2._0_._Business.General.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ApiSeguridad._2._0_._Business
{
    public class Enviroment
    {
        private ValuesRepository _valuesRepository;
        private readonly IConfiguration _configuration;

        public Enviroment(ValuesRepository valuesRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _valuesRepository = valuesRepository;
        }

        public ResponseAll GetEnviroments(int idProgram)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("p_idProgram", idProgram.ToString());
            return ApiSeguridad.Utilities.Utilities.GetCat<SelectEnviroment>(_valuesRepository, data, "getAmbientes");
        }

        public List<SelectEnv> SelectEnviroment(int idEnviroment)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("p_idEnviromentProgram", idEnviroment.ToString());
            ResponseAll resultEnv = ApiSeguridad.Utilities.Utilities.GetCat<SelectEnv>(_valuesRepository, data, "spc_selectAmbiente");
            if (resultEnv.error)
            {
                throw new Exception(resultEnv.message);
            }
            return (List<SelectEnv>)resultEnv.result;
        }

        public ResponseTokenLogin BuildTokenProgram(SelectEnv dataEnv,string idUser,string name,string email)
        {
            
            string publicKeyFile = Directory.GetCurrentDirectory() + _configuration["RouteKeyPublic"];
            //string privateKeyFile = Directory.GetCurrentDirectory() + _configuration["RouteKey"] + "privateKey.xml";
            var claims = new[]
            {
                new Claim("idUser",idUser),
                new Claim("name",name),
                new Claim("e_mail",email),
                new Claim("connection",ApiSeguridad.Utilities.Utilities.EncriptRsa(publicKeyFile, dataEnv.stringConnection)),
                new Claim("idProgram",dataEnv.idProgram.ToString()),
                new Claim("nameEnviroment",dataEnv.nameEnviroment),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };
            //string conexxionCifrada=ApiSeguridad.Utilities.Utilities.EncriptRsa(publicKeyFile, dataEnv.stringConnection);
            //string conexxionNoCifrada = ApiSeguridad.Utilities.Utilities.DecryptRsa(privateKeyFile, conexxionCifrada);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["KeyJWT"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddHours(Int32.Parse(_configuration["TimeHoursValidToken"]));
            
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds);
            return new ResponseTokenLogin()
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = expiration
            };
        }

    }
}
