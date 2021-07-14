using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using TrainingLab.Models;
using TrainingLab.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using TrainingLab.Controllers;
using TrainingLab.Services;

namespace TrainingLab.Services
{
    public class CourseService
    {
        private static Lazy<CourseService> Initializer = new Lazy<CourseService>(() => new CourseService());
        public static CourseService Instance => Initializer.Value;
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Startup.connectionString);
        private string loadLocation = "";
        private string isCacheData = "";
        SQLiteDataReader dr;
        public async Task<IEnumerable> GetCourses(string id)
        {
            if (id == null)
            {
                CourseController.recordKey = "Course_" + DateTime.Now.ToString("yyyyMMdd_hh");
                List<CourseModel> courseModel = await GetCourseDetails();
                return courseModel;
            }
            else
            {
                CourseController.recordKey = "Chapter_"+id+"_" + DateTime.Now.ToString("yyyyMMdd_hh");
                List<ChapterModel> chapterModel =await GetCourseTopics(id);
                return chapterModel;
            }
        }
        public async Task<List<ChapterModel>> GetCourseTopics(string id)
        {
            List<ChapterModel> chapterModel;
            IDistributedCache cache = CourseController._distributedCache;
            loadLocation = null;
            string recordKey = CourseController.recordKey;
            //Getting data from cache
            chapterModel =  await cache.GetRecordAsync<List<ChapterModel>>(recordKey);
            if (chapterModel is null)
            {
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    SQLiteCommand cmdd = new SQLiteCommand();
                    cmd.Connection = con;
                    cmdd.Connection = con;

                    cmd.CommandText = "select * from Chapter where CourseId='" + id + "'";
                    con.Open();
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    int i = 0;
                    chapterModel = new List<ChapterModel>();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            chapterModel.Add(new ChapterModel());
                            chapterModel[i].chapterId = dr.GetInt32(0);
                            chapterModel[i].chapterName = dr.GetString(1);
                            cmdd.CommandText = "select * from Topic t inner join Chapter ch on ch.Id=t.ChapterId inner join Course c on c.Id=ch.CourseId where t.ChapterId='" + chapterModel[i].chapterId + "'";
                            SQLiteDataReader sQLiteDataReader = cmdd.ExecuteReader();
                            int j = 0;
                            List<TopicModel> topicModel = new List<TopicModel>();
                            if (sQLiteDataReader.HasRows)
                            {
                                while (sQLiteDataReader.Read())
                                {
                                    topicModel.Add(new TopicModel());
                                    topicModel[j].topicId = int.Parse(sQLiteDataReader["Id"].ToString());
                                    topicModel[j].topicName = sQLiteDataReader["TopicName"].ToString();
                                    topicModel[j].videoURL = "http://localhost:5500/videos/courses" + sQLiteDataReader["VideoURL"].ToString();
                                    topicModel[j].notesURL = sQLiteDataReader["NotesURL"].ToString();
                                    topicModel[j].chapterId = int.Parse(sQLiteDataReader["ChapterId"].ToString());
                                    j++;
                                }
                            }
                            chapterModel[i].topics = topicModel;
                            sQLiteDataReader.Close();
                            i++;
                        }
                    }
                    dr.Close();
                    con.Close();
                    cmd.Dispose();
                    cmdd.Dispose();
                    loadLocation = "Loaded from API at" + DateTime.Now;
                    Console.WriteLine(loadLocation);
                    isCacheData = "";
                    //Setting data in cache
                    await cache.SetRecordAsync(recordKey, chapterModel);
                    return chapterModel;
                }
                catch (Exception e)
                {
                    return chapterModel;
                }
            }
            else
            {
                loadLocation = "Loaded from cache at" + DateTime.Now;
                Console.WriteLine(loadLocation);
                isCacheData = "data";
                return chapterModel;
            }
        }


        public async Task<List<CourseModel>> GetCourseDetails()
        {
            IDistributedCache cache = CourseController._distributedCache;
            List<CourseModel> courseModel;
            loadLocation = null;
            string recordKey = CourseController.recordKey;
            //Getting data from cache
            courseModel =  await cache.GetRecordAsync<List<CourseModel>>(recordKey);
            if (courseModel is null)
            {
                try
                {
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "select * from Course";
                    con.Open();
                    SQLiteDataReader sQLiteDataReader = cmd.ExecuteReader();
                    int i = 0;
                    courseModel = new List<CourseModel>();
                    if (sQLiteDataReader.HasRows)
                    {
                        while (sQLiteDataReader.Read())
                        {
                            courseModel.Add(new CourseModel());
                            courseModel[i].courseId = int.Parse(sQLiteDataReader["Id"].ToString());
                            courseModel[i].courseName = sQLiteDataReader["CourseName"].ToString();
                            courseModel[i].authorName = sQLiteDataReader["AuthorName"].ToString();
                            courseModel[i].imageURL = "http://localhost:5500/images/courses" + sQLiteDataReader["ImageURL"].ToString();
                            i++;
                        }
                    }
                    sQLiteDataReader.Close();
                    con.Close();
                    cmd.Dispose();
                    loadLocation = "Loaded from API at" + DateTime.Now;
                    Console.WriteLine(loadLocation);
                    isCacheData = "";
                    //Setting data in cache
                    await cache.SetRecordAsync(recordKey, courseModel);
                    return courseModel;
                }
                catch (Exception e)
                {
                    return courseModel;
                }

            }
            else
            {
                loadLocation = "Loaded from cache at" + DateTime.Now;
                Console.WriteLine(loadLocation);
                isCacheData = "data";
                return courseModel;
            }

        }
       


        public bool AddChapter(ChapterModel chapterModel)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "INSERT INTO Chapter(Id,Chaptername,CourseId) VALUES (@chapterId,@chaptername,@courseId)";
                cmd.Parameters.AddWithValue("@chapterId", chapterModel.chapterId);
                cmd.Parameters.AddWithValue("@chaptername", chapterModel.chapterName);
                cmd.Parameters.AddWithValue("@courseId", chapterModel.courseId);
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
                return true;
            }
            catch (Exception e)
            {
                cmd.Dispose();
                con.Close();
                return false;
            }

        }

        public bool EditChapter(ChapterModel chapterModel, [FromQuery] int id)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "UPDATE Chapter SET Id=@chapterId,Chaptername=@chapterName,CourseId=@courseId where Id='" + id + "'";
                cmd.Parameters.AddWithValue("@chapterId", chapterModel.chapterId);
                cmd.Parameters.AddWithValue("@chaptername", chapterModel.chapterName);
                cmd.Parameters.AddWithValue("@courseId", chapterModel.courseId);
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                cmd.Dispose();
                con.Close();
                return false;
            }

        }

        public bool AddTopics(TopicModel topicModel)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "INSERT INTO Topic(TopicName,VideoURL,NotesURL,ChapterId) VALUES (@topicName,@videoURL,@notesURL,@chapterId)";
                cmd.Parameters.AddWithValue("@topicName", topicModel.topicName);
                cmd.Parameters.AddWithValue("@videoURL", topicModel.videoURL);
                cmd.Parameters.AddWithValue("@notesURL", topicModel.notesURL);
                cmd.Parameters.AddWithValue("@chapterId", topicModel.chapterId);
                int rowsAffected = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                cmd.Dispose();
                con.Close();
                return false;
            }
        }

        public bool EditTopics(TopicModel topicModel, [FromQuery] int id)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "UPDATE Topic SET TopicName=@topicName,VideoURL=@videoURL,NotesURL=@notesURL,ChapterId=@chapterId where Id='" + id + "'";
                cmd.Parameters.AddWithValue("@topicName", topicModel.topicName);
                cmd.Parameters.AddWithValue("@videoURL", topicModel.videoURL);
                cmd.Parameters.AddWithValue("@notesURL", topicModel.notesURL);
                cmd.Parameters.AddWithValue("@chapterId", topicModel.chapterId);
                int rowsAffected = cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                cmd.Dispose();
                con.Close();
                return false;
            }
        }
    }
}

