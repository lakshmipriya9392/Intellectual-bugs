using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TrainingLab.Models;

namespace TrainingLab.Services
{
    public class TestService
    {
        private static Lazy<TestService> Initializer = new Lazy<TestService>(() => new TestService());
        public static TestService Instance => Initializer.Value;
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Startup.connectionString);


        static int testId = 0;

        public async Task<IEnumerable> GetCourses(string id,string levelName)
        {
            if (id == null)
            {
                return  await GetCourseDetails();
            }
            else
            {
                return  await GetQuestionnaires(id, levelName);
            }
        }

        public async Task<List<CourseModel>> GetCourseDetails()
        {
            SQLiteCommand cmd = new SQLiteCommand();
            List<CourseModel> courseModel = new List<CourseModel>();
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "select * from Course";
                SQLiteDataReader sQLiteDataReader = cmd.ExecuteReader();
                int i = 0;

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
                return courseModel;
            }
            catch(Exception e)
            {
                con.Close();
                return courseModel;
            }
        }

        public async Task<List<QuestionnaireModel>> GetQuestionnaires(string id, string levelName)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            List<QuestionnaireModel> questionnaireModel = new List<QuestionnaireModel>();
            try
            {
                cmd.Connection = con;
                con.Open();
                cmd.CommandText = "select q.Id,t.Id,l.LevelName,q.QuestionText,q.OptionList,q.TypeOfQuestion from Test t inner join Course c on c.Id=t.CourseId inner join Questionnaire q on t.Id=q.TestId inner join Level l on l.Id=t.LevelId where c.Id='" + id + "' and l.LevelName='" + levelName + "'";
                SQLiteDataReader dr = cmd.ExecuteReader();
                int i = 0;
                
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        questionnaireModel.Add(new QuestionnaireModel());
                        questionnaireModel[i].questionId = dr.GetInt32(0);
                        questionnaireModel[i].testId = dr.GetInt32(1);
                        questionnaireModel[i].question = dr.GetString(3);
                        questionnaireModel[i].typeOfQuestion = dr.GetString(5);

                        SQLiteCommand cmdd = new SQLiteCommand();
                        cmdd.Connection = con;

                        cmdd.CommandText = "select * from Options where QuestionId='" + questionnaireModel[i].questionId + "'";
                        SQLiteDataReader sQLiteDataReader = cmdd.ExecuteReader();
                        OptionModel optionModel = new OptionModel();
                        if (sQLiteDataReader.HasRows)
                        {
                            while (sQLiteDataReader.Read())
                            {
                                optionModel.optionA = sQLiteDataReader.GetString(1);
                                optionModel.optionB = sQLiteDataReader.GetString(2);
                                optionModel.optionC = sQLiteDataReader.GetString(3);
                                optionModel.optionD = sQLiteDataReader.GetString(4);
                                optionModel.questionId = sQLiteDataReader.GetInt32(5);
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
                return questionnaireModel;
            }
            catch(Exception e)
            {
                con.Close();
                return questionnaireModel;
            }
        }

        public static int score = 0;

        public async Task<bool> CheckAnswer(int id, string answer, string emailId)
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }

        }

        public async Task<bool> PostScore(int id, int score, string emailId)
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
                return result;
            }
            catch(Exception e)
            {
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
                    cmd.CommandText = "INSERT INTO Questionnaire(QuestionText,TypeOfQuestion,CorrectAnswer,TestId) VALUES('" + questionnaireModels[i].question + "','" + questionnaireModels[i].typeOfQuestion + "','" + questionnaireModels[i].answer + "','" + questionnaireModels[i].testId + "')";
                    rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected < 0)
                    {
                        break;
                    }
                    i++;
                }
                con.Close();
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public async Task<bool> PostOptions(OptionModel[] optionModels)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                con.Open();
                int i = 0, rowsAffected = 0;
                while (i < optionModels.Length)
                {
                    cmd.CommandText = "INSERT INTO Options(OptionA,OptionB,OptionC,OptionD,QuestionId) VALUES('" + optionModels[i].optionA + "','" + optionModels[i].optionB + "','" + optionModels[i].optionC + "','" + optionModels[i].optionD + "','" + optionModels[i].questionId + "')";
                    rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected < 0)
                    {
                        break;
                    }
                    i++;
                }
                con.Close();
                if (rowsAffected > 0)
                {
                    return true;
                }
                return false;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
