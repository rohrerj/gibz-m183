using LoginOverSQL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginOverSQL.Controllers
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
                if(CheckLogin(model.Username, model.Password))
                {
                    Session["username"] = model.Username;
                    return RedirectToAction("Feedback");
                }
            }
            return View(model);
        }
        [HttpGet]
        public ActionResult Feedback()
        {
            if (Session["username"] == null) ViewBag.ReadOnly = true;
            List<string> feedbacks = GetAllFeedbacks();
            return View(feedbacks);
        }
        [HttpPost]
        public ActionResult Feedback(string feedback)
        {
            if (Session["username"] == null) return RedirectToAction("Login");

            AddFeedback(feedback);

            List<string> feedbacks = GetAllFeedbacks();
            return View(feedbacks);
        }
        private void AddFeedback(string feedback)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = GetConnectionString();
            con.Open();

            SqlCommand command = new SqlCommand();

            command.Connection = con;
            command.CommandText = "INSERT INTO [dbo].[Feedback] (Feedback) VALUES(@feedback)";
            command.Parameters.Add("@feedback", SqlDbType.VarChar).Value = feedback;

            command.ExecuteNonQuery();
            con.Close();
        }
        private List<string> GetAllFeedbacks()
        {
            List<string> feedbacks = new List<string>();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = GetConnectionString();
            con.Open();

            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            command.Connection = con;
            command.CommandText = "SELECT Feedback FROM [dbo].[Feedback]";

            reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    feedbacks.Add(reader.GetString(0));
                }
            }
            con.Close();
            return feedbacks;
        }

        private bool CheckLogin(string name, string password)
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = GetConnectionString();
            con.Open();

            SqlCommand command = new SqlCommand();
            SqlDataReader reader;

            command.Connection = con;
            command.CommandText = "SELECT Id FROM [dbo].[User] WHERE Name = @name AND Password = @password";
            command.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
            command.Parameters.Add("@password", SqlDbType.VarChar).Value = password;

            reader = command.ExecuteReader();
            
            if (reader.HasRows)
            {
                con.Close();
                return true;
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