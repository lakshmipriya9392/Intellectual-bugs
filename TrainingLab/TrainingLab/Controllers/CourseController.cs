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
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\Intellectual-bugs";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();

        [HttpGet]
        public async Task<IActionResult> GetCourses(string id)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            if (id==null)
            {
                return CreatedAtAction(nameof(GetCourses), await GetCourseDetails());
            }
            else
            {
                return CreatedAtAction(nameof(GetCourses), await GetCourseTopics(id));                
            }
        }

        public async Task<List<ChapterModel>> GetCourseTopics(string id)
        {
            List<ChapterModel> chapterModel = new List<ChapterModel>();
            cmd.CommandText = "select * from Chapter where CourseId='" + id + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            int i = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    chapterModel.Add(new ChapterModel());
                    chapterModel[i].chapterId = dr.GetInt32(0);
                    chapterModel[i].chapterName = dr.GetString(1);
                    //chapterModel[i].topics=GetTopics(chapterModel[i].chapterId);

                    cmdd.CommandText = "select * from Topic t inner join Chapter ch on ch.Id=t.ChapterId inner join Course c on c.Id=ch.CourseId where t.ChapterId='" + chapterModel[i].chapterId + "'";
                    SQLiteDataReader sQLiteDataReader = cmdd.ExecuteReader();
                    int j = 0;
                    List<TopicModel> topicModel = new List<TopicModel>();
                    if (sQLiteDataReader.HasRows)
                    {
                        while (sQLiteDataReader.Read())
                        {
                            topicModel.Add(new TopicModel());
                            topicModel[j].TopicId = int.Parse(sQLiteDataReader["Id"].ToString());
                            topicModel[j].TopicName = sQLiteDataReader["TopicName"].ToString();
                            topicModel[j].VideoURL = sQLiteDataReader["VideoURL"].ToString();
                            topicModel[j].NotesURL = sQLiteDataReader["NotesURL"].ToString();
                            j++;
                        }
                    }
                    chapterModel[i].topics = topicModel;
                    sQLiteDataReader.Close();
                    i++;
                }
            }
            return chapterModel;
        }

        public async Task<List<CourseModel>> GetCourseDetails()
        {
            cmd.CommandText = "select * from Course";
            SQLiteDataReader sQLiteDataReader = cmd.ExecuteReader();
            int i = 0;
            List<CourseModel> courseModel = new List<CourseModel>();

            if (sQLiteDataReader.HasRows)
            {
                while (sQLiteDataReader.Read())
                {
                    courseModel.Add(new CourseModel());
                    courseModel[i].CourseId = int.Parse(sQLiteDataReader["Id"].ToString());
                    courseModel[i].CourseName = sQLiteDataReader["CourseName"].ToString();
                    courseModel[i].AuthorName = sQLiteDataReader["AuthorName"].ToString();
                    courseModel[i].imageURL = sQLiteDataReader["ImageURL"].ToString();
                    i++;
                }
            }
            sQLiteDataReader.Close();
            con.Close();
            return courseModel;
        }
    }
}
