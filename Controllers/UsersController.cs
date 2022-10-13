using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AuthenticationInMVC.Models;
using CRUD_Using_DataBaseFirst_MVC.Models;

namespace CRUD_Using_DataBaseFirst_MVC.Controllers
{
    //[CustomAuthenticationFilter]
    //[OutputCache(Duration = 0, NoStore = true)]
    public class UsersController : Controller
    {
        private UserDBContext db = new UserDBContext();

        // GET: Users
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.City).Include(u => u.Country).Include(u => u.State);
            return View(users.ToList());
        }
        public ActionResult UserData()
        {
            if (Session["User"] == null)
            {
                return RedirectToAction ("Login", "UserLogins");
            }
            else
            {
                var userlist = db.Users.Include(u => u.City).Include(u => u.Country).Include(u => u.State).ToList();

                return View(userlist);
            }
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                ViewBag.Title = "Create User";
                ViewBag.ID = id;
                ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName");
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName");
                ViewBag.StateID = new SelectList(db.States, "StateID", "StateName");
                return View();
            }
            else
            {
                ViewBag.Title = "Update User";
                ViewBag.ID = id;
                User user = db.Users.Find(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                user.Phone_Number = user.Phone_Number.Trim();
                ViewBag.CityID = new SelectList(db.Cities.Where(city => city.StateID == user.StateID), "CityID", "CityName", user.CityID);
                ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", user.CountryID);
                ViewBag.StateID = new SelectList(db.States.Where(state => state.CountryID == user.CountryID), "StateID", "StateName", user.StateID);
                return View(user);
            }
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,First_Name,Last_Name,Phone_Number,Email,Gender,CityID,StateID,CountryID")] User user)
        {
            if (ModelState.IsValid)
            {
                if (user.ID <= 0)
                {
                    db.Users.Add(user);
                }
                else
                {
                    db.Entry(user).State = EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("UserData");
            }

            ViewBag.CityID = new SelectList(db.Cities, "CityID", "CityName", user.CityID);
            ViewBag.CountryID = new SelectList(db.Countries, "CountryID", "CountryName", user.CountryID);
            ViewBag.StateID = new SelectList(db.States, "StateID", "StateName", user.StateID);
            return View(user);
        }

        public ActionResult Delete(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("UserData");
        }

        public JsonResult GetStatesByCountry(int? CountryID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var states = db.States.Where(state => state.CountryID == CountryID).ToList();
            return Json(states, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCityByState(int? StateID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var cities = db.Cities.Where(city => city.StateID == StateID).ToList();
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
