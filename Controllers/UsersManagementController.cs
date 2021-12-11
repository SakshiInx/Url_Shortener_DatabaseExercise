using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Url_Shortener_DatabaseExercise;

namespace Url_Shortener_DatabaseExercise.Controllers
{
    public class UsersManagementController : Controller
    {
        private url_shortenerEntities db = new url_shortenerEntities();

        // GET: UsersManagement
        public ActionResult Index()
        {
            return View(db.users.ToList());
        }

        // GET: UsersManagement/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: UsersManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UsersManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,password,name,email")] user user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //return View(user);
            return Redirect("/Home/Index");


        }

        // GET: UsersManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: UsersManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,password,name,email")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: UsersManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: UsersManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection fc)
        {
            if (fc["Username"] == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(fc["Username"]);

            //If user does not exist, redirect him to the creation
            if (user == null)
            {
                ViewBag.message = "User and password do not match";
                return View();
            }

            //User exists
            else
            {
                //Check password
                if (user.password.Equals(fc["password"]))
                {
                    //Authenticated
                    HttpCookie cookie = new HttpCookie("AuthCookie");
                    cookie.Value = fc["Username"];
                    ViewBag.salut = ("Hello " + fc["Username"] + ", welcome back!!");
                    cookie.Path = Request.ApplicationPath;
                    Response.Cookies.Add(cookie);

                    return Redirect("/Home/Index");
                }
                else
                {
                    ViewBag.message = "User and password do not match";
                    return View();
                }

            }
        }

        public ActionResult Logout()
        {
            HttpCookie cookie = Request.Cookies["AuthCookie"];
            //If it doesnt exist, means the person is not logged in
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Path = Request.ApplicationPath;
                Response.Cookies.Add(cookie);
            }
            //Check here
            return Redirect("/Home/Index");
        }
    }
}
