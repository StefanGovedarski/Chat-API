using Autofac;
using Autofac.Integration.WebApi;
using ChatTU.App_Start;
using ChatTU.Services.Interfaces;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ChatTU
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configuration.Formatters.Add(new JsonMediaTypeFormatter());
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.MediaTypeMappings.Add(
                new QueryStringMapping("type", "json", new MediaTypeHeaderValue("application/json")));
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            var container = AutoFacConfig.Configure();

            using (var scope = container.BeginLifetimeScope())
            {
                var services = container.Resolve<IEnumerable<IService>>();
            }

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }
    }
}
