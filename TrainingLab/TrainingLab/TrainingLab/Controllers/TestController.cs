using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingLab.Models;

namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\TraninngLab\\BackEnd";
        SQLiteConnection con = new SQLiteConnection("Data Source="+path+"\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();
        [HttpGet]
        public string GetCourses([FromQuery] string courseName,[FromQuery] string levelName)
        {
            cmd.Connection = con;
            con.Open();
            int size = 0;
            if (courseName==null)
            {
                cmd.CommandText = "select CourseName from Course";
                SQLiteDataReader dr = cmd.ExecuteReader();
                dr.Close();
                dr = cmd.ExecuteReader();
                StringBuilder sb = new StringBuilder();
                int i = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (i != 0)
                            sb.Append(",");
                        sb.Append("{\"CourseName\":\""+dr["CourseName"].ToString()+"\"}");
                        i++;
                    }
                }
                return "["+sb.ToString()+"]";
            }
            else
            {
                cmd.CommandText = "select l.LevelName,q.QuestionText,q.OptionList,q.CorrectAnswer from Test t inner join Course c on c.Id=t.CourseId inner join Questionnaire q on t.Id=q.TestId inner join Level l on l.Id=t.LevelId where c.CourseName='"+courseName+"' l.LevelName='"+levelName+"'";
                SQLiteDataReader dr = cmd.ExecuteReader();
                int i = 0;
                StringBuilder sb = new StringBuilder();
                sb.Append("[");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        if (i != 0)
                            sb.Append(",");
                        sb.Append("{\"question\":\"" + dr["QuestionText"].ToString() + "\"");
                        sb.Append(","+ dr["OptionList"].ToString());
                        sb.Append(",\"answer\":\"" + dr["CorrectAnswer"].ToString() + "\"}");                       
                        i++;
                    }
                }
                sb.Append("]");
                dr.Close();
                con.Close();
                return sb.ToString();
            }            
        }
        [HttpPost("score")]
        public async void postScores([FromBody] UserScore userScore)
        {
            cmd.CommandText = "INSERT INTO UserScore(Score,EmailId,";
        }

    }
}
