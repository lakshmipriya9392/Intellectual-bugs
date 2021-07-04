using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingLab.Models;

namespace TrainingLab.Services
{
    public class EventService
    {
        private static Lazy<EventService> Initializer = new Lazy<EventService>(() => new EventService());
        public static EventService Instance => Initializer.Value;
        public static string path = "C:\\Users\\HIMANI\\Desktop\\Perspectify Internship\\Training Lab\\Intellectual-bugs";
        SQLiteConnection con = new SQLiteConnection("Data Source=" + path + "\\TrainingLab\\TrainingLab\\TrainingLabDB.db");
        SQLiteCommand cmd = new SQLiteCommand();
        SQLiteCommand cmdd = new SQLiteCommand();
        SQLiteDataReader dr;
        public async Task<IEnumerable<EventModel>> GetEvents([FromQuery] int id)
        {
            cmd.Connection = con;
            cmdd.Connection = con;
            con.Open();
            if (id > 0)
            {
                cmd.CommandText = "select * from Event where Id='" + id + "' ORDER BY StartTime DESC";
            }
            else
            {
                cmd.CommandText = "select * from Event where Id EXCEPT select * from Event where StartTime>='" + DateTime.UtcNow.AddHours(5.5).ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY StartTime DESC";
            }
            dr = cmd.ExecuteReader();
            List<EventModel> eventModel = new List<EventModel>();
            int i = 0;
            if (dr.HasRows)
            {
                while (dr.Read())
                {

                    eventModel.Add(new EventModel());
                    GetEventAttendee(i, eventModel, dr["EventName"].ToString());
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

        public async Task<IEnumerable<EventModel>> GetFutureEvents()
        {
            cmd.Connection = con;
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
                    GetEventAttendee(i, eventModel, dr["EventName"].ToString());
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

        public List<EventModel> GetEventAttendee(int i, List<EventModel> eventModel, string eventName)
        {
            cmdd.CommandText = "select u.Name,ea.Panelist from User u inner join EventAttendee ea on u.EmailId=ea.EmailId inner join Event e on e.Id=ea.EventId where e.EventName='" + eventName + "'";
            SQLiteDataReader dr2 = cmdd.ExecuteReader();
            StringBuilder panelist = new StringBuilder();
            StringBuilder attendee = new StringBuilder();
            eventModel[i].Panelists = "";
            eventModel[i].Attendee = "";
            if (dr2.HasRows)
            {
                int j = 0;
                while (dr2.Read())
                {
                    if (dr2["Panelist"].ToString() == "True")
                    {
                        if (j != 0 && panelist.ToString() != "")
                        {
                            panelist.Append(",");
                        }
                        panelist.Append(dr2["Name"]);
                    }
                    else
                    {
                        if (j != 0 && attendee.ToString() != "")
                        {
                            attendee.Append(",");
                        }
                        attendee.Append(dr2["Name"]);
                    }
                    j++;
                }
            }
            eventModel[i].Panelists = panelist.ToString();
            eventModel[i].Attendee = attendee.ToString();
            dr2.Close();
            return eventModel;
        }

        public bool AddEvent(EventModel eventModel)
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
                cmd.CommandText = "select Id from Event where EventName='" + eventModel.EventName + "' AND StartTime='" + eventModel.StartTime + "'";
                int eventId = int.Parse(cmd.ExecuteScalar().ToString());
                string[] panelists = eventModel.Panelists.Split(",");
                int i = 0;
                while (i < panelists.Length)
                {
                    cmd.CommandText = "INSERT INTO EventAttendee(Panelist,EmailId,EventId) VALUES('True','" + panelists[i] + "','" + eventId + "')";
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateEvent(EventModel eventModel, [FromQuery] int id)
        {
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.CommandText = "UPDATE Event SET EventName=@eventName,StartTime=@startTime,EndTime=@endTime,Description=@description,EventURL=@eventURL where Id='" + id + "'";
                cmd.Parameters.AddWithValue("@eventName", eventModel.EventName);
                cmd.Parameters.AddWithValue("@startTime", eventModel.StartTime);
                cmd.Parameters.AddWithValue("@endTime", eventModel.EndTime);
                cmd.Parameters.AddWithValue("@description", eventModel.Description);
                cmd.Parameters.AddWithValue("@eventURL", eventModel.EventURL);
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

         public bool DeleteEvent([FromQuery] int id)
        {
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.CommandText = "DELETE * FROM Event where Id='" + id + "'";
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddAttendee(EventModel eventModel)
        {
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "INSERT INTO EventAttendee(EmailId,EventId,Panelist) VALUES('" + eventModel.Attendee + "','" + eventModel.EventId + "','False')";
            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                return true;
            }
            return false;
        }
    }
}
