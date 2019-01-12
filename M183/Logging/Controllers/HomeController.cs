using Logging.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Logging.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (CheckLogin(model.Username, model.Password))
                {
                    Session["username"] = model.Username;
                    return RedirectToAction("GetLogs");
                }
            }
            return View(model);
        }
        public ActionResult GetLogs(string username = null)
        {
            LogsViewModel model = new LogsViewModel();
            if(!string.IsNullOrEmpty(username))
            {
                model.Logs = GetAllLogsFor(username);
            }
            else if(Session["username"] != null)
            {
                model.Logs = GetAllLogsFor((string)Session["username"]);
            }
            return View(model);
        }
        private void WriteLog(string username, string browser, string ip, string action, int result, string message = null)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = GetConnectionString();
            con.Open();

            SqlCommand command = new SqlCommand();

            command.Connection = con;
            command.CommandText = "INSERT INTO [dbo].[Log] (Username, Browser, IP, Action, Result, Message) VALUES(@username, @browser, @ip, @action, @result, @message)";
            command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            command.Parameters.Add("@browser", SqlDbType.VarChar).Value = browser;
            command.Parameters.Add("@ip", SqlDbType.VarChar).Value = ip;
            command.Parameters.Add("@action", SqlDbType.VarChar).Value = action;
            command.Parameters.Add("@result", SqlDbType.Int).Value = result;
            command.Parameters.Add("@message", SqlDbType.VarChar).Value = message;

            command.ExecuteNonQuery();
            con.Close();
        }
        private List<(string browser, string ip, string action, int result, string message, DateTime creationDate)> GetAllLogsFor(string username, string action = null)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = GetConnectionString();
            con.Open();

            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            command.Connection = con;
            string cmd = "SELECT Browser, IP, Action, Result, Message, CreationDate FROM [dbo].[Log] WHERE Username = @username";
            if(action != null)
            {
                cmd += " AND Action = @action";
            }
            cmd += " ORDER BY CreationDate desc";
            command.CommandText = cmd;
            command.Parameters.Add("@username", SqlDbType.VarChar).Value = username;
            if(action != null)
            {
                command.Parameters.Add("@action", SqlDbType.VarChar).Value = action;
            }

            reader = command.ExecuteReader();
            List<(string browser, string ip, string action, int result, string message, DateTime creationDate)> list = new List<(string browser, string ip, string action, int result, string message, DateTime creationDate)>();

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    (string browser, string ip, string action, int result, string message, DateTime creationDate) entry =
                        (reader.GetString(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4), reader.GetDateTime(5));
                    list.Add(entry);
                }
            }
            return list;
        }
        private bool CheckLogin(string name, string password)
        {
            string browser = Request.UserAgent;
            string ip = Request.UserHostAddress;
            string action = "Login";
            SqlConnection con = new SqlConnection();
            con.ConnectionString = GetConnectionString();
            con.Open();

            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            command.Connection = con;
            command.CommandText = "SELECT Name FROM [dbo].[User] WHERE Name = @name AND Password = @password";
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
            command.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

            reader = command.ExecuteReader();

            var allLogs = GetAllLogsFor(name, "Login");

            if (reader.HasRows && reader.Read())
            {
                string username = reader.GetString(0);
                con.Close();

                if(!allLogs.Any(x=>x.ip == ip && x.browser == browser))
                {
                    //send email to user and / or require 2fa
                    ;
                }

                WriteLog(username, browser, ip, action, 200, "Login successful");
                
                return true;
            }
            else
            {
                int countWrong = 0;
                for(int i = 0; i < allLogs.Count; i++)
                {
                    if(allLogs[i].message == "Login failed")
                    {
                        countWrong++;
                    }
                    else if(allLogs[i].message == "Login successful")
                    {
                        break;
                    }
                }
                if(countWrong >= 4)
                {
                    //Send SMS
                    ;
                }
                WriteLog(name, browser, ip, action, 200, "Login failed");
            }
            con.Close();
            return false;
        }

        private string GetConnectionString()
        {
            string dbFile = Server.MapPath("~/db.mdf");
            return @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='" + dbFile + "';Integrated Security=True;Connect Timeout=30";
        }
    }
}