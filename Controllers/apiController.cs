using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Url_Shortener_DatabaseExercise.Models;

namespace Url_Shortener_DatabaseExercise.Controllers
{
    public class apiController : Controller
    {
        private url_shortenerEntities db = new url_shortenerEntities();

        // GET: api
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(string username, string password, string short_url, string long_url)
        {
            //If the method it's post, the way to test it is using Postman
            //If its GET 
            //https://localhost:44389/api/Create?username=matheus&password=12345&short_url=bbc
            Response response = new Response();
            //username = Request.QueryString["username"];
            //password = Request.QueryString["password"];
            //short_url = Request.QueryString["short_url"];
            //long_url = Request.QueryString["long_url"];
            //string long_url = "https://www.bbc.com/";
            //Check if authentication succedded
            if (!isValidAuthentication(username, password))
            {
                response.status_code = "2";
                response.status_description = "Authentication failed";

            }
            //Check if Short url is available
            else if (!isValidShortener(short_url, long_url, username))
            {
                response.status_code = "3";
                response.status_description = "Short URL is not available";
            }
            else
            {
                response.status_code = "1";
                response.status_description = "Successfull";
                response.short_url = "https://localhost:44389/u/" + short_url;
            }
            return Json(new { status_code = response.status_code, 
                status_description = response.status_description, 
                short_url = response.short_url }, JsonRequestBehavior.AllowGet);
            //return Json(response);



        }
        public bool isValidAuthentication(string username, string password)
        {
           
            if (username == null)
            {
                return false;
            }
            user user = db.users.Find(username);

            //If user does not exist, return false
            if (user == null)
            {
                return false;
            }

            //User exists
            else
            {
                //Check password
                if (user.password.Equals(password))
                {
                    //Authenticated
                    HttpCookie cookie = new HttpCookie("AuthCookie");
                    cookie.Value = username;
                    cookie.Path = Request.ApplicationPath;
                    Response.Cookies.Add(cookie);
                    return true;
                  
                }
                else
                {
                    return false;
                }

            }
        }
        public bool isValidShortener(string short_url, string long_url, string username)
        {

            if (short_url == null)
            {
                return false;
            }
            url url = db.urls.Find(short_url);

            //If url does not exist, return false
            if (url == null)
            {
                url = new url();
                //HttpCookie cookie = new HttpCookie("AuthCookie");
                //url.username = cookie.Value;
                url.username = username;
                url.short_url = short_url;
                url.long_url = long_url;
                if (ModelState.IsValid)
                {
                    db.urls.Add(url);
                    db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            //Url exists
            else
            {
                return false;

            }
        }
    }
}