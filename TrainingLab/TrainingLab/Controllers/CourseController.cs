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
        public async Task<IActionResult> GetCourses(int id)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            if (id > 0)
            {
                cmd.CommandText = "select Count(*) from Chapter where CourseId='" + id + "'";
                SQLiteDataReader dr = cmd.ExecuteReader();
                int totalChapters = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        totalChapters = dr.GetInt32(0);
                    }
                }
                dr.Close();
                ChapterModel[] chapterModel = new ChapterModel[totalChapters];
                cmd.CommandText = "select * from Chapter where CourseId='" + id + "'";
                dr = cmd.ExecuteReader();
                int i = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        chapterModel[i] = new ChapterModel();
                        chapterModel[i].chapterId = dr.GetInt32(0);
                        chapterModel[i].chapterName = dr.GetString(1);
                        //chapterModel[i].topics=GetTopics(chapterModel[i].chapterId);
                        cmdd.CommandText = "select count(*) from Topic t inner join Chapter ch on ch.Id=t.ChapterId inner join Course c on c.Id=ch.CourseId where t.ChapterId='" + id + "'";
                        SQLiteDataReader sQLiteDataReader = cmdd.ExecuteReader();
                        int size = 0;
                        if (sQLiteDataReader.HasRows)
                        {
                            while (sQLiteDataReader.Read())
                            {
                                size = sQLiteDataReader.GetInt32(0);
                            }
                        }
                        sQLiteDataReader.Close();
                        cmdd.CommandText = "select * from Topic t inner join Chapter ch on ch.Id=t.ChapterId inner join Course c on c.Id=ch.CourseId where t.ChapterId='" + chapterModel[i].chapterId + "'";
                        sQLiteDataReader = cmdd.ExecuteReader();
                        int j = 0;
                        TopicModel[] topicModel = new TopicModel[size];
                        if (sQLiteDataReader.HasRows)
                        {
                            while (sQLiteDataReader.Read())
                            {
                                topicModel[j] = new TopicModel();
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
                return CreatedAtAction(nameof(GetCourses), chapterModel);
            }
            else
            {
                cmd.CommandText = "select count(*) from Course";
                SQLiteDataReader sQLiteDataReader = cmd.ExecuteReader();
                int size = 0;
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
                return CreatedAtAction(nameof(GetCourses), courseModel);
            }
        }
    }
}
