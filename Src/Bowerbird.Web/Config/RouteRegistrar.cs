﻿/* Bowerbird V1 - Licensed under MIT 1.1 Public License

 Developers: 
 * Frank Radocaj : frank@radocaj.com
 * Hamish Crittenden : hamish.crittenden@gmail.com
 
 Project Manager: 
 * Ken Walker : kwalker@museum.vic.gov.au
 
 Funded by:
 * Atlas of Living Australia
 
*/

using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using Bowerbird.Web.Controllers;
using System.Collections.Generic;

namespace Bowerbird.Web.Config
{
    public class RouteRegistrar
    {
        #region Fields

        #endregion

        #region Constructors

        #endregion

        #region Properties

        #endregion

        #region Methods

        public static void RegisterRoutesTo(RouteCollection routes)
        {
            routes.IgnoreRoute(
                "{resource}.axd/{*pathInfo}");

            routes.IgnoreRoute(
                "{*favicon}",
                new {favicon = @"(.*/)?favicon.ico(/.*)?"});

            routes.MapRoute(
                "home-privateindex",
                "",
                new {controller = "home", action = "privateindex"},
                new {authorised = new AuthenticatedConstraint()});

            routes.MapRoute(
                "home-publicindex",
                "",
                new {controller = "home", action = "publicindex"});

            routes.MapRoute(
                "home-activity",
                "activity",
                new { controller = "home", action = "activity" },
                new { acceptType = new AcceptTypeContstraint("application/json", "text/javascript") });

            routes.MapRoute(
                "account-resetpassword",
                "account/resetpassword/{resetpasswordkey}",
                new { controller = "account", action = "resetpassword", resetpasswordkey = UrlParameter.Optional });

            // Load up restful controllers and create routes based on method name conventions
            RegisterRestfulControllerRouteConventions(routes);

            routes.MapRoute(
                "default",
                "{controller}/{action}",
                new {controller = "home", action = "index"});
        }

        private static void RegisterRestfulControllerRouteConventions(RouteCollection routes)
        {
            var controllers = typeof(RouteRegistrar).Assembly.GetTypes().Where(x => x.BaseType == typeof(Bowerbird.Web.Controllers.ControllerBase));

            foreach (var controller in controllers)//.Where(controller => controller.GetCustomAttributes(true).Any(x => x is RestfulAttribute)))
            {
                CreateRestfulControllerRoutes(routes, controller.Name.ToLower().Replace("controller", "").Trim(), controller.GetMethods().Select(x => x.Name.ToLower()));
            }
        }

