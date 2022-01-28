using ITRoots.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace ITRoots.Controllers
{
    public class BaseController : Controller
    {
        public UserVM LoggedInUser
        {
            get { return (UserVM)Session[Constants.LoggedInUserSessionKey];  }
        }

        public int? CurrentUserID
        {

            get {
                if (LoggedInUser == null) return null;
                return LoggedInUser.ID;
            }
        }
        public bool LoggedInAsAdmin
        {

            get
            {
                if (LoggedInUser == null) return false;

                return LoggedInUser.UserType == Constants.AdminsRole;
            }
        }

        //public int? CurrentUserID
        //{
        //    if(LoggedInUser == null )
        //    get { return (UserVM)Session[Constants.LoggedInUserSessionKey]; }
        //}

        protected override void OnActionExecuting(
         ActionExecutingContext filterContext)
        {
            // Grab the culture route parameter
            string culture = filterContext.RouteData.Values["culture"]?.ToString() ?? Constants.DefaultLanguageKey;
            // Set the action parameter just in case we didn't get one
            // from the route.
            filterContext.ActionParameters["culture"] = culture;
            var cultureInfo = CultureInfo.GetCultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            // Because we've overwritten the ActionParameters, we
            // make sure we provide the override to the 
            // base implementation.
            base.OnActionExecuting(filterContext);
        }
        public ActionResult RedirectToLocalized()
        {
            return RedirectPermanent("/"+ Constants.DefaultLanguageKey);
        }
    }
  
    }
