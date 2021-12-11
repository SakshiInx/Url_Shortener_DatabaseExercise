using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Url_Shortener_DatabaseExercise;
using PagedList;

namespace Url_Shortener_DatabaseExercise.Controllers
{
    public class LogsManagementController : Controller
    {
        private url_shortenerEntities db = new url_shortenerEntities();

        // GET: LogsManagement
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.currentSort = sortOrder;
            ViewBag.NameSortParam = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParam = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            var logs = from l in db.logs
                       select l;
            if (!String.IsNullOrEmpty(searchString))
            {
                logs = logs.Where(l => l.short_url.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    logs = logs.OrderByDescending(l => l.short_url);
                    break;
                case "Date":
                    logs = logs.OrderBy(l => l.time_date);
                    break;
                case "date_desc":
                    logs = logs.OrderByDescending(l => l.time_date);
                    break;
                default:
                    logs = logs.OrderBy(l => l.short_url);
                    break;

            }
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            //return View(logs.ToList());
            return View(logs.ToPagedList(pageNumber, pageSize));
            //return View(db.logs.ToList());
        }

        // GET: LogsManagement/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // GET: LogsManagement/Create
        public ActionResult Create()
        {
            HttpCookie cookie = Request.Cookies["AuthCookie"];
            HttpCookie cookie2 = Request.Cookies["shortUrl"];
            HttpCookie cookie3 = Request.Cookies["longUrl"];

            log log = new log();
            log.short_url = cookie2.Value;
            log.time_date = DateTime.Now;
            log.ip_address = Request.ServerVariables["REMOTE_ADDR"];
            if (ModelState.IsValid)
            {
                db.logs.Add(log);
                db.SaveChanges();
                return Redirect(cookie3.Value);
                //return RedirectToAction("Index");
            }

            return View(log);
        }

        // POST: LogsManagement/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "log_id,short_url,time_date,ip_address")] log log)
        {
            
            if (ModelState.IsValid)
            {
                db.logs.Add(log);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(log);
        }

        // GET: LogsManagement/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: LogsManagement/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "log_id,short_url,time_date,ip_address")] log log)
        {
            if (ModelState.IsValid)
            {
                db.Entry(log).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(log);
        }

        // GET: LogsManagement/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            log log = db.logs.Find(id);
            if (log == null)
            {
                return HttpNotFound();
            }
            return View(log);
        }

        // POST: LogsManagement/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            log log = db.logs.Find(id);
            db.logs.Remove(log);
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
    }
}
