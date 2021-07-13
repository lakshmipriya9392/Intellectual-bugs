using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
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
        SQLiteConnection con = new SQLiteConnection("Data Source=" + Startup.connectionString);

        SQLiteDataReader dr;
        public async Task<IEnumerable<EventModel>> GetEvents(string id)
        {
            List<EventModel> eventModel = new List<EventModel>();
            SQLiteCommand cmd = new SQLiteCommand();

            try
            {
                cmd.Connection = con;
                con.Open();
                if (id == null)
                {
                    cmd.CommandText = "select * from Event EXCEPT select * from Event where StartTime>='" + DateTime.UtcNow.AddHours(5.5).ToString("yyyy-MM-dd HH:mm:ss") + "' ORDER BY StartTime DESC";
                }
                else
                {
                    cmd.CommandText = "select * from Event where Id='" + id + "' ORDER BY StartTime DESC";
                }
                dr = cmd.ExecuteReader();

                int i = 0;

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        eventModel.Add(new EventModel());
                        GetEventAttendee(i, eventModel, dr.GetInt32(0));
                        eventModel[i].EventId = dr.GetInt32(0);
                        eventModel[i].EventName = dr.GetString(1);
                        DateTime date = DateTime.Parse(dr.GetString(2));
                        eventModel[i].StartTime = date.ToString("f", CultureInfo.CreateSpecificCulture("en-US"));
                        date = DateTime.Parse(dr.GetString(3));
                        eventModel[i].EndTime = date.ToString("f", CultureInfo.CreateSpecificCulture("en-US"));
                        eventModel[i].Description = dr.GetString(4);
                        eventModel[i].EventURL = "http://localhost:5500/videos/events" + dr.GetString(5);
                        i++;
                    }
                }
                dr.Close();
                con.Close();
                cmd.Dispose();
                return eventModel;
            }
            catch (Exception e)
            {
                //dr.Close();               
                con.Close();
                cmd.Dispose();
                return eventModel;
            }
        }

        public async Task<IEnumerable<EventModel>> GetFutureEvents(string emailId)
        {
            List<EventModel> eventModel = new List<EventModel>();
            SQLiteCommand cmd = new SQLiteCommand();
            SQLiteCommand cmdd = new SQLiteCommand();
            try
            {
                cmd.Connection = con;
                cmdd.Connection = con;
                con.Open();
                cmd.CommandText = "select * from Event where StartTime>='" + DateTime.UtcNow.AddHours(5.5).ToString("yyyy-MM-dd HH:mm:ss") + "'";
                dr = cmd.ExecuteReader();
                int i = 0;
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        eventModel.Add(new EventModel());
                        GetEventAttendee(i, eventModel, dr.GetInt32(0));
                        eventModel[i].EventId = dr.GetInt32(0);
                        eventModel[i].EventName = dr.GetString(1);
                        DateTime date = DateTime.Parse(dr.GetString(2));
                        eventModel[i].StartTime = date.ToString("f", CultureInfo.CreateSpecificCulture("en-US"));
                        date = DateTime.Parse(dr.GetString(3));
                        eventModel[i].EndTime = date.ToString("f", CultureInfo.CreateSpecificCulture("en-US"));
                        eventModel[i].Description = dr.GetString(4);
                        eventModel[i].EventURL = "http://localhost:5500/videos/events" + dr.GetString(5);
                        eventModel[i].imageURL = "http://localhost:5500/images/events" + dr.GetString(6);
                        cmdd.CommandText = "select count(*) from EventAttendee where EmailId='" + emailId + "' and EventId='" + eventModel[i].EventId + "'";
                        int totalCount = int.Parse(cmdd.ExecuteScalar().ToString());
                        if (totalCount > 0)
                            eventModel[i].attendeeModel.Attendee = true;
                        else
                            eventModel[i].attendeeModel.Attendee = false;
                        i++;
                    }
                }
                dr.Close();
                con.Close();
                cmd.Dispose();
                return eventModel;
            }
            catch (Exception e)
            {
               // dr.Close();
                con.Close();
                cmd.Dispose();
                return eventModel;
            }
        }

        public void GetEventAttendee(int i, List<EventModel> eventModel, int eventId)
        {
            SQLiteCommand cmdd = new SQLiteCommand();
            SQLiteDataReader dr2 = null;
            try
            {
                cmdd.Connection = con;
                cmdd.CommandText = "select u.Name,ea.Panelist from User u inner join EventAttendee ea on u.EmailId=ea.EmailId inner join Event e on e.Id=ea.EventId where e.Id='" + eventId + "'";
                dr2 = cmdd.ExecuteReader();

                StringBuilder attendee = new StringBuilder();
                eventModel[i].attendeeModel = new EventAttendeeModel();
                eventModel[i].attendeeModel.Panelists = new List<string>();
                eventModel[i].Attendee = 0;
                if (dr2.HasRows)
                {
                    int j = 0;
                    while (dr2.Read())
                    {
                        if (dr2["Panelist"].ToString() == "True")
                        {
                            eventModel[i].attendeeModel.Panelists.Add(dr2["Name"].ToString());
                        }
                        else
                        {
                            eventModel[i].Attendee += 1;
                        }
                        j++;
                    }
                }
                cmdd.Dispose();
                dr2.Close();
            }
            catch (Exception e)
            {
                cmdd.Dispose();
               // dr2.Close();
                eventModel[i].attendeeModel.Panelists = null;
                eventModel[i].Attendee = 0;
            }
        }

        public bool AddEvent(EventModel eventModel)
        {
            SQLiteCommand cmd = new SQLiteCommand();
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
                eventModel.attendeeModel = new EventAttendeeModel();
                List<string> panelists = eventModel.attendeeModel.Panelists;
                int i = 0;
                while (i < panelists.Count)
                {
                    cmd.CommandText = "INSERT INTO EventAttendee(Panelist,EmailId,EventId) VALUES('True','" + panelists[i] + "','" + eventId + "')";
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                con.Close();
                cmd.Dispose();
                return true;
            }
            catch (Exception e)
            {
                con.Close();
                cmd.Dispose();
                return false;
            }
        }

        public bool UpdateEvent(EventModel eventModel, [FromQuery] int id)
        {
            SQLiteCommand cmd = new SQLiteCommand();
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
                cmd.Dispose();
                return true;
            }
            catch (Exception e)
            {
                con.Close();
                cmd.Dispose();
                return false;
            }
        }

        public bool DeleteEvent([FromQuery] int id)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            try
            {
                con.Open();
                cmd.CommandText = "DELETE * FROM Event where Id='" + id + "'";
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();
                cmd.Dispose();
                return true;
            }
            catch (Exception e)
            {
                con.Close();
                cmd.Dispose();
                return false;
            }
        }
        public bool AddAttendee([FromBody] EventAttendeeModel eventAttendeeModel)
        {
            SQLiteCommand cmd = new SQLiteCommand();
            cmd.Connection = con;
            con.Open();
            cmd.CommandText = "INSERT INTO EventAttendee(EmailId,EventId,Panelist) VALUES('" + eventAttendeeModel.emailId + "','" + eventAttendeeModel.eventId + "','False')";
            int rowsAffected = cmd.ExecuteNonQuery();

            con.Close();
            cmd.Dispose();
            if (rowsAffected > 0)
            {
                return true;
            }
            //cmd.Dispose();
            return false;
        }
    }
}
