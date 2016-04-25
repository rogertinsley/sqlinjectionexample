using SqlInjectionExample.Models;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Web.Configuration;

namespace SqlInjectionExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View(new Credentials());
        }

        [HttpPost]
        public ActionResult Login(Credentials credentials)
        {
            bool valid = Validate(credentials);

            if (valid) return RedirectToAction("LoggedIn");
            return RedirectToAction("Index");
        }

        public ActionResult LoggedIn()
        {
            return View();
        }

        private bool Validate(Credentials credentials)
        {
            //   ' or '1' = '1
            var connectionString = WebConfigurationManager
                                    .ConnectionStrings["DefaultConnection"]
                                    .ConnectionString;

            string query = "Select * From Users Where Username = '" + credentials.UserName + "' And Password = '" + credentials.Password + "'";
            int result = 0;

            using (var conn = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    result = (int)cmd.ExecuteScalar();
                }
            }

            return result > 0;
        }
    }
}