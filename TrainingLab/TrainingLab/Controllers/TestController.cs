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
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\Intellectual-bugs";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();
        static int testId = 0;
        [HttpGet]

        public async Task<IActionResult> Get([FromQuery] string id, [FromQuery] string levelName)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
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
                cmd.CommandText = "select q.Id,t.Id,l.LevelName,q.QuestionText,q.OptionList,q.CorrectAnswer from Test t inner join Course c on c.Id=t.CourseId inner join Questionnaire q on t.Id=q.TestId inner join Level l on l.Id=t.LevelId where c.Id='" + id + "' and l.LevelName='" + levelName + "'";
                SQLiteDataReader dr = cmd.ExecuteReader();
                int i = 0;
                List<QuestionnaireModel> questionnaireModel = new List<QuestionnaireModel>();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        questionnaireModel.Add(new QuestionnaireModel());
                        questionnaireModel[i].questionId = dr.GetInt32(0);
                        questionnaireModel[i].testId = dr.GetInt32(1);
                        questionnaireModel[i].question = dr.GetString(3);
                       
                        cmdd.CommandText = "select * from Options where QuestionId='" + questionnaireModel[i].questionId + "'";
                        SQLiteDataReader sQLiteDataReader = cmdd.ExecuteReader();
                        OptionModel optionModel = new OptionModel();
                        if(sQLiteDataReader.HasRows)
                        {
                            while(sQLiteDataReader.Read())
                            {
                                optionModel.optionA = sQLiteDataReader.GetString(1);
                                optionModel.optionB = sQLiteDataReader.GetString(2);
                                optionModel.optionC = sQLiteDataReader.GetString(3);
                                optionModel.optionD = sQLiteDataReader.GetString(4);
                                optionModel.questionId = sQLiteDataReader.GetInt32(5);
                            }
                        }
                        sQLiteDataReader.Close();
                        questionnaireModel[i].optionList = optionModel;
                        i++;                        
                    }
                }
                dr.Close();
                con.Close();
                return CreatedAtAction(nameof(Get), questionnaireModel);
            }
        }
        public static int score = 0;
        [HttpPost]
        public async Task<IActionResult> PostAnswer(int id, string answer, string emailId)
        {
               return CreatedAtAction(nameof(PostAnswer),await CheckAnswer(id, answer, emailId));
        }
        public async Task<string> CheckAnswer(int id, string answer, string emailId)
        {

            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "select CorrectAnswer from Questionnaire where Id='" + id + "'";
            string correctAnswer = cmd.ExecuteScalar().ToString();
            if (correctAnswer.Equals(answer))
            {
                score++;
                return "{\"message\":\"CORRECT ANSWER\"}";
            }
            else
            {
                return "{\"message\":\"WRONG ANSWER\"}";
            }

        }
        [HttpPost("score")]
        public async Task<IActionResult> PostScore(int id, int score, string emailId)
        {
            cmd.Connection = con;
            con.Open();           
            cmd.CommandText = "INSERT INTO UserTestLevel(EmailId,TestId,Status) VALUES('" + emailId + "','" + id + "','UPGRADING')";
            int rowsAffetcted = cmd.ExecuteNonQuery();
            await UpgradeLevel(id,score,emailId);
            if (rowsAffetcted > 0)
            {
                return Ok();
            }
            return CreatedAtAction(nameof(PostScore), "Not Inserted");
        }
        public async Task<IActionResult> UpgradeLevel(int id,int score,string emailId)
        {
            cmd.Connection = con;
            cmd.CommandText = "SELECT MinimumScore from Test where Id='" + id + "'";
            float minimumScore = float.Parse(cmd.ExecuteScalar().ToString());
            cmd.CommandText = "INSERT INTO UserScore(Score,EmailId,TestId) VALUES('" + score + "','" + emailId + "','" + id + "')";
            int rowsAffetcted = cmd.ExecuteNonQuery();
            if(score>=minimumScore)
            {
                cmd.CommandText = "UPDATE UserTestLevel SET Status='PASSED' where EmailId='"+emailId+"' and TestId='"+id+"'";
                rowsAffetcted = cmd.ExecuteNonQuery();
            }
            return Ok();
        }

        [HttpPost("postQuestion")]
        public async Task<IActionResult> PostQuestion(QuestionnaireModel[] questionnaireModels)
        {
            cmd.Connection = con;
            con.Open();
            int i = 0,rowsAffected=0;
            while (i < questionnaireModels.Length)
            {
                cmd.CommandText = "INSERT INTO Questionnaire(QuestionText,TypeOfQuestion,CorrectAnswer,TestId) VALUES('"+questionnaireModels[i].question+"','"+questionnaireModels[i].typeOfQuestion+"','"+questionnaireModels[i].answer+"','"+questionnaireModels[i].testId+"')";
                rowsAffected = cmd.ExecuteNonQuery();
                if(rowsAffected<0)
                {
                    break;
                }
                i++;
            }
            if(rowsAffected>0)
            {
                return Ok();
            }
            return CreatedAtAction(nameof(PostQuestion), "Not inserted");
        }
        [HttpPost("postOption")]
        public async Task<IActionResult> PostOptions(OptionModel[] optionModels)
        {
            cmd.Connection = con;
            con.Open();
            int i = 0, rowsAffected = 0;
            while (i < optionModels.Length)
            {
                cmd.CommandText = "INSERT INTO Options(OptionA,OptionB,OptionC,OptionD,QuestionId) VALUES('" + optionModels[i].optionA + "','" + optionModels[i].optionB + "','" + optionModels[i].optionC + "','" + optionModels[i].optionD + "','"+optionModels[i].questionId+"')";
                rowsAffected = cmd.ExecuteNonQuery();
                if (rowsAffected < 0)
                {
                    break;
                }
                i++;
            }
            if (rowsAffected > 0)
            {
                return Ok();
            }
            return CreatedAtAction(nameof(PostQuestion), "Not inserted");
        }
    }
}
