using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project3_Books_CarlosAlves.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Manages the Index Page
        /// </summary>
        /// <returns>Returns page view</returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Manages the About Page
        /// </summary>
        /// <returns>Returns the About page view</returns>
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        /// <summary>
        /// Manages the contact page    
        /// </summary>
        /// <returns>retun the contact page view</returns>
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}