using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using CRUD_Using_DataBaseFirst_MVC.Models;

namespace CRUD_Using_DataBaseFirst_MVC.Controllers
{
    [AllowAnonymous]
    public class UserLoginsController : Controller
    {
        private UserDBContext db = new UserDBContext();

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var isEmailExis = db.UserLogins.Where(user => user.Email == userLogin.Email).FirstOrDefault();
                if (isEmailExis != null)
                {
                    ViewBag.ErrorMessage = "Email Address Already Exist";
                    return View();
                }
                else
                {
                    db.RegisterUser(userLogin.Email, userLogin.Password);
                    db.SaveChanges();
                    SendEmail(userLogin.Email);
                    return RedirectToAction("Login", "UserLogins");
                }
            }
            else
            {
                return View();
            }
        }

        // GET: UserLogins
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserLogin userLogin)
        {
            if (ModelState.IsValid)
            {
                var user = db.IsUserValid(userLogin.Email, userLogin.Password).FirstOrDefault();
                if (user == 1)
                {
                    Session["User"] = userLogin.Email;
                    FormsAuthentication.SetAuthCookie(userLogin.Email, false);
                    return RedirectToAction("UserData", "Users");
                }
                else
                {
                    var isValidEmail = db.UserLogins.Where(model => model.Email == userLogin.Email).FirstOrDefault();
                    if (isValidEmail == null)
                    {
                        ViewBag.ErrorMessage = "Email Address is not valid";
                        return View();
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Password is not valid";
                        return View();
                    }
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "UserLogins");
        }

        public void SendEmail(string userEmail)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("rvpatel7719@gmail.com");
            mailMessage.To.Add(new MailAddress(userEmail));

            mailMessage.Subject = "Confirm your email";
            mailMessage.Body = "We are excited to tell you that your account is successfully created.";

            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("rvpatel7719@gmail.com", "cpxprjrwtykhqeqs");
            client.UseDefaultCredentials = true;
            client.Credentials = NetworkCred;
            client.Port = 587;

            client.Send(mailMessage);
        }
    }
}
