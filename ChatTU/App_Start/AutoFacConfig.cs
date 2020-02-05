using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using ChatTU.Services.Interfaces;

namespace ChatTU.App_Start
{
    public static class AutoFacConfig
    {
        public static IContainer Configure()
        {
            var builder = new ContainerBuilder();

            // Register the Web API controllers with the builder
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterAssemblyTypes(typeof(IService).Assembly)
                 .AssignableTo<IService>()
                 .AsImplementedInterfaces();

            return builder.Build();
        }
    }
}