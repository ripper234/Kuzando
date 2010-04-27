using System;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
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

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if ((HttpContext.Current.User == null) ||
                (!HttpContext.Current.User.Identity.IsAuthenticated)) return;

            // Get Forms Identity From Current User
            var id = (FormsIdentity)HttpContext.Current.User.Identity;

            // Create a custom Principal Instance and assign to Current User (with caching)
            var principal = (GenericPrincipal)HttpContext.Current.Cache.Get(id.Name);
            if (principal == null)
            {
                // Create and populate your Principal object with the needed data and Roles.
                principal = new GenericPrincipal(id, new string[0]);
                HttpContext.Current.Cache.Add(
                    id.Name,
                    principal,
                    null,
                    System.Web.Caching.Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 30, 0),
                    System.Web.Caching.CacheItemPriority.Default,
                    null);
            }

            HttpContext.Current.User = principal;
        }
    }
}