using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TrainingLab.Models;
using TrainingLab.Services;

namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : Controller
    {

        [HttpGet]
        public async Task<IEnumerable> Get([FromQuery] string id, [FromQuery] string levelName)
        {
            return await TestService.Instance.GetCourses(id, levelName);
        }

        [HttpPost]
        public async Task<IActionResult> PostAnswer(int id, string answer, string emailId)
        {
            string checkAnswer = await TestService.Instance.CheckAnswer(id, answer, emailId);
            if (checkAnswer == "True")
            {
                return Ok(new { message = "CORRECT ANSWER!" });
            }
            else
            {
                return Ok(new { message = "WRONG ANSWER!",correctAnswer=checkAnswer });
            }
        }

        [HttpPost("score")]
        public async Task<IActionResult> PostScore(int id, string emailId)
        {
            if (await TestService.Instance.PostScore(id, emailId))
            {
                return Ok(new { totalQuestion=TestService.totalCorrectAnswer+TestService.totalWrongAnswer,totalCorrectAnswer=TestService.totalCorrectAnswer,totalWrongAnswer=TestService.totalWrongAnswer,score=TestService.score });
            }
            return Ok(new { result = "something gone wrong!" });
        }


        [HttpPost("postQuestion")]
        public async Task<IActionResult> PostQuestion(QuestionnaireModel[] questionnaireModels)
        {
            if (await TestService.Instance.PostQuestion(questionnaireModels))
            {
                return Ok(new { result = "success" });
            }
            return Ok(new { result = "something gone wrong!" });
        }

       /* [HttpPost("postOption")]
        public async Task<IActionResult> PostOptions(OptionModel[] optionModels)
        {
            if (await TestService.Instance.PostOptions(optionModels))
            {
                return Ok(new { result = "success" });
            }
            return Ok(new { result = "something gone wrong!" });
        }*/
    }
}