        private static void CreateRestfulControllerRoutes(RouteCollection routes, string controllerName, IEnumerable<string> controllerMethods)
        {
            if (controllerMethods.Contains("list"))
            {
                /* 
                 * Eg: "/users" HTML/JSON GET
                 * Used to get many users based one some filter criteria as HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-list",
                    controllerName,
                    new { controller = controllerName, action = "list" },
                    new { httpMethod = new HttpMethodConstraint("GET") });
            }

            if (controllerMethods.Contains("createform"))
            {
                /* 
                 * Eg: "/users/create" HTML/JSON GET
                 * Used to get create form data as HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-create-form",
                    controllerName + "/create",
                    new { controller = controllerName, action = "createform" },
                    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("GET") });
            }

            if (controllerMethods.Contains("updateform"))
            {
                /* 
                 * Eg: "/users/update" HTML/JSON GET
                 * Used to get update form data based on an ID as HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-update-form",
                    controllerName + "/{id}/update",
                    new { controller = controllerName, action = "updateform" },
                    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("GET") });
            }

            if (controllerMethods.Contains("deleteform"))
            {
                /* 
                 * Eg: "/users/delete" HTML/JSON GET
                 * Used to get delete form data based on an ID as HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-delete-form",
                    controllerName + "/{id}/delete",
                    new { controller = controllerName, action = "deleteform" },
                    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("GET") });
            }

            /* 
            * Eg: "/users/2/activity" HTML/JSON GET
            * Used to get a page sub-section based on an ID as HTML or JSON output
            */
            routes.MapRoute(
                controllerName + "-section",
                controllerName + "/{id}/{action}",
                new { controller = controllerName },
                new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("GET") });

            if (controllerMethods.Contains("index"))
            {
                /* 
                 * Eg: "/users/2" HTML/JSON GET
                 * Used to get one user based on an ID as HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-index",
                    controllerName + "/{id}",
                    new { controller = controllerName, action = "index" },
                    new { httpMethod = new HttpMethodConstraint("GET") });
            }

            if (controllerMethods.Contains("update"))
            {
                /* 
                 * Eg: "/users/2" HTML/JSON PUT 
                 * Used to update a user based on an ID with HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-update",
                    controllerName + "/{id}",
                    new { controller = controllerName, action = "update" },
                    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("PUT") });
            }

            if (controllerMethods.Contains("create"))
            {
                /* 
                 * Eg: "/users/2" HTML/JSON POST 
                 * Used to create a user with HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-create",
                    controllerName,
                    new { controller = controllerName, action = "create" },
                    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("POST") });
            }

            if (controllerMethods.Contains("delete"))
            {
                /* 
                 * Eg: "/users/2" HTML/JSON DELETE 
                 * Used to delete a user based on an ID with HTML or JSON output
                 */
                routes.MapRoute(
                    controllerName + "-delete",
                    controllerName + "/{id}",
                    new { controller = controllerName, action = "delete" },
                    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("DELETE") });
            }







            //// Restful get many method
            //routes.MapRoute(
            //    controllerName + "-get-many",
            //    controllerName,
            //    new { controller = controllerName, action = "getmany" },
            //    new { httpMethod = new HttpMethodConstraint("GET") });

            //// Restful get one method
            //routes.MapRoute(
            //    controllerName + "-get-one",
            //    controllerName + "/{id}", 
            //    new { controller = controllerName, action = "getone" },
            //    new { httpMethod = new HttpMethodConstraint("GET"), id = @"^((?!create|update|delete|explore).*)$" });

            //// Non-restful utility method to get data for displaying form
            //routes.MapRoute(
            //    controllerName + "-create-form",
            //    controllerName + "/create",
            //    new { controller = controllerName, action = "createform" },
            //    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("GET") });

            //// Non-restful utility method to get data for displaying form
            //routes.MapRoute(
            //    controllerName + "-update-form",
            //    controllerName + "/{id}/update",
            //    new { controller = controllerName, action = "updateform" },
            //    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("GET"), id = @"^((?!create|update|delete).*)$" });

            //// Non-restful utility method to get data for displaying form
            //routes.MapRoute(
            //    controllerName + "-delete-form",
            //    controllerName + "/{id}/delete",
            //    new { controller = controllerName, action = "deleteform" },
            //    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("GET"), id = @"^((?!create|update|delete).*)$" });

            //// Restful update method
            //routes.MapRoute(
            //    controllerName + "-update",
            //    controllerName + "/{id}",
            //    new { controller = controllerName, action = "update" },
            //    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("PUT") },
            //    new[] { "Bowerbird.Web.Controllers" });

            //// Restful create method
            //routes.MapRoute(
            //    controllerName + "-create",
            //    controllerName + "/",
            //    new { controller = controllerName, action = "create" },
            //    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("POST") },
            //    new[] { "Bowerbird.Web.Controllers" });

            //// Restful delete method
            //routes.MapRoute(
            //    controllerName + "-delete",
            //    controllerName + "/{id}",
            //    new { controller = controllerName, action = "delete" },
            //    new { authorised = new AuthenticatedConstraint(), httpMethod = new HttpMethodConstraint("DELETE") },
            //    new[] { "Bowerbird.Web.Controllers" });
        }

        #endregion
    }
}
