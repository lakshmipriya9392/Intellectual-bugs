using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using TrainingLab.Models;

namespace TrainingLab.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventController : Controller
    {
        public static string path = "C:\\Users\\HIMANI\\OneDrive\\BackEnd";
        SQLiteConnection con = new SQLiteConnection("Data Source="+path+"\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();        
        SQLiteDataReader dr;


        [HttpGet]
        public EventModel[] Get([FromQuery] int id)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            int size = 0;
            if (id >0)
            {
                cmd.CommandText = "select count(*) from Event where Id='" + id + "'";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        size = dr.GetInt32(0);
                    }
                }

                dr.Close();
                cmd.CommandText = "select * from Event where Id='" + id + "'";
               
            }
            else
            {
                cmd.CommandText = "select count(*) from Event";
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        size = dr.GetInt32(0);
                    }
                }

                dr.Close();
                cmd.CommandText = "select * from Event";
            }
            dr = cmd.ExecuteReader();
            EventModel[] eventModel = new EventModel[size];
            int i = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    eventModel[i] = new EventModel();
                    getEventAttendee(i, eventModel, dr["EventName"].ToString());
                    eventModel[i].EventId = dr.GetInt32(0);
                    eventModel[i].EventName = dr.GetString(1);
                    eventModel[i].StartTime = dr.GetString(2);
                    eventModel[i].EndTime = dr.GetString(3);
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
        public EventModel[] GetFutureEvent()
        {

            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            int size = 0;
            cmd.CommandText = "select count(*) from Event where StartTime>='" + System.DateTime.UtcNow.AddHours(5.50) + "'";
            SQLiteDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    size = dr.GetInt32(0);
                }
            }

            dr.Close();
            cmd.CommandText = "select * from Event where StartTime>='" + System.DateTime.UtcNow.AddHours(5.50) + "'";
            dr = cmd.ExecuteReader();
            EventModel[] eventModel = new EventModel[size];
            int i = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    eventModel[i] = new EventModel();
                    getEventAttendee(i, eventModel, dr["EventName"].ToString());
                    eventModel[i].EventId = dr.GetInt32(0);
                    eventModel[i].EventName = dr.GetString(1);
                    eventModel[i].StartTime = dr.GetString(2);
                    eventModel[i].EndTime = dr.GetString(3);
                    eventModel[i].Description = dr.GetString(4);
                    eventModel[i].EventURL = dr.GetString(5);
                    i++;
                }
            }
            dr.Close();
            con.Close();
            return eventModel;
        }

        public async void getEventAttendee(int i, EventModel[] eventModel, string eventName)
        {
            cmdd.CommandText = "select u.FirstName, u.LastName,ea.Panelist from User u inner join EventAttendee ea on u.EmailId=ea.EmailId inner join Event e on e.Id=ea.EventId where e.EventName='" + eventName + "'";
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
                        eventModel[i].Panelists += dr2["FirstName"] + " " + dr2["LastName"];
                    }
                    else
                    {
                        if (j != 0 && eventModel[i].Attendee != "")
                        {
                            eventModel[i].Attendee += ",";
                        }
                        eventModel[i].Attendee += dr2["FirstName"] + " " + dr2["LastName"];
                    }
                    j++;
                }
            }
            dr2.Close();
        }
    }
}
