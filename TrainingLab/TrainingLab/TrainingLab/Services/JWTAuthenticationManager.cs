/*using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace TrainingLab
{
    public interface IJWTAuthenticationManager
    {
        string Authenticate(string emailId, string password);
    }

    public class JWTAuthenticationManager : IJWTAuthenticationManager
    {
        private readonly string tokenKey;

        public JWTAuthenticationManager(string tokenKey)
        {
            this.tokenKey = tokenKey;
        }

        public string Authenticate(string emailId, string password)
        {

            SQLiteConnection con = new SQLiteConnection("Data Source=C:\\Users\\HIMANI\\Downloads\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
            SQLiteCommand cmd = new SQLiteCommand(con);
            con.Open();
            string newpass = Crypto.Encryptor.Encrypt(password);
            cmd.CommandText = "SELECT * FROM User where EmailId='" + emailId + "' and Password ='" + newpass + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            List<string> studentData = new List<string>();
            if (!dr.HasRows)
            {

                return null;

            }

           
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tokenKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, emailId)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}*/