using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShortURL
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "URLShortner", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "URLShortner",
            //    url: "{controller}/{action}",
            //    defaults: new { controller = "URLShortner", action = "Index", id = UrlParameter.Optional }
            //);


            routes.MapRoute(
                name: "ShortUrl",
                url: "{shortCode}",
                defaults: new { controller = "URLShortner", action = "RedirectToURL", shortCode = UrlParameter.Optional }
            );
        }
    }
}
