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
    public class UrlsManagementController : Controller
    {
        private url_shortenerEntities db = new url_shortenerEntities();


        // GET: UrlsManagement
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["AuthCookie"];
            var query = from u in db.urls
                        where u.username == cookie.Value
                        select u;
            
            return View(query);
        }

        // GET: UrlsManagement/Details/5
        public ActionResult Details(string id)
        {
            var logs = from v in db.logs
                       where v.short_url == id
                       select v;
            ViewBag.logs = logs;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            url url = db.urls.Find(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        // GET: UrlsManagement/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UrlsManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "short_url,long_url,username")] url url)
        {
            HttpCookie cookie = Request.Cookies["AuthCookie"];
            url.username = cookie.Value;
            if (ModelState.IsValid)
            {
                db.urls.Add(url);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(url);
        }

        // GET: UrlsManagement/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            url url = db.urls.Find(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        // POST: UrlsManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "short_url,long_url,username")] url url)
        {
            HttpCookie cookie = Request.Cookies["AuthCookie"];
            url.username = cookie.Value;
            if (ModelState.IsValid)
            {
                db.Entry(url).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(url);
        }

        // GET: UrlsManagement/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            url url = db.urls.Find(id);
            if (url == null)
            {
                return HttpNotFound();
            }
            return View(url);
        }

        // POST: UrlsManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            url url = db.urls.Find(id);
            db.urls.Remove(url);
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
        public ActionResult Go(string shortUrlParameter)
        {
            if (shortUrlParameter == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            url url = db.urls.Find(shortUrlParameter);

            //If url does not exist, redirect him to the creation
            if (url == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //User exists
            else
            {
                //All good
                //HttpCookie cookie2 = new HttpCookie("shortUrl");
                //cookie2.Value = url.short_url;
                //HttpCookie cookie3 = new HttpCookie("longUrl");
                //cookie3.Value = url.long_url;
                //cookie.Path = Request.ApplicationPath;
                //Response.Cookies.Add(cookie2);
                //Response.Cookies.Add(cookie3);
                log log = new log();
                log.short_url = url.short_url;
                log.time_date = DateTime.Now;
                log.ip_address = Request.ServerVariables["REMOTE_ADDR"];
                if (ModelState.IsValid)
                {
                    db.logs.Add(log);
                    db.SaveChanges();
                }
                return Redirect(url.long_url);


            }
        }
    }
}
