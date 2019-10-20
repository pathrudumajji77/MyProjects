using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShortURL.Models;

namespace ShortURL.Controllers
{
    public class TrackclicksController : Controller
    {
        // GET: Trackclicks
        public ActionResult Index(string u)
        {
            if(!string.IsNullOrEmpty(u))
            {
                URLInfo info = new URLInfo();
                Uri uri = new Uri(u);
                ProcessURLController processURL = new ProcessURLController();
                info = processURL.GetURLInfo(uri.AbsolutePath.TrimStart('/'));

                return View(info);
            }
            else
            {
                return View();
            }
           
        }

        
    }
}