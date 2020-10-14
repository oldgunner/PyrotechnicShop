using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PyrotechnicShop.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                null,
                "",
                new
                {
                    controller = "Pyrotechnics",
                    action = "List",
                    category = (string)null,
                    page = 1
                });

            routes.MapRoute(
               name: null,
               url: "Page{page}",
               defaults: new { controller = "Pyrotechnics", action = "List" },
               constraints: new { page = @"\d+" }
               );

            routes.MapRoute(
               null,
               "{category}",
               new { controller = "Pyrotechnics", action = "List", page = 1 }
               );

            routes.MapRoute(
               null,
               "{category}/Page{page}",
               new { controller = "Pyrotechnics", action = "List" },
               new { page = @"\d+" }
               );

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}
