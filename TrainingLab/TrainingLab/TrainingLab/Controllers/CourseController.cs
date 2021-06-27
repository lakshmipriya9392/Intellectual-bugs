using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingLab.Models;


namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : Controller
    {
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\Intellectual-bugs\\TrainingLab";
        SQLiteConnection con = new SQLiteConnection("Data Source="+path+"\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();

        [HttpGet]
        public IEnumerable<CourseModel> GetCourses()
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            cmd.CommandText = "select count(*) from Course";
            SQLiteDataReader dr = cmd.ExecuteReader();
            int size = 0;
            if(dr.HasRows)
            {
                while(dr.Read())
                {
                    size = dr.GetInt32(0);
                }
            }
            
            dr.Close();
            cmd.CommandText = "select * from Course";
            dr = cmd.ExecuteReader();
            int i = 0;
            CourseModel[] courseModel = new CourseModel[size];
            
            if(dr.HasRows)
            {
                while(dr.Read())
                {
                    courseModel[i] = new CourseModel();
                    courseModel[i].CourseId= int.Parse(dr["Id"].ToString());
                    courseModel[i].CourseName = dr["CourseName"].ToString();
                    courseModel[i].AuthorName = dr["AuthorName"].ToString();
                    courseModel[i].Chapters = getChapters(courseModel[i].CourseId);                   
                    i++;
                }
            }
            dr.Close();
            con.Close();
            return courseModel;
        }
        public string getChapters(int id)
        {
            StringBuilder sb = new StringBuilder();
            cmdd.CommandText = "select * from Chapter where CourseId='" + id+ "'";
            SQLiteDataReader drr = cmdd.ExecuteReader();
            sb.Append("{");
            int j = 0;
            if (drr.HasRows)
            {
                while (drr.Read())
                {
                    if (j != 0)
                        sb.Append(",");
                    sb.Append(drr["ChapterName"].ToString());
                    j++;
                }
            }
            sb.Append("}");
            drr.Close();
            return sb.ToString();
        }
        [HttpGet("chapter")]
        public IEnumerable<ChapterModel> GetChapters([FromQuery] string course)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            cmd.CommandText = "select count(*) from Chapter ch inner join Course c on c.Id=ch.CourseId where c.CourseName='" + course + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            int size = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    size = dr.GetInt32(0);
                }
            }
            dr.Close();
            cmd.CommandText = "select * from Chapter ch inner join Course c on c.Id=ch.CourseId where c.CourseName='"+course+"'";
            dr = cmd.ExecuteReader();
            int i = 0;
            ChapterModel[] chapterModel = new ChapterModel[size];           
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    chapterModel[i] = new ChapterModel();
                    chapterModel[i].ChapterId= int.Parse(dr["Id"].ToString());                   
                    chapterModel[i].Topics = getTopics(chapterModel[i].ChapterId);                   
                    i++;
                }
            }
            dr.Close();
            con.Close();
            return chapterModel;
        }
        public string getTopics(int id)
        {
            StringBuilder sb = new StringBuilder();
            cmdd.CommandText = "select * from Topic where ChapterId='" + id + "'";
            SQLiteDataReader drr = cmdd.ExecuteReader();
            sb.Append("{");
            int j = 0;
            if (drr.HasRows)
            {
                while (drr.Read())
                {
                    if (j != 0)
                        sb.Append(",");
                    sb.Append(drr["TopicName"].ToString());
                    j++;
                }
            }
            sb.Append("}");
            drr.Close();
            return sb.ToString();
        }
        [HttpGet("topic")]
        public IEnumerable<TopicModel> GetTopics([FromQuery] string course,[FromQuery] string chapter)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            cmd.CommandText = "select count(*) from Topic t inner join Chapter ch on ch.Id=t.ChapterId inner join Course c on c.Id=ch.CourseId where c.CourseName='" + course + "' and ch.ChapterName='" + chapter + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            int size = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    size = dr.GetInt32(0);
                }
            }
            dr.Close();
            cmd.CommandText = "select * from Topic t inner join Chapter ch on ch.Id=t.ChapterId inner join Course c on c.Id=ch.CourseId where c.CourseName='" + course + "' and ch.ChapterName='"+chapter+"'";
            dr = cmd.ExecuteReader();
            int i = 0;
            TopicModel[] topicModel = new TopicModel[size];
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    topicModel[i] = new TopicModel();
                    topicModel[i].TopicId= int.Parse(dr["Id"].ToString());
                    topicModel[i].TopicName = dr["TopicName"].ToString();
                    topicModel[i].VideoURL = dr["VideoURL"].ToString();
                    topicModel[i].NotesURL = dr["NotesURL"].ToString();
                    i++;
                }
            }
            dr.Close();
            con.Close();
            return topicModel;
        }
       
    }
}
