using ApiSeguridad._0._0_._Data;
using ApiSeguridad._0._0_._Entities.General.Response;
using ApiSeguridad._2._0_._Business.General.Response;
using ApiSeguridad._2._0_._Entities.Auth.Inputs;
using ApiSeguridad._2._0_._Entities.Auth.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiSeguridad._2._0_._Business
{
    public class Auth
    {
        private ValuesRepository _valuesRepository;
        private readonly IConfiguration _configuration;

        public Auth(ValuesRepository valuesRepository, IConfiguration configuration)
        {
            _configuration = configuration;
            _valuesRepository = valuesRepository;
        }

        public List<ResponseLogin> GetUser(Credentials credential)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("p_email", credential.email);
            data.Add("p_pass", credential.password);
            ResponseAll resultUser = ApiSeguridad.Utilities.Utilities.GetCat<ResponseLogin>(_valuesRepository, data, "spr_login");
            if (resultUser.error)
            {
                throw new Exception(resultUser.message);
            }
            ResponseAll result = (ApiSeguridad.Utilities.Utilities.GetCat<ResponseLogin>(_valuesRepository, data, "spr_login"));

            return (List<ResponseLogin>)result.result;
        }

        public ResponseTokenLogin BuildToken(ResponseLogin userInfo)
        {
            var claims = new[]
            {
                new Claim("id",userInfo.id.ToString()),
                new Claim("name",userInfo.name),
                new Claim("e_mail",userInfo.email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
            };

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
