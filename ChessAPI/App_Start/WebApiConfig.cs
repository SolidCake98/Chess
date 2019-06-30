using ChessAPI.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ChessAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Конфигурация и службы веб-API

            // Маршруты веб-API
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new AuthorizeAttribute());

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}/{move}",
                defaults: new { id = RouteParameter.Optional, move = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "TokenApi",
                routeTemplate: "api/{controller}/{action}/{username}/{password}",
                defaults: new { username = RouteParameter.Optional, password = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "GameApi",
                routeTemplate: "api/{controller}/{action}/{username}",
                defaults: new { username = RouteParameter.Optional}
            );


            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.None;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
