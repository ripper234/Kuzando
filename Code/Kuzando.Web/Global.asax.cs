using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kuzando.Common.Web;
using Kuzando.Core.Bootsrap;
using Kuzando.Web.Controllers;

namespace Kuzando.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            var container = Bootstrapper.Instance.CreateContainer(typeof(HomeController).Assembly);
            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));

            RegisterRoutes(RouteTable.Routes);
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.IgnoreRoute("{js}", new {"Scripts/Lib/.*");}
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(.*/)?favicon.ico(/.*)?" });
            routes.IgnoreRoute("{*css}", new { favicon = @"(.*/)?Site.css(/.*)?" });

            routes.MapRoute(
                "Default",                                              // Route name
                "{controller}/{action}/{id}",                           // URL with parameters
                new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
            );
        }
    }
}