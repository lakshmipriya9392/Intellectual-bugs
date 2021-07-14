using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingLab.Models;
using TrainingLab.Services;

namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : Controller
    {
        //for redis cache
        public static IDistributedCache _distributedCache;
        public static string recordKey;
        public CourseController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
            //recordKey = "Course_" + DateTime.Now.ToString("yyyyMMdd_hh");
        }
        [HttpGet]
        public async Task<IEnumerable> GetCourses(string id)
        {                        
            return await CourseService.Instance.GetCourses(id);           
        }

        
        [HttpPost]
        [Route("addchapter")]
        public IActionResult AddChapter(ChapterModel chapterModel)
        {
            if(CourseService.Instance.AddChapter(chapterModel))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't insert data" });

        }

        [HttpPost]
        [Route("editchapter")]
        public IActionResult EditChapter(ChapterModel chapterModel,[FromQuery] int id)
        {
            if (CourseService.Instance.EditChapter(chapterModel,id))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't update data" });
        }

        [HttpPost]
        [Route("addtopics")]
        public IActionResult AddTopics(TopicModel topicModel)
        {
            if (CourseService.Instance.AddTopics(topicModel))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't insert data" });
        }

        [HttpPost]
        [Route("edittopics")]
        public IActionResult EditTopics(TopicModel topicModel,[FromQuery] int id)
        {
            if (CourseService.Instance.EditTopics(topicModel,id))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't update data" });
        }
    }
}
