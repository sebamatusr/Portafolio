﻿using PortafolioWeb.CustomXML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PortafolioWeb
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(

               name: "DefaultApi2",

               routeTemplate: "api/{controller}/{action}",

               defaults: new { id = RouteParameter.Optional }

           );

            //config.Formatters.Remove(config.Formatters.XmlFormatter);
            //config.Formatters.Add(new CustomXmlFormatter());
        }
    }
}
