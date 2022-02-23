using ApiSeguridad._0._0_._Data;
using ApiSeguridad._2._0_._Business.General.Response;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ApiSeguridad.Utilities
{
    public class Utilities
    {
        public enum KeySizes
        {
            SIZE_512 = 512,
            SIZE_1024 = 1024,
            SIZE_2048 = 2048,
            SIZE_952 = 952,
            SIZE_1369 = 1369,
        }

        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    try
                    {
                        if (!object.Equals(dr[prop.Name], DBNull.Value))
                        {
                            prop.SetValue(obj, dr[prop.Name], null);
                        }
                    }
                    catch (Exception e)
                    {
                        prop.SetValue(obj, null, null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }

        public static ResponseAll GetCat<T>(ValuesRepository repository, Dictionary<string, string> paramethers, string nameStore)
        {
            try
            {
                IEnumerable<T> res = new List<T>();
                repository.OpenStore(nameStore);
                foreach (KeyValuePair<string, string> kvp in paramethers)
                {
                    repository.AddParametherIn(kvp.Key, kvp.Value);
                }
                var reader = repository.GetValuesStore();
                res = DataReaderMapToList<T>(reader);
                reader.Close();
                return new ResponseAll
                {
                    error = false,
                    message = "Ok",
                    result = (IEnumerable<Object>)res
                };
            }
            catch (Exception e)
            {
                return new ResponseAll{ 
                    error=true,
                    message= e.Message,
                    result=null
                };
            }
            finally
            {
                repository.CloseStore();
            }
        }

        public static string GetClaim(HttpContext data, string nameClaim)
        {

            var identity = data.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                var value = identity.FindFirst(nameClaim).Value;
                return value;
            }
            return null;
        }

        public static string EncriptRsa(string publicKeyFile,string message)
        {
            byte[] encrypted;
            using (var rsa = new RSACryptoServiceProvider((int)KeySizes.SIZE_2048))
            {
                rsa.PersistKeyInCsp = false;
                string publicKey = File.ReadAllText(publicKeyFile);
                rsa.FromXmlString(publicKey);
                encrypted = rsa.Encrypt(Encoding.UTF8.GetBytes(message),true);
            }

            return BitConverter.ToString(encrypted);
        }

        public static string DecryptRsa(string privateKeyFile, string encrypted)
        {
            byte[] decrypted;
            using (var rsa = new RSACryptoServiceProvider((int)KeySizes.SIZE_2048))
            {

                rsa.PersistKeyInCsp = false;
                string privateKey = File.ReadAllText(privateKeyFile);
                rsa.FromXmlString(privateKey);
                decrypted = rsa.Decrypt(encrypted.Split('-').Select(b => Convert.ToByte(b, 16)).ToArray(), true);
            }
            return Encoding.UTF8.GetString(decrypted);
        }

    }
}
