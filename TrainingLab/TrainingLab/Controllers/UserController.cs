using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TrainingLab.Models;
using TrainingLab.Services;

namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\TraninngLab\\BackEnd";
        SQLiteConnection con = new SQLiteConnection("Data Source="+path+"\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
       // private readonly IJwtAuthenticationManager jWTAuthenticationManager;

        //private readonly ILogger<UserController> _logger;

        //public UserController(ILogger<UserController> logger, IJwtAuthenticationManager jWTAuthenticationManager)
        //{
        //    _logger = logger;
        //    this.jWTAuthenticationManager = jWTAuthenticationManager;
        //}

        [HttpGet]
        [Route("auth")]
        public UserModel GetData([FromQuery] string email, [FromQuery] string password)
        {

            SQLiteCommand cmd = new SQLiteCommand(con);
            con.Open();
            string newPassword = ServiceFile.Encryptor.Encrypt(password);
            cmd.CommandText = "SELECT * FROM User where EmailId='" + email + "' and Password ='" + newPassword + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            UserModel userModel = new UserModel();
            int a = dr.FieldCount;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    userModel.emailId = dr.GetString(0);
                    userModel.name = dr.GetString(1);
                    userModel.password = dr.GetString(3);
                    userModel.contactNo = dr.GetDecimal(2);
                }
            }
            dr.Close();
            con.Close();
            return userModel;
        }



        // for signup
        [HttpPost]
        [Route("signup")]
        public IActionResult Create([FromBody] UserModel userModel)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);
            con.Open();
            cmd.CommandText = "INSERT INTO User(Name,EmailId,Password,ContactNo) VALUES('" + userModel.name + "','" + userModel.emailId + "','" + ServiceFile.Encryptor.Encrypt(userModel.password) + "','"+userModel.contactNo+"')";
            int result = cmd.ExecuteNonQuery();
            con.Close();
            return CreatedAtAction(nameof(Create), userModel);

        }


        [HttpPost]
        [Route("login")]
        public IActionResult Signin([FromBody] UserModel userModel)
        {
            SQLiteCommand cmd = new SQLiteCommand(con);
            con.Open();
            string newpass = ServiceFile.Encryptor.Encrypt(userModel.password);
            cmd.CommandText = "SELECT * FROM User WHERE EmailId ='" + userModel.emailId + "' AND Password='" + newpass + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            List<string> userData = new List<string>();
            UserModel user = new UserModel();
            int a = dr.FieldCount;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    user.emailId = dr.GetString(0);
                    user.name = dr.GetString(1);
                    user.password = dr.GetString(3);
                    user.contactNo = dr.GetDecimal(2);
                }
            }
            dr.Close();
            con.Close();
            return CreatedAtAction(nameof(Create),user);
        }

        //   [AllowAnonymous]
       // [HttpPost("authenticate")]
        //public IActionResult Authenticate([FromBody] UserModel userModel)
        //{
        //    var token = jWTAuthenticationManager.Authenticate(userModel.emailId, userModel.password);

        //    if (token == null)
        //        return Unauthorized();

        //    return Ok(token);
        //}


    }

}

