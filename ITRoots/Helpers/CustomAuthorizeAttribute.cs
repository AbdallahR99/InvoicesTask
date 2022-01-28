using ITRoots.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ITRoots.Helpers
{
 
        public class CustomAuthorizeAttribute : AuthorizeAttribute
        {
            private readonly string[] allowedroles;

            public CustomAuthorizeAttribute(params string[] roles)
            {
                this.allowedroles = roles;
            }
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                bool authorize = false;
                var user = (UserVM)httpContext.Session[Constants.LoggedInUserSessionKey];
                if (user != null)
                    using (var context = new db_a79052_rootdbEntities())
                    {
                    var userRole = context.Users_Roles
                        .Where(a => a.UserId == user.ID).FirstOrDefault();

                    if (userRole == null) return authorize;

                        foreach (var role in allowedroles)
                        {
                            if (role == userRole.Roles.Name) return true;
                        }
                    }


                return authorize;
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                    { "controller", "Account" },
                    { "action", "UnAuthorized" }
                   });
            }
        }
    }
