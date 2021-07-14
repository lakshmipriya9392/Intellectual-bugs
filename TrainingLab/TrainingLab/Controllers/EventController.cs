using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Threading.Tasks;
using TrainingLab.Models;
using TrainingLab.Services;
using Microsoft.Extensions.Configuration;

namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {


        [HttpGet]
        public async Task<IEnumerable<EventModel>>  Get([FromQuery] string id)
        
        {            
            return await EventService.Instance.GetEvents(id);        
        }

        [HttpGet("FutureEvents")]
        public async Task<IEnumerable<EventModel>>  GetFutureEvent([FromQuery] string emailId)
        {
            return await EventService.Instance.GetFutureEvents(emailId);
        }

       
        [HttpPost("addEvent")]
        public IActionResult AddEvent(EventModel eventModel)
        {
           if(EventService.Instance.AddEvent(eventModel))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't insert data" });
        }

        [HttpPost("updateEvent")]
        public IActionResult UpdateEvent(EventModel eventModel,[FromQuery] int id)
        {
            if (EventService.Instance.UpdateEvent(eventModel,id))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't update data" });
        }

        [HttpPost("deleteEvent")]
        public IActionResult DeleteEvent([FromQuery] int id)
        {
            if (EventService.Instance.DeleteEvent(id))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't delete data" });
        }

        [HttpPost("addattendee")]
        public IActionResult AddAttendee([FromBody] EventAttendeeModel eventAttendeeModel)
        {
            if(eventAttendeeModel.eventId<=0 || eventAttendeeModel.emailId=="")
            {
                return Ok(new { result = "Couldn't insert data" });
            }
            if (EventService.Instance.AddAttendee(eventAttendeeModel))
            {
                return Ok();
            }
            return Ok(new { result = "Couldn't insert data" });
        }
    }
}
