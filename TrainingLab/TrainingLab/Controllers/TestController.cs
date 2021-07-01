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
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\Intellectual-bugs\\TrainingLab";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();

        [HttpGet]
        public IActionResult Get([FromQuery] string id, [FromQuery] string levelName)
        {
            cmd.Connection = con;
            con.Open();
            int size = 0;
            if (id == null)
            {
                cmd.CommandText = "select count(*) from Course";
                SQLiteDataReader sQLiteDataReader = cmd.ExecuteReader();
                if (sQLiteDataReader.HasRows)
                {
                    while (sQLiteDataReader.Read())
                    {
                        size = sQLiteDataReader.GetInt32(0);
                    }
                }
                sQLiteDataReader.Close();
                cmd.CommandText = "select * from Course";
                sQLiteDataReader = cmd.ExecuteReader();
                int i = 0;
                CourseModel[] courseModel = new CourseModel[size];

                if (sQLiteDataReader.HasRows)
                {
                    while (sQLiteDataReader.Read())
                    {
                        courseModel[i] = new CourseModel();
                        courseModel[i].CourseId = int.Parse(sQLiteDataReader["Id"].ToString());
                        courseModel[i].CourseName = sQLiteDataReader["CourseName"].ToString();
                        courseModel[i].AuthorName = sQLiteDataReader["AuthorName"].ToString();
                        courseModel[i].imageURL = sQLiteDataReader["ImageURL"].ToString();
                        i++;
                    }
                }
                sQLiteDataReader.Close();
                con.Close();
                return CreatedAtAction(nameof(Get), courseModel);
            }
            else
            {
                cmd.CommandText = "select l.LevelName,q.QuestionText,q.OptionList,q.CorrectAnswer from Test t inner join Course c on c.Id=t.CourseId inner join Questionnaire q on t.Id=q.TestId inner join Level l on l.Id=t.LevelId where c.Id='" + id + "' and l.LevelName='" + levelName + "'";
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
                        sb.Append("," + dr["OptionList"].ToString());
                        sb.Append(",\"answer\":\"" + dr["CorrectAnswer"].ToString() + "\"}");
                        i++;
                    }
                }
                sb.Append("]");
                dr.Close();
                con.Close();
                return CreatedAtAction(nameof(Get), sb.ToString());
            }
        }


    }
}
