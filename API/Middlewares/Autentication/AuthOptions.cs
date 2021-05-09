using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Middlewares.Autentication
{
    public class AuthOptions
    {
        public string Issuer { get; set; } = "MyAuthServer"; // издатель токена
        public string Audience { get; set; } //= "MyAuthClient"; // потребитель токена
        public string SecretKey { get; set; } //= "mysupersecret_secretkey!123";   // ключ для шифрации
        public int LifeTime { get; set; } // время жизни токена - 1 минута
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string secretKey)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
        }
    }
}
