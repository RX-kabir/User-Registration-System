using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;
using RegistrationApp.Models;

namespace RegistrationApp.Controllers
{
    public class AuthController : Controller
    {
        string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO UserDB (FirstName, LastName, UserName, Password, ConfirmPassword, Gender, Role) " +
                                   "VALUES (@FirstName, @LastName, @UserName, @Password, @ConfirmPassword, @Gender, @Role)";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@FirstName", model.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", model.LastName);
                    cmd.Parameters.AddWithValue("@UserName", model.UserName);
                    cmd.Parameters.AddWithValue("@Password", model.Password);
                    cmd.Parameters.AddWithValue("@ConfirmPassword", model.ConfirmPassword);
                    cmd.Parameters.AddWithValue("@Gender", model.Gender);
                    cmd.Parameters.AddWithValue("@Role", model.Role);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return RedirectToAction("Users");
            }

            return View(model);
        }

        public ActionResult Users()
        {
            List<RegisterViewModel> users = new List<RegisterViewModel>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM UserDB";
                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new RegisterViewModel
                    {
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        UserName = reader["UserName"].ToString(),
                        Gender = reader["Gender"].ToString(),
                        Role = reader["Role"].ToString()
                    });
                }
            }

            return View(users);
        }
    }
}
