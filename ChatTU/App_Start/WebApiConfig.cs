using System.Web.Http;


namespace ChatTU
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Ignore anything to do with SignalR
            config.Routes.IgnoreRoute("signalr", "signalr/{*pathInfo}");

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
