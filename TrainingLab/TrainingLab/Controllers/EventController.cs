using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.Threading.Tasks;
using TrainingLab.Models;

namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\Intellectual-bugs";
        SQLiteConnection con = new SQLiteConnection("Data Source="+path+"\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();        
        SQLiteDataReader dr;


        [HttpGet]
        public async Task<IEnumerable<EventModel>>  Get([FromQuery] int id)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            if (id >0)
            {               
                cmd.CommandText = "select * from Event where Id='" + id + "'";               
            }
            else
            {               
                cmd.CommandText = "select * from Event";
            }
            dr = cmd.ExecuteReader();
            List<EventModel> eventModel = new List<EventModel>();
            int i = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    eventModel.Add(new EventModel());
                    await getEventAttendee(i, eventModel, dr["EventName"].ToString());
                    eventModel[i].EventId = dr.GetInt32(0);
                    eventModel[i].EventName = dr.GetString(1);
                    eventModel[i].StartTime = DateTime.Parse(dr.GetString(2));
                    eventModel[i].EndTime = DateTime.Parse(dr.GetString(3));
                    eventModel[i].Description = dr.GetString(4);
                    eventModel[i].EventURL = dr.GetString(5);
                    i++;
                }
            }
            dr.Close();
            con.Close();
            return eventModel;
        }

        [HttpGet("FutureEvents")]
        public async Task<IEnumerable<EventModel>>  GetFutureEvent()
        {

            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            cmd.CommandText = "select * from Event where StartTime>='" + DateTime.UtcNow.AddHours(5.5).ToString("yyyy-MM-dd HH:mm:ss") + "'";
            dr = cmd.ExecuteReader();
            List<EventModel> eventModel = new List<EventModel>();
            int i = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    eventModel.Add(new EventModel());
                    Console.WriteLine(eventModel.Count);
                    await getEventAttendee(i, eventModel, dr["EventName"].ToString());
                    eventModel[i].EventId = dr.GetInt32(0);
                    eventModel[i].EventName = dr.GetString(1);
                    eventModel[i].StartTime = DateTime.Parse(dr.GetString(2));
                    eventModel[i].EndTime = DateTime.Parse(dr.GetString(3));
                    eventModel[i].Description = dr.GetString(4);
                    eventModel[i].EventURL = dr.GetString(5);
                    i++;
                }
            }
            dr.Close();
            con.Close();
            return eventModel;
        }

        public async Task<IActionResult> getEventAttendee(int i, List<EventModel> eventModel, string eventName)
        {
            cmdd.CommandText = "select u.Name,ea.Panelist from User u inner join EventAttendee ea on u.EmailId=ea.EmailId inner join Event e on e.Id=ea.EventId where e.EventName='" + eventName + "'";
            SQLiteDataReader dr2 = cmdd.ExecuteReader();
            eventModel[i].Panelists = "";
            eventModel[i].Attendee = "";
            if (dr2.HasRows)
            {
                int j = 0;
                while (dr2.Read())
                {
                    if (dr2["Panelist"].ToString() == "True")
                    {
                        if (j != 0 && eventModel[i].Panelists != "")
                        {
                            eventModel[i].Panelists += ",";
                        }
                        eventModel[i].Panelists += dr2["Name"];
                    }
                    else
                    {
                        if (j != 0 && eventModel[i].Attendee != "")
                        {
                            eventModel[i].Attendee += ",";
                        }
                        eventModel[i].Attendee += dr2["Name"];
                    }
                    j++;
                }
            }
            dr2.Close();
            return Ok();
        }
        [HttpPost]
        public IActionResult addEvent(EventModel eventModel)
        {
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.CommandText = "INSERT INTO Event(EventName,StartTime,EndTime,Description,EventURL) VALUES (@eventName,@startTime,@endTime,@description,@eventURL)";
                cmd.Parameters.AddWithValue("@eventName", eventModel.EventName);
                cmd.Parameters.AddWithValue("@startTime", eventModel.StartTime);
                cmd.Parameters.AddWithValue("@endTime", eventModel.EndTime);
                cmd.Parameters.AddWithValue("@description", eventModel.Description);
                cmd.Parameters.AddWithValue("@eventURL", eventModel.EventURL);
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                return Ok();
            }
            catch (Exception e)
            {
                return (IActionResult)e;
            }
        }
    }
}
