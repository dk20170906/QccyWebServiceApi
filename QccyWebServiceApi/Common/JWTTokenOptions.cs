using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QccyWebServiceApi.Common
{
    public class JWTTokenOptions
    {
        //谁颁发的
        public string Issuer { get; set; } = "server";

        //颁发给谁
        public string Audience { get; set; } = "client";

        //令牌密码
        public string SecurityKey { get; private set; } = "lkjlkj";
        /// <summary>
        /// 过期时间
        /// </summary>
        public int Expires { get; set; }

        //修改密码，重新创建数字签名
        public void SetSecurityKey(string value)
        {
            SecurityKey = value;

            CreateKey();
        }

        //对称秘钥
        public SymmetricSecurityKey Key { get; set; }

        //数字签名
        public SigningCredentials Credentials { get; set; }

        public JWTTokenOptions(string Issuer, string Audience, string SecurityKey, int Expires)
        {
            this.Issuer = Issuer;
            this.Audience = Audience;
            this.SecurityKey = SecurityKey;
            this.Expires = Expires;
            CreateKey();
        }

        private void CreateKey()
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey));
            Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
        }
        public string GetToken(string username)
        {
            //创建用户身份标识
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Sid, username),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "user"),
            };

            //创建令牌
            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(Expires),
                signingCredentials: Credentials
                );

            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

            return jwtToken;
        }

        public string TokenToSignature(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return "";

            byte[] bytes = Convert.FromBase64String(token);
            var means = 14;// (timestamp + nonce) lenght = 14
            var clearText = Encoding.UTF8.GetString(bytes);
            return clearText.Substring(means);
        }

    }
}
