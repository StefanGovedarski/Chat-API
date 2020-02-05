using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using ChatTU.App_Start;
using ChatTU.MessageHubs;
using ChatTU.Services.Interfaces;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Infrastructure;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;

[assembly: OwinStartup(typeof(ChatTU.Infrastructure.StartUp))]

namespace ChatTU.Infrastructure
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof(IService).Assembly)
                 .AssignableTo<IService>()
                 .AsImplementedInterfaces();

            builder.RegisterType<ChatHub>().ExternallyOwned();

            var container = builder.Build();

            using (var scope = container.BeginLifetimeScope())
            {
                var services = container.Resolve<IEnumerable<IService>>();
            }

            app.Map("/signalr", map =>
            {
                map.UseAutofacMiddleware(container);
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    EnableJSONP = true,
                    Resolver = new AutofacDependencyResolver(container)
            };
                map.RunSignalR(hubConfiguration);
            });

            app.UseCors(CorsOptions.AllowAll);
            ConfigureOAuth(app);

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver((IContainer)container);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
