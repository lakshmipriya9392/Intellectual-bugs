using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using TrainingLab.Models;
using System.Configuration;

namespace TrainingLab.Services
{
    public class UserService
    {

        private static Lazy<UserService> Initializer = new Lazy<UserService>(() => new UserService());
        public static UserService Instance => Initializer.Value;
        SQLiteConnection con = new SQLiteConnection("Data Source=" + DBConnection.path);
        public bool SignUp(UserModel user)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select count(*) from User where EmailId='" + user.emailId + "'";
            int userCount = int.Parse(cmd.ExecuteScalar().ToString());
            if (userCount > 0)
            {
                con.Close();
                return false;             
            }
            else
            {
                cmd.CommandText = "INSERT INTO User(Name,EmailId,Password) VALUES('" + user.name + "','" + user.emailId + "','" + Crypto.Encryptor.Encrypt(user.password) + "')";
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
        }
        public string SignIn(UserModel userModel)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
           con.Open();
            string newPassword = Crypto.Encryptor.Encrypt(userModel.password);
            UserModel user = new UserModel();            
                cmd.CommandText = "SELECT * FROM User WHERE EmailId ='" + userModel.emailId + "' AND Password='"+newPassword+"'";
                SQLiteDataReader dr = cmd.ExecuteReader();
                string userName = null;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        userName= dr.GetString(1);                       
                    }
                }
            dr.Close();
            con.Close();
            return userName;                                
        }
    }
}
