using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TrainingLab.Controllers;
using TrainingLab.Extensions;
using TrainingLab.Models;

namespace TrainingLab.Services
{
    public class TestService
    {
        private static Lazy<TestService> Initializer = new Lazy<TestService>(() => new TestService());
        public static TestService Instance => Initializer.Value;
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Startup.connectionString);
        private string loadLocation = "";
        private string isCacheData = "";
        static int testId = 0;

        public async Task<IEnumerable> GetCourses(string id,string levelName)
        {
            if (id == null)
            {
                TestController.recordKey = "TestCourse_" + DateTime.Now.ToString("yyyyMMdd_hh");
                return  await GetCourseDetails();
            }
            else
            {
                TestController.recordKey = "Questionnaire_"+id+"_"+levelName+"_" + DateTime.Now.ToString("yyyyMMdd_hh");
                return  await GetQuestionnaires(id, levelName);
            }
        }

        public async Task<IEnumerable> GetLevels()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            TestController.recordKey = "Level_" + DateTime.Now.ToString("yyyyMMdd_hh");
            List<LevelModel> levelModels;
            IDistributedCache cache = TestController._distributedCache;
            loadLocation = null;
            string recordKey = TestController.recordKey;
            //Getting data from cache
            levelModels = null;
            //levelModels= await cache.GetRecordAsync<List<LevelModel>>(recordKey);
            if (levelModels is null)
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "select * from Level";
                    SQLiteDataReader sQLiteDataReader = cmd.ExecuteReader();
                    int i = 0;
                    levelModels = new List<LevelModel>();
                    if (sQLiteDataReader.HasRows)
                    {
                        while (sQLiteDataReader.Read())
                        {
                            levelModels.Add(new LevelModel());
                            levelModels[i].levelId = sQLiteDataReader.GetInt32(0);
                            levelModels[i].levelName = sQLiteDataReader.GetString(1);
                            i++;
                        }
                    }
                    sQLiteDataReader.Close();
                    cmd.Dispose();
                    con.Close();
                    loadLocation = "Loaded from API at" + DateTime.Now;
                    Console.WriteLine(loadLocation);
                    isCacheData = "";
                    //Setting data in cache
                    //await cache.SetRecordAsync(recordKey, levelModels);
                    return levelModels;
                }
                catch (Exception e)
                {
                    return levelModels;
                }
            }
            else
            {
                loadLocation = "Loaded from cache at" + DateTime.Now;
                Console.WriteLine(loadLocation);
                isCacheData = "data";
                return levelModels;
            }
        }
        public async Task<List<CourseModel>> GetCourseDetails()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            List<CourseModel> courseModel;
            IDistributedCache cache = TestController._distributedCache;
            loadLocation = null;
            string recordKey = TestController.recordKey;
            //Getting data from cache
            courseModel = null;
                //courseModel=await cache.GetRecordAsync<List<CourseModel>>(recordKey);
            if (courseModel is null)
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "select * from Course";
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
                            courseModel[i].imageURL = sQLiteDataReader["ImageURL"].ToString();
                            i++;
                        }
                    }
                    sQLiteDataReader.Close();
                    cmd.Dispose();
                    con.Close();
                    loadLocation = "Loaded from API at" + DateTime.Now;
                    Console.WriteLine(loadLocation);
                    isCacheData = "";
                    //Setting data in cache
                    //await cache.SetRecordAsync(recordKey, courseModel);
                    return courseModel;
                }
                catch (Exception e)
                {
                    con.Close();
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

        public async Task<List<QuestionnaireModel>> GetQuestionnaires(string id, string levelName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            List<QuestionnaireModel> questionnaireModel;
            IDistributedCache cache = TestController._distributedCache;
            loadLocation = null;
            string recordKey = TestController.recordKey;
            //Getting data from cache
            questionnaireModel = null;
               // questionnaireModel=await cache.GetRecordAsync<List<QuestionnaireModel>>(recordKey);
            if (questionnaireModel is null)
            {
                try
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.CommandText = "select q.Id,t.Id,q.NoOfOptions,l.LevelName,q.QuestionText,q.TypeOfQuestion from Test t inner join Course c on c.Id=t.CourseId inner join Questionnaire q on t.Id=q.TestId inner join Level l on l.Id=t.LevelId where c.Id='" + id + "' and l.LevelName='" + levelName + "'";
                    SQLiteDataReader dr = cmd.ExecuteReader();
                    int i = 0;
                    questionnaireModel = new List<QuestionnaireModel>();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            questionnaireModel.Add(new QuestionnaireModel());
                            questionnaireModel[i].questionId = dr.GetInt32(0);
                            questionnaireModel[i].testId = dr.GetInt32(1);
                            questionnaireModel[i].question = dr.GetString(4);
                            questionnaireModel[i].typeOfQuestion = dr.GetString(5);
                            questionnaireModel[i].noOfOptions = dr.GetInt32(2);
                            SQLiteCommand cmdd = new SQLiteCommand();
                            cmdd.Connection = con;

                            cmdd.CommandText = "select * from Options where QuestionId='" + questionnaireModel[i].questionId + "'";
                            SQLiteDataReader sQLiteDataReader = cmdd.ExecuteReader();
                            OptionModel[] optionModel = new OptionModel[questionnaireModel[i].noOfOptions];
                            
                            if (sQLiteDataReader.HasRows)
                            {
                                while (sQLiteDataReader.Read())
                                {
                                    int j = 0;
                                    while (j < questionnaireModel[i].noOfOptions)
                                    {
                                        optionModel[j] = new OptionModel();
                                        optionModel[j].option = sQLiteDataReader.GetString(j+1);
                                        optionModel[j].optionId = sQLiteDataReader.GetInt32(0);
                                        j++;
                                    }
                                  
                                    //optionModel[i].questionId = sQLiteDataReader.GetInt32(5);
                                }
                            }
                            sQLiteDataReader.Close();
                            cmdd.Dispose();
                            questionnaireModel[i].optionList = optionModel;
                            i++;
                        }
                    }
                    dr.Close();
                    cmd.Dispose();
                    con.Close();
                    loadLocation = "Loaded from API at" + DateTime.Now;
                    Console.WriteLine(loadLocation);
                    isCacheData = "";
                    //Setting data in cache
                   // await cache.SetRecordAsync(recordKey, questionnaireModel);
                    return questionnaireModel;
                }
                catch (Exception e)
                {
                    con.Close();
                    return questionnaireModel;
                }
            }
            else
            {
                loadLocation = "Loaded from cache at" + DateTime.Now;
                Console.WriteLine(loadLocation);
                isCacheData = "data";
                return questionnaireModel;
            }
        }

        public static int score = 0;
        public static int totalCorrectAnswer = 0;
        public static int totalWrongAnswer = 0;
        public async Task<string> CheckAnswer(int id, string answer, string emailId)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "select CorrectAnswer from Questionnaire where Id='" + id + "'";
                string correctAnswer = cmd.ExecuteScalar().ToString();
                con.Close();
                if (correctAnswer.Equals(answer))
                {
                    score++;
                    totalCorrectAnswer++;
                    return "True";
                }
                else
                {
                    totalWrongAnswer++;
                    return correctAnswer;
                }
            }
            catch(Exception e)
            {
                cmd.Dispose();
                con.Close();
                return "False";
            }

        }

        public async Task<bool> PostScore(int id,string emailId)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "INSERT INTO UserTestLevel(EmailId,TestId,Status) VALUES('" + emailId + "','" + id + "','UPGRADING')";
                int rowsAffetcted = cmd.ExecuteNonQuery();
                bool result=await UpgradeLevel(id, score, emailId);
                con.Close();                
                return true;
            }
            catch(Exception e)
            {
                cmd.Dispose();
                con.Close();
                return false;
            }
        }
        public async Task<bool> UpgradeLevel(int id, int score, string emailId)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                cmd.CommandText = "SELECT MinimumScore from Test where Id='" + id + "'";
                float minimumScore = float.Parse(cmd.ExecuteScalar().ToString());
                cmd.CommandText = "INSERT INTO UserScore(Score,EmailId,TestId) VALUES('" + score + "','" + emailId + "','" + id + "')";
                int rowsAffetcted = cmd.ExecuteNonQuery();
                if (score >= minimumScore)
                {
                    cmd.CommandText = "UPDATE UserTestLevel SET Status='PASSED' where EmailId='" + emailId + "' and TestId='" + id + "'";
                    rowsAffetcted = cmd.ExecuteNonQuery();
                }
                if(rowsAffetcted>0)
                    return true;
                else
                    return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> PostQuestion(QuestionnaireModel[] questionnaireModels)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                con.Open();
                int i = 0, rowsAffected = 0;
                while (i < questionnaireModels.Length)
                {
                    OptionModel[] optionModel =questionnaireModels[i].optionList;
                    cmd.CommandText = "INSERT INTO Questionnaire(QuestionText,TypeOfQuestion,CorrectAnswer,NoOfOptions,TestId) VALUES('" + questionnaireModels[i].question + "','" + questionnaireModels[i].typeOfQuestion + "','" + questionnaireModels[i].answer + "','" +questionnaireModels[i].noOfOptions+"','"+ questionnaireModels[i].testId + "')";
                    rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected < 0)
                    {
                        break;
                    }
                    cmd.CommandText = "select Id from Questionnaire where QuestionText='" + questionnaireModels[i].question+"' and TestId='"+questionnaireModels[i].testId+"'";
                    int questionId=int.Parse(cmd.ExecuteScalar().ToString());
                    if (rowsAffected < 0)
                    {
                        break;
                    }
                    await PostOptions(optionModel,questionId);
                    i++;
                }
                con.Close();
                cmd.Dispose();
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                con.Close();
                cmd.Dispose();
                return false;
            }
        }

        public async Task<bool> PostOptions(OptionModel[] optionModel,int questionId)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
           
                int  rowsAffected = 0;
                if (optionModel.Length > 2)
                {
                    cmd.CommandText = "INSERT INTO Options(OptionA,OptionB,OptionC,OptionD,QuestionId) VALUES('" + optionModel[0].option + "','" + optionModel[1].option + "','" + optionModel[2].option + "','" + optionModel[3].option + "','" + questionId + "')";
                }
                else
                {
                    cmd.CommandText = "INSERT INTO Options(OptionA,OptionB,QuestionId) VALUES('" + optionModel[0].option + "','" + optionModel[1].option + "','" + questionId + "')";
                }
                    rowsAffected = cmd.ExecuteNonQuery();
              
                cmd.Dispose();
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                con.Close();
                cmd.Dispose();
                return false;
            }
        }
    }
}
