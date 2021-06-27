using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
//using WebApplication3;
using Microsoft.AspNetCore.Authorization;
using TrainingLab.Models;


namespace TrainingLab.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\Intellectual-bugs\\TrainingLab";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            _logger = logger;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }



        [HttpGet]
        [Route("auth")]
        public IEnumerable<string> GetData([FromQuery] string emailId, [FromQuery] string password)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);
            con.Open();
            string newpass = Crypto.Encryptor.Encrypt(password);
            cmd.CommandText = "SELECT * FROM User where EmailId='" + emailId + "' and Password ='" + newpass + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            List<string> studentData = new List<string>();
            int a = dr.FieldCount;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    studentData.Add("Name: " + dr["Name"].ToString());
                    studentData.Add("emailId: " + dr["EmailId"].ToString());
                    studentData.Add("password: " + Crypto.Encryptor.Decrypt(dr["Password"].ToString()));

                }
            }
            dr.Close();
            con.Close();
            return studentData;
        }



        // for signup
        [AllowAnonymous]
        [HttpPost]
        [Route("signup")]
        public IActionResult Create([FromBody] UserModel user)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = "INSERT INTO User(Name,EmailId,Password) VALUES('" + user.name + "','" + user.emailId + "','" + Crypto.Encryptor.Encrypt(user.password) + "')";
            int result = cmd.ExecuteNonQuery();
            user.password = Crypto.Encryptor.Encrypt(user.password);
            con.Close();
            return CreatedAtAction(nameof(Create), user);

        }


        [HttpPost]
        [Route("login")]
        public IActionResult Signin([FromBody] UserModel userModel)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);
            con.Open();
            string newpass = Crypto.Encryptor.Encrypt(userModel.password);
            cmd.CommandText = "SELECT * FROM User WHERE EmailId ='" + userModel.emailId + "' AND Password='" + newpass + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            UserModel user = new UserModel();
            int a = dr.FieldCount;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    user.emailId = dr.GetString(0);
                    user.name = dr.GetString(1);
                    user.password = dr.GetString(3);
                }
            }
            dr.Close();
            con.Close();
            return CreatedAtAction(nameof(Create), user);
            /*SQLiteDataReader dr = cmd.ExecuteReader();
             List<string> studentData = new List<string>();
             int a = dr.FieldCount;
             if (dr.HasRows)
             {
                 while (dr.Read())
                 {
                     studentData.Add("Name: " + dr["name"].ToString());
                     studentData.Add("email: " + dr["email"].ToString());
                     studentData.Add("password: " + Crypto.Encryptor.Decrypt(dr["password"].ToString()));

                 }
             }
             dr.Close();
             con.Close();
             return (IActionResult)studentData;*/


            con.Close();
            return CreatedAtAction(nameof(Signin), user);
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserModel user)
        {
            var token = jWTAuthenticationManager.Authenticate(user.emailId, user.password);

            if (token == null)
                return Unauthorized();

            return Ok(token);
        }

        /*public class UserCred
          {
              public string email { get; set; }
              public string password { get; set; }
          }*/


    }
}