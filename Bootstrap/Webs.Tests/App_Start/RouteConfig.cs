using System.Web.Mvc;
using System.Web.Routing;

namespace Webs.Tests
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                // ReSharper disable once ArgumentsStyleStringLiteral
                name: "Default",
                // ReSharper disable once ArgumentsStyleStringLiteral
                url: "{controller}/{action}/{id}",
                // ReSharper disable once ArgumentsStyleOther
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
