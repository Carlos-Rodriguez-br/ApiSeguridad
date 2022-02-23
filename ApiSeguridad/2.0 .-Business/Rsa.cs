using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace ApiSeguridad._2._0_._Business
{
    public class Rsa
    {
        private readonly IConfiguration _configuration;
        public enum KeySizes
        {
            SIZE_512 = 512,
            SIZE_1024 = 1024,
            SIZE_2048 = 2048,
            SIZE_952 = 952,
            SIZE_1369 = 1369,
        }

        public Rsa(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool GenerateKeys()
        {
            string publicKeyFile = Directory.GetCurrentDirectory() + _configuration["RouteKey"] +"publicKey.xml";
            string privateKeyFile = Directory.GetCurrentDirectory() + _configuration["RouteKey"] + "privateKey.xml";
            using (var rsa = new RSACryptoServiceProvider((int)KeySizes.SIZE_2048))
            {
                rsa.PersistKeyInCsp = false;
                if (File.Exists(privateKeyFile))
                    return false;
                if (File.Exists(publicKeyFile))
                    return false;
                string publicKey = rsa.ToXmlString(false);
                File.WriteAllText(publicKeyFile, publicKey);
                string privateKey = rsa.ToXmlString(true);
                File.WriteAllText(privateKeyFile, privateKey);
                return true;
            }
        }


    }
}
