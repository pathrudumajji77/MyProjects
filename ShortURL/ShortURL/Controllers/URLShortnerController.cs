using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using ShortURL.Helpers;
using ShortURL.Models;

namespace ShortURL.Controllers
{
    public class URLShortnerController : Controller
    {
        
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(URLInfo urlInfo)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    if (string.IsNullOrEmpty(urlInfo.LongURL))
                    {
                        return View();
                    }
                    else
                    {
                        ProcessURLController processURL = new ProcessURLController();
                        response = processURL.ShortenURL(urlInfo);


                        return View(urlInfo);
                    }
                }

                return View(urlInfo);
                

            }
            catch (Exception ex)
            {
                ServiceLocator.ErrorLogger("NEW ERROR LINE : URLShortner/Index | " + DateTime.Now + " | Error:  " + ex.ToString());

                return View();                               
            }
        }

        [HttpGet]
        public ActionResult RedirectToURL(string shortCode)
        {
            try
            {
                URLInfo info = new URLInfo();
                ProcessURLController processURL = new ProcessURLController();
                info = processURL.GetURLInfo(shortCode);

                if (info != null && !string.IsNullOrEmpty(info.LongURL))
                {
                    return Redirect(info.LongURL);
                }
                else
                {
                    return Redirect(ConfigurationManager.AppSettings["redirecturl"].ToString());
                }
            }
            catch(Exception ex)
            {
                ServiceLocator.ErrorLogger("NEW ERROR LINE : URLShortner/RedirectToURL | " + DateTime.Now + " | Error:  " + ex.ToString());

                return Redirect(ConfigurationManager.AppSettings["redirecturl"].ToString());
            }
            

        }
                        
    }
}
